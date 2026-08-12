import { useCallback, useEffect, useMemo, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import { getUsers } from "../../api/authApi";
import { getBranches } from "../../api/branchApi";
import {
  createMechanicProfile,
  deleteMechanicProfile,
  getMechanicProfiles,
  getMechanicsByBranch,
  sortMechanicsByExperience,
  updateMechanicAvailability,
  updateMechanicProfile,
} from "../../api/mechanicProfileApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

function MechanicManagement() {
  const [users, setUsers] = useState([]);
  const [branches, setBranches] = useState([]);
  const [branchFilter, setBranchFilter] = useState("");
  const loadItems = useCallback(() => getMechanicProfiles(), []);

  useEffect(() => {
    Promise.all([getUsers(), getBranches()]).then(([userResponse, branchResponse]) => {
      setUsers(userResponse.data.filter((user) => user.role === "Mechanic"));
      setBranches(branchResponse.data);
    });
  }, []);

  const fields = useMemo(() => [
    { name: "userId", label: "Mechanic user", type: "select", required: true, options: users.map((user) => ({ value: user.userId, label: `${user.userName} (#${user.userId})` })) },
    { name: "branchId", label: "Branch", type: "select", required: true, options: branches.map((branch) => ({ value: branch.branchId, label: branch.branchName })) },
    { name: "specialization", label: "Specialization", required: true },
    { name: "experienceYears", label: "Experience years", type: "number", min: 0, required: true },
    { name: "hireDate", label: "Hire date", type: "date", required: true },
    { name: "isAvailable", label: "Available", type: "checkbox" },
  ], [users, branches]);

  const initialValues = useMemo(() => ({ userId: "", branchId: "", specialization: "", experienceYears: 0, hireDate: "", isAvailable: true }), []);
  const toPayload = (values) => ({ ...values, userId: Number(values.userId), branchId: Number(values.branchId), experienceYears: Number(values.experienceYears) });

  return <EntityManager
    title="Mechanic Profiles"
    description="Create profiles, assign branches, edit skills, and manage availability."
    idKey="mechanicProfileId"
    loadItems={loadItems}
    createItem={createMechanicProfile}
    updateItem={updateMechanicProfile}
    deleteItem={deleteMechanicProfile}
    initialValues={initialValues}
    fields={fields}
    columns={[
      { key: "mechanicProfileId", label: "ID" }, { key: "user.userName", label: "Mechanic" },
      { key: "branch.branchName", label: "Branch" }, { key: "specialization", label: "Specialization" },
      { key: "experienceYears", label: "Experience" }, { key: "isAvailable", label: "Available" },
    ]}
    prepareCreate={toPayload}
    prepareUpdate={toPayload}
    actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
      try { await updateMechanicAvailability(item.mechanicProfileId, !item.isAvailable); reload(); }
      catch (error) { setError(getApiErrorMessage(error)); }
    }}>Toggle availability</button>}
    toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4"><div className="row g-2 align-items-end">
      <div className="col-md-5"><label className="form-label" htmlFor="mechanic-branch-filter">Branch</label><select id="mechanic-branch-filter" className="form-select" value={branchFilter} onChange={(event) => setBranchFilter(event.target.value)}><option value="">Select branch</option>{branches.map((branch) => <option value={branch.branchId} key={branch.branchId}>{branch.branchName}</option>)}</select></div>
      <div className="col-md-7 d-flex flex-wrap gap-2"><button className="btn btn-outline-primary" disabled={!branchFilter} type="button" onClick={async () => { try { setItems((await getMechanicsByBranch(branchFilter)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Filter</button><button className="btn btn-outline-info" type="button" onClick={async () => { try { setItems((await sortMechanicsByExperience(true)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Most experienced</button><button className="btn btn-outline-secondary" type="button" onClick={reload}>Reset</button></div>
    </div></div>}
  />;
}

export default MechanicManagement;
