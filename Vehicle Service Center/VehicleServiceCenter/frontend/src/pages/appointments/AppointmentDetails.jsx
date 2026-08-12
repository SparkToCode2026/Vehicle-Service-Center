import { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { deleteAppointment, getAppointmentById, updateAppointment, updateAppointmentStatus } from "../../api/appointmentApi";
import { useAuth } from "../../context/AuthContext";
import { getApiErrorMessage } from "../../utils/httpErrors";

function AppointmentDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [appointment, setAppointment] = useState(null);
  const [form, setForm] = useState(null);
  const [editing, setEditing] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const load = useCallback(async () => {
    try {
      const response = await getAppointmentById(id);
      setAppointment(response);
      setForm({ ...response, appointmentDate: response.appointmentDate?.slice(0, 16) });
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not load the appointment."));
    }
  }, [id]);

  useEffect(() => { load(); }, [load]);

  async function save(event) {
    event.preventDefault();
    try {
      await updateAppointment(id, {
        ...form,
        customerProfileId: Number(form.customerProfileId),
        vehicleId: Number(form.vehicleId),
        serviceTypeId: Number(form.serviceTypeId),
        mechanicProfileId: form.mechanicProfileId ? Number(form.mechanicProfileId) : null,
        branchId: Number(form.branchId),
        appointmentDate: new Date(form.appointmentDate).toISOString(),
      });
      setEditing(false); setMessage("Appointment updated successfully."); await load();
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not update the appointment."));
    }
  }

  async function changeStatus(status) {
    try {
      await updateAppointmentStatus(id, status);
      setMessage(`Appointment changed to ${status}.`); await load();
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not change appointment status."));
    }
  }

  if (!appointment || !form) {
    return <section>{error ? <div className="alert alert-danger">{error}</div> : <div className="text-center py-5"><div className="spinner-border text-primary" /></div>}</section>;
  }

  const canEdit = ["Admin", "Customer"].includes(user?.role);
  const canSetStatus = ["Admin", "Mechanic"].includes(user?.role);

  return <section>
    <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4"><div><h2>Appointment #{appointment.appointmentId}</h2><p className="text-secondary">{new Date(appointment.appointmentDate).toLocaleString()}</p></div><span className="badge text-bg-info">{appointment.status}</span></div>
    {error && <div className="alert alert-danger">{error}</div>}{message && <div className="alert alert-success">{message}</div>}
    {canSetStatus && <div className="card card-body shadow-sm mb-4"><div className="d-flex flex-wrap gap-2 align-items-center"><strong>Update status:</strong>{["Confirmed", "In Progress", "Completed", "Cancelled"].map((status) => <button type="button" className="btn btn-outline-secondary btn-sm" key={status} onClick={() => changeStatus(status)}>{status}</button>)}</div></div>}
    <div className="card shadow-sm"><div className="card-body">
      {!editing ? <div className="row g-3">{[["Customer profile", appointment.customerProfileId], ["Vehicle", appointment.vehicle ? `${appointment.vehicle.make} ${appointment.vehicle.model}` : `#${appointment.vehicleId}`], ["Service", appointment.serviceType?.name || `#${appointment.serviceTypeId}`], ["Branch", appointment.branch?.branchName || `#${appointment.branchId}`], ["Mechanic", appointment.mechanicProfileId || "Unassigned"], ["Notes", appointment.notes || "-"]].map(([label, value]) => <div className="col-md-4" key={label}><p className="text-secondary mb-1">{label}</p><p>{value}</p></div>)}</div> : <form onSubmit={save}><div className="row g-3">{[
        { name: "customerProfileId", label: "Customer profile ID", type: "number" }, { name: "vehicleId", label: "Vehicle ID", type: "number" }, { name: "serviceTypeId", label: "Service type ID", type: "number" }, { name: "branchId", label: "Branch ID", type: "number" }, { name: "mechanicProfileId", label: "Mechanic profile ID", type: "number" }, { name: "appointmentDate", label: "Appointment date", type: "datetime-local" },
      ].map((field) => <div className="col-md-4" key={field.name}><label className="form-label" htmlFor={`appointment-${field.name}`}>{field.label}</label><input id={`appointment-${field.name}`} className="form-control" type={field.type} required={field.name !== "mechanicProfileId"} value={form[field.name] ?? ""} onChange={(event) => setForm({ ...form, [field.name]: event.target.value })} /></div>)}<div className="col-12"><label className="form-label" htmlFor="appointment-notes">Notes</label><textarea id="appointment-notes" className="form-control" value={form.notes || ""} onChange={(event) => setForm({ ...form, notes: event.target.value })} /></div></div><div className="d-flex gap-2 mt-3"><button className="btn btn-primary">Save</button><button className="btn btn-outline-secondary" type="button" onClick={() => setEditing(false)}>Cancel</button></div></form>}
      <div className="d-flex flex-wrap gap-2 mt-4">{canEdit && !editing && <button className="btn btn-primary" type="button" onClick={() => setEditing(true)}>Edit appointment</button>}{canEdit && <button className="btn btn-outline-danger" type="button" onClick={async () => { if (!window.confirm("Delete this appointment?")) return; try { await deleteAppointment(id); navigate(-1); } catch (requestError) { setError(getApiErrorMessage(requestError)); } }}>Delete</button>}<button className="btn btn-outline-secondary" type="button" onClick={() => navigate(-1)}>Back</button></div>
    </div></div>
  </section>;
}

export default AppointmentDetails;
