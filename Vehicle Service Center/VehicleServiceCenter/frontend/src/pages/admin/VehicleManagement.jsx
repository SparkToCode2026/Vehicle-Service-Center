import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  createVehicle, deleteVehicle, filterVehiclesByMake, getAllVehicles,
  getVehicleCountByMake, reassignVehicle, updateVehicle,
} from "../../api/vehicleApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = { customerProfileId: "", plateNumber: "", vin: "", make: "", model: "", year: new Date().getFullYear(), color: "", mileage: "" };
const toPayload = (values) => ({ ...values, customerProfileId: Number(values.customerProfileId), year: Number(values.year), mileage: values.mileage === "" ? null : Number(values.mileage), createdAt: new Date().toISOString() });

function VehicleManagement() {
  const [make, setMake] = useState("");
  const [summary, setSummary] = useState([]);
  const loadItems = useCallback(() => getAllVehicles(), []);

  return <EntityManager
    title="Vehicle Management"
    description="Manage all vehicles, ownership assignments, filters, and make totals."
    idKey="vehicleId" loadItems={loadItems} createItem={createVehicle} updateItem={updateVehicle} deleteItem={deleteVehicle}
    initialValues={initialValues} prepareCreate={toPayload} prepareUpdate={toPayload}
    fields={[
      { name: "customerProfileId", label: "Customer profile ID", type: "number", min: 1, required: true },
      { name: "plateNumber", label: "Plate number", required: true }, { name: "vin", label: "VIN" },
      { name: "make", label: "Make", required: true }, { name: "model", label: "Model", required: true },
      { name: "year", label: "Year", type: "number", min: 1900, max: 2100, required: true },
      { name: "color", label: "Color" }, { name: "mileage", label: "Mileage", type: "number", min: 0, step: "0.01" },
    ]}
    columns={[
      { key: "vehicleId", label: "ID" }, { key: "plateNumber", label: "Plate" }, { key: "make", label: "Make" },
      { key: "model", label: "Model" }, { key: "year", label: "Year" }, { key: "customerProfileId", label: "Owner profile" },
    ]}
    actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
      const profileId = window.prompt("New customer profile ID:", item.customerProfileId);
      if (!profileId) return;
      try { await reassignVehicle(item.vehicleId, Number(profileId)); reload(); }
      catch (error) { setError(getApiErrorMessage(error, "Could not reassign vehicle.")); }
    }}>Reassign</button>}
    toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4"><div className="row g-2 align-items-end"><div className="col-md-5"><label className="form-label" htmlFor="vehicle-make-filter">Make</label><input id="vehicle-make-filter" className="form-control" value={make} onChange={(event) => setMake(event.target.value)} /></div><div className="col-md-7 d-flex flex-wrap gap-2"><button className="btn btn-outline-primary" type="button" disabled={!make.trim()} onClick={async () => { try { setItems((await filterVehiclesByMake(make)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Filter</button><button className="btn btn-outline-info" type="button" onClick={async () => { try { setSummary((await getVehicleCountByMake()).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Summary</button><button className="btn btn-outline-secondary" type="button" onClick={reload}>Reset</button></div></div>{summary.length > 0 && <div className="d-flex flex-wrap gap-2 mt-3">{summary.map((item) => <span className="badge text-bg-info" key={item.make}>{item.make}: {item.count}</span>)}</div>}</div>}
  />;
}

export default VehicleManagement;
