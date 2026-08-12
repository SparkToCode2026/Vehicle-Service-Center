import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  changeUserStatus,
  deleteUser,
  filterUsers,
  getUserRoleSummary,
  getUsers,
  registerUser,
  updateUser,
} from "../../api/authApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = {
  userName: "",
  email: "",
  password: "",
  role: "Customer",
  phoneNumber: "",
  isActive: true,
};

function UserManagement() {
  const [roleFilter, setRoleFilter] = useState("");
  const [summary, setSummary] = useState([]);
  const loadItems = useCallback(() => getUsers(), []);
  const createUser = useCallback(async (values) => {
    const registration = await registerUser({ ...values, role: "Customer" });
    await updateUser(registration.data.userId, {
      ...values,
      password: "BindingOnly1!",
    });
    return registration;
  }, []);

  return (
    <EntityManager
      title="User Management"
      description="View accounts, update roles, manage status, and remove users."
      idKey="userId"
      loadItems={loadItems}
      createItem={createUser}
      updateItem={updateUser}
      deleteItem={deleteUser}
      initialValues={initialValues}
      fields={[
        { name: "userName", label: "Name", required: true },
        { name: "email", label: "Email", type: "email", required: true },
        { name: "password", label: "Temporary password", type: "password", required: true, minLength: 8, hidden: (editing) => editing },
        { name: "role", label: "Role", type: "select", required: true, options: [
          { value: "Customer", label: "Customer" },
          { value: "Mechanic", label: "Mechanic" },
          { value: "Admin", label: "Admin" },
        ] },
        { name: "phoneNumber", label: "Phone" },
        { name: "isActive", label: "Active", type: "checkbox" },
      ]}
      columns={[
        { key: "userId", label: "ID" },
        { key: "userName", label: "Name" },
        { key: "email", label: "Email" },
        { key: "role", label: "Role" },
        { key: "isActive", label: "Active" },
      ]}
      prepareCreate={(values) => values}
      prepareUpdate={(values) => ({ ...values, password: "NotChanged1!" })}
      actions={({ item, reload, setError, setMessage }) => (
        <button
          className={`btn btn-sm ${item.isActive ? "btn-outline-warning" : "btn-outline-success"}`}
          type="button"
          onClick={async () => {
            try {
              await changeUserStatus(item.userId, !item.isActive);
              setMessage(`User ${item.isActive ? "deactivated" : "activated"}.`);
              reload();
            } catch (error) {
              setError(getApiErrorMessage(error, "Could not change user status."));
            }
          }}
        >
          {item.isActive ? "Deactivate" : "Activate"}
        </button>
      )}
      toolbar={({ setItems, reload, setError }) => (
        <div className="card card-body shadow-sm mb-4">
          <div className="row g-3 align-items-end">
            <div className="col-md-4">
              <label className="form-label" htmlFor="user-role-filter">Role filter</label>
              <select id="user-role-filter" className="form-select" value={roleFilter} onChange={(event) => setRoleFilter(event.target.value)}>
                <option value="">All roles</option>
                <option value="Customer">Customer</option>
                <option value="Mechanic">Mechanic</option>
                <option value="Admin">Admin</option>
              </select>
            </div>
            <div className="col-md-8 d-flex flex-wrap gap-2">
              <button className="btn btn-outline-primary" type="button" disabled={!roleFilter} onClick={async () => {
                try {
                  const response = await filterUsers(roleFilter);
                  setItems(response.data);
                } catch (error) {
                  setError(getApiErrorMessage(error, "Could not filter users."));
                }
              }}>Apply filter</button>
              <button className="btn btn-outline-secondary" type="button" onClick={reload}>Clear</button>
              <button className="btn btn-outline-info" type="button" onClick={async () => {
                try {
                  const response = await getUserRoleSummary();
                  setSummary(response.data);
                } catch (error) {
                  setError(getApiErrorMessage(error, "Could not load role summary."));
                }
              }}>Role summary</button>
            </div>
          </div>
          {summary.length > 0 && (
            <div className="d-flex flex-wrap gap-2 mt-3">
              {summary.map((item) => <span className="badge text-bg-info" key={item.role}>{item.role}: {item.totalUsers} ({item.activeUsers} active)</span>)}
            </div>
          )}
        </div>
      )}
    />
  );
}

export default UserManagement;
