import { useCallback, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import {
  createCustomerProfile,
  deleteCustomerProfile,
  filterCustomerProfiles,
  getAllCustomerProfiles,
  sortCustomerProfiles,
  updateCustomerProfile,
} from "../../api/customerProfileApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

const initialValues = { userId: "", address: "", dateOfBirth: "" };

function CustomerProfileManagement() {
  const [address, setAddress] = useState("");
  const loadItems = useCallback(() => getAllCustomerProfiles(), []);

  return (
    <EntityManager
      title="Customer Profiles"
      description="Create, inspect, filter, update, sort, and remove customer profiles."
      idKey="customerProfileId"
      loadItems={loadItems}
      createItem={createCustomerProfile}
      updateItem={updateCustomerProfile}
      deleteItem={deleteCustomerProfile}
      initialValues={initialValues}
      fields={[
        { name: "userId", label: "Customer user ID", type: "number", required: true, min: 1 },
        { name: "address", label: "Address", required: true, maxLength: 255 },
        { name: "dateOfBirth", label: "Date of birth", type: "date" },
      ]}
      columns={[
        { key: "customerProfileId", label: "Profile" },
        { key: "userId", label: "User" },
        { key: "user.userName", label: "Customer" },
        { key: "address", label: "Address" },
        { key: "dateOfBirth", label: "Birth date" },
      ]}
      prepareCreate={(values) => ({ ...values, userId: Number(values.userId), dateOfBirth: values.dateOfBirth || null })}
      prepareUpdate={(values) => ({ ...values, userId: Number(values.userId), dateOfBirth: values.dateOfBirth || null })}
      toolbar={({ setItems, reload, setError }) => (
        <div className="card card-body shadow-sm mb-4">
          <div className="row g-2 align-items-end">
            <div className="col-md-5">
              <label className="form-label" htmlFor="profile-address-filter">Address contains</label>
              <input id="profile-address-filter" className="form-control" value={address} onChange={(event) => setAddress(event.target.value)} />
            </div>
            <div className="col-md-7 d-flex flex-wrap gap-2">
              <button className="btn btn-outline-primary" type="button" disabled={!address.trim()} onClick={async () => {
                try { setItems((await filterCustomerProfiles({ address })).data); }
                catch (error) { setError(getApiErrorMessage(error, "Could not filter profiles.")); }
              }}>Filter</button>
              <button className="btn btn-outline-info" type="button" onClick={async () => {
                try { setItems((await sortCustomerProfiles(true)).data); }
                catch (error) { setError(getApiErrorMessage(error, "Could not sort profiles.")); }
              }}>Newest first</button>
              <button className="btn btn-outline-secondary" type="button" onClick={reload}>Clear</button>
            </div>
          </div>
        </div>
      )}
    />
  );
}

export default CustomerProfileManagement;
