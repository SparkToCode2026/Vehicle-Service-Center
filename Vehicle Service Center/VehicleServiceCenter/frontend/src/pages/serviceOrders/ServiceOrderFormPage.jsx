import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { createServiceOrder, getServiceOrderById, updateServiceOrder } from "../../api/serviceOrderApi";
import { getMechanicProfileByUserId } from "../../api/mechanicProfileApi";
import { useAuth } from "../../context/AuthContext";
import { getApiErrorMessage } from "../../utils/httpErrors";

const blankForm = {
  appointmentId: "", customerProfileId: "", vehicleId: "", mechanicProfileId: "",
  branchId: "", diagnosis: "", customerComplaint: "", totalAmount: 0, status: "Pending",
};

function ServiceOrderFormPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { user } = useAuth();
  const [form, setForm] = useState(blankForm);
  const [loading, setLoading] = useState(Boolean(id));
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        setLoading(true);
        setError("");
        if (id) {
          const response = await getServiceOrderById(id);
          setForm({ ...blankForm, ...response.data });
        } else if (user?.role === "Mechanic") {
          const response = await getMechanicProfileByUserId(user.userId);
          setForm((current) => ({ ...current, mechanicProfileId: response.data.mechanicProfileId }));
        }
      } catch (requestError) {
        setError(getApiErrorMessage(requestError, "Could not load the service order."));
      } finally {
        setLoading(false);
      }
    }
    load();
  }, [id, user]);

  function change(event) {
    setForm((current) => ({ ...current, [event.target.name]: event.target.value }));
  }

  async function submit(event) {
    event.preventDefault();
    const payload = {
      ...form,
      appointmentId: form.appointmentId ? Number(form.appointmentId) : null,
      customerProfileId: Number(form.customerProfileId), vehicleId: Number(form.vehicleId),
      mechanicProfileId: form.mechanicProfileId ? Number(form.mechanicProfileId) : null,
      branchId: Number(form.branchId), totalAmount: Number(form.totalAmount),
    };
    try {
      setSaving(true); setError("");
      if (id) await updateServiceOrder(id, payload);
      else await createServiceOrder(payload);
      navigate("/service-orders");
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not save the service order."));
    } finally { setSaving(false); }
  }

  if (loading) return <div className="text-center py-5"><div className="spinner-border text-primary" /></div>;

  const isMechanic = user?.role === "Mechanic";
  const isMechanicEdit = isMechanic && Boolean(id);
  const numericFields = isMechanicEdit
    ? [
        { name: "totalAmount", label: "Total amount", required: true },
      ]
    : [
        { name: "customerProfileId", label: "Customer profile ID", required: true },
        { name: "vehicleId", label: "Vehicle ID", required: true },
        { name: "branchId", label: "Branch ID", required: true },
        ...(!isMechanic
          ? [{ name: "mechanicProfileId", label: "Mechanic profile ID" }]
          : []),
        { name: "appointmentId", label: "Appointment ID" },
        { name: "totalAmount", label: "Total amount", required: true },
      ];

  const formDescription = isMechanicEdit
    ? "Update the diagnosis and working total for your assigned order."
    : isMechanic
      ? "Create a service order that will automatically be assigned to you."
      : "Assign the vehicle, customer, branch, mechanic, and working amounts.";

  return <section><div className="mb-4"><h2>{id ? `Edit Service Order #${id}` : "Create Service Order"}</h2><p className="text-secondary">{formDescription}</p></div>
    {error && <div className="alert alert-danger">{error}</div>}
    {isMechanicEdit && <div className="alert alert-info">Customer #{form.customerProfileId}, vehicle #{form.vehicleId}, branch #{form.branchId}. Assignment details can only be changed by an administrator.</div>}
    <div className="card shadow-sm"><form className="card-body" onSubmit={submit}><div className="row g-3">
      {numericFields.map((field) => <div className="col-md-4" key={field.name}><label className="form-label" htmlFor={`order-${field.name}`}>{field.label}</label><input id={`order-${field.name}`} name={field.name} className="form-control" type="number" min={field.name === "totalAmount" ? "0" : "1"} step={field.name === "totalAmount" ? "0.01" : "1"} required={field.required} value={form[field.name] ?? ""} onChange={change} /></div>)}
      {!isMechanicEdit && <div className="col-md-6"><label className="form-label" htmlFor="order-complaint">Customer complaint</label><textarea id="order-complaint" name="customerComplaint" className="form-control" value={form.customerComplaint || ""} onChange={change} /></div>}
      <div className="col-md-6"><label className="form-label" htmlFor="order-diagnosis">Diagnosis</label><textarea id="order-diagnosis" name="diagnosis" className="form-control" value={form.diagnosis || ""} onChange={change} /></div>
    </div><div className="d-flex gap-2 mt-4"><button className="btn btn-primary" disabled={saving}>{saving ? "Saving..." : "Save order"}</button><button className="btn btn-outline-secondary" type="button" onClick={() => navigate(-1)}>Cancel</button></div></form></div>
  </section>;
}

export default ServiceOrderFormPage;
