import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  changeBranchStatus,
  createBranch,
  deleteBranch,
  getBranches,
  getBranchStatusSummary,
  sortBranches,
  updateBranch,
} from "../../api/branchApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = {
  branchName: "", address: "", phoneNumber: "", email: "",
  openingTime: "08:00", closingTime: "17:00", isActive: true,
};

function toPayload(values) {
  return {
    ...values,
    openingTime: values.openingTime?.length === 5 ? `${values.openingTime}:00` : values.openingTime,
    closingTime: values.closingTime?.length === 5 ? `${values.closingTime}:00` : values.closingTime,
  };
}

function BranchManagement() {
  const [summary, setSummary] = useState([]);
  const loadItems = useCallback(() => getBranches(), []);

  return (
    <EntityManager
      title="Branch Management"
      description="Maintain locations, opening hours, status, and branch summaries."
      idKey="branchId"
      loadItems={loadItems}
      createItem={createBranch}
      updateItem={updateBranch}
      deleteItem={deleteBranch}
      initialValues={initialValues}
      normalizeForEdit={(item) => ({ ...item, openingTime: item.openingTime?.slice(0, 5), closingTime: item.closingTime?.slice(0, 5) })}
      prepareCreate={toPayload}
      prepareUpdate={toPayload}
      fields={[
        { name: "branchName", label: "Branch name", required: true },
        { name: "phoneNumber", label: "Phone", required: true },
        { name: "email", label: "Email", type: "email" },
        { name: "address", label: "Address", required: true },
        { name: "openingTime", label: "Opening time", type: "time", required: true },
        { name: "closingTime", label: "Closing time", type: "time", required: true },
        { name: "isActive", label: "Active", type: "checkbox" },
      ]}
      columns={[
        { key: "branchId", label: "ID" },
        { key: "branchName", label: "Branch" },
        { key: "address", label: "Address" },
        { key: "openingTime", label: "Opens" },
        { key: "closingTime", label: "Closes" },
        { key: "isActive", label: "Active" },
      ]}
      actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
        try { await changeBranchStatus(item.branchId, !item.isActive); reload(); }
        catch (error) { setError(getApiErrorMessage(error, "Could not change branch status.")); }
      }}>{item.isActive ? "Deactivate" : "Activate"}</button>}
      toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4">
        <div className="d-flex flex-wrap gap-2">
          <button className="btn btn-outline-primary" type="button" onClick={async () => {
            try { setItems((await sortBranches(false)).data); }
            catch (error) { setError(getApiErrorMessage(error)); }
          }}>Sort A-Z</button>
          <button className="btn btn-outline-info" type="button" onClick={async () => {
            try { setSummary((await getBranchStatusSummary()).data); }
            catch (error) { setError(getApiErrorMessage(error)); }
          }}>Status summary</button>
          <button className="btn btn-outline-secondary" type="button" onClick={reload}>Reset</button>
        </div>
        {summary.length > 0 && <div className="mt-3 d-flex gap-2">{summary.map((item) => <span className="badge text-bg-info" key={String(item.isActive)}>{item.isActive ? "Active" : "Inactive"}: {item.count}</span>)}</div>}
      </div>}
    />
  );
}

export default BranchManagement;
