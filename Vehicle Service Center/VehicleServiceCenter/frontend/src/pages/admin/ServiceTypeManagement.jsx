import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  changeServiceTypeStatus,
  createServiceType,
  deleteServiceType,
  filterServiceTypes,
  getServiceTypeRevenue,
  getServiceTypes,
  updateServiceType,
} from "../../api/serviceTypeApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = { name: "", description: "", basePrice: "", estimatedDurationMinutes: "", isActive: true };

function ServiceTypeManagement() {
  const [summary, setSummary] = useState([]);
  const loadItems = useCallback(() => getServiceTypes(), []);

  return (
    <EntityManager
      title="Service Types"
      description="Manage the service catalog, availability, pricing, and revenue summary."
      idKey="serviceTypeId"
      loadItems={loadItems}
      createItem={createServiceType}
      updateItem={updateServiceType}
      deleteItem={deleteServiceType}
      initialValues={initialValues}
      fields={[
        { name: "name", label: "Service name", required: true },
        { name: "basePrice", label: "Base price", type: "number", min: 0, step: "0.01", required: true },
        { name: "estimatedDurationMinutes", label: "Duration (minutes)", type: "number", min: 1, required: true },
        { name: "isActive", label: "Active", type: "checkbox" },
        { name: "description", label: "Description", type: "textarea", columnClass: "col-12" },
      ]}
      columns={[
        { key: "serviceTypeId", label: "ID" },
        { key: "name", label: "Service" },
        { key: "basePrice", label: "Price" },
        { key: "estimatedDurationMinutes", label: "Minutes" },
        { key: "isActive", label: "Active" },
      ]}
      prepareCreate={(values) => ({ ...values, basePrice: Number(values.basePrice), estimatedDurationMinutes: Number(values.estimatedDurationMinutes) })}
      prepareUpdate={(values) => ({ ...values, basePrice: Number(values.basePrice), estimatedDurationMinutes: Number(values.estimatedDurationMinutes) })}
      actions={({ item, reload, setError }) => (
        <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
          try { await changeServiceTypeStatus(item.serviceTypeId, !item.isActive); reload(); }
          catch (error) { setError(getApiErrorMessage(error, "Could not change service status.")); }
        }}>{item.isActive ? "Deactivate" : "Activate"}</button>
      )}
      toolbar={({ setItems, reload, setError }) => (
        <div className="card card-body shadow-sm mb-4">
          <div className="d-flex flex-wrap gap-2">
            <button className="btn btn-outline-success" type="button" onClick={async () => {
              try { setItems((await filterServiceTypes(true)).data); }
              catch (error) { setError(getApiErrorMessage(error)); }
            }}>Active only</button>
            <button className="btn btn-outline-secondary" type="button" onClick={reload}>All services</button>
            <button className="btn btn-outline-info" type="button" onClick={async () => {
              try { setSummary((await getServiceTypeRevenue()).data); }
              catch (error) { setError(getApiErrorMessage(error, "Could not load revenue.")); }
            }}>Revenue summary</button>
          </div>
          {summary.length > 0 && <div className="row g-2 mt-2">{summary.map((item) => <div className="col-md-4" key={item.serviceTypeId}><div className="border rounded p-2">{item.name}: {Number(item.totalRevenue).toFixed(2)}</div></div>)}</div>}
        </div>
      )}
    />
  );
}

export default ServiceTypeManagement;
