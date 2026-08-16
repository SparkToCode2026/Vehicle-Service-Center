import { useCallback, useEffect, useState } from "react";
import {
  createServiceOrderItem, deleteServiceOrderItem, filterServiceOrderItems,
  getServiceOrderTotal, updateServiceOrderItem, updateServiceOrderItemQuantity,
} from "../../api/serviceOrderItemApi";
import { getApiErrorMessage } from "../../utils/httpErrors";
import {
  formatServiceOrderItemType,
  normalizeServiceOrderItemType,
} from "../../utils/serviceOrderValues";

const blank = { itemType: "Service", serviceTypeId: "", sparePartId: "", description: "", quantity: 1, unitPrice: 0, laborHours: "" };

function ServiceOrderItemManager({ serviceOrderId }) {
  const [items, setItems] = useState([]);
  const [total, setTotal] = useState(0);
  const [itemTypeFilter, setItemTypeFilter] = useState("");
  const [form, setForm] = useState(blank);
  const [editingId, setEditingId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState("");

  const load = useCallback(async (itemType = "") => {
    try {
      const [itemsResponse, totalResponse] = await Promise.all([
        filterServiceOrderItems({ serviceOrderId }),
        getServiceOrderTotal(serviceOrderId),
      ]);
      const normalizedItems = itemsResponse.data.map((item) => ({
        ...item,
        itemType: normalizeServiceOrderItemType(item.itemType),
      }));
      setItems(
        itemType
          ? normalizedItems.filter((item) => item.itemType === itemType)
          : normalizedItems
      );
      setTotal(totalResponse.data.total);
    } catch (requestError) { setError(getApiErrorMessage(requestError, "Could not load order items.")); }
  }, [serviceOrderId]);

  useEffect(() => { load(); }, [load]);

  function edit(item) {
    setEditingId(item.serviceOrderItemId);
    setForm({ ...blank, ...item, itemType: normalizeServiceOrderItemType(item.itemType), serviceTypeId: item.serviceTypeId || "", sparePartId: item.sparePartId || "", laborHours: item.laborHours || "" });
    setShowForm(true);
  }

  async function submit(event) {
    event.preventDefault();
    const payload = { ...form, serviceOrderId: Number(serviceOrderId), serviceTypeId: form.serviceTypeId ? Number(form.serviceTypeId) : null, sparePartId: form.sparePartId ? Number(form.sparePartId) : null, quantity: Number(form.quantity), unitPrice: Number(form.unitPrice), laborHours: form.laborHours === "" ? null : Number(form.laborHours), subtotal: Number(form.quantity) * Number(form.unitPrice) };
    try {
      if (editingId) await updateServiceOrderItem(editingId, payload); else await createServiceOrderItem(payload);
      setForm(blank); setEditingId(null); setShowForm(false); setError(""); await load(itemTypeFilter);
    } catch (requestError) { setError(getApiErrorMessage(requestError, "Could not save the order item.")); }
  }

  return <div className="card shadow-sm mb-4"><div className="card-header bg-white d-flex flex-wrap justify-content-between align-items-center gap-2"><div><h3 className="h5 mb-0">Manage Order Items</h3><small className="text-secondary">API-calculated total: {Number(total).toFixed(2)}</small></div><button className="btn btn-primary btn-sm" type="button" onClick={() => { setForm(blank); setEditingId(null); setShowForm(true); }}>Add item</button></div>
    <div className="card-body">
      {error && <div className="alert alert-danger">{error}</div>}
      <div className="d-flex gap-2 mb-3"><select className="form-select filter-control-sm" value={itemTypeFilter} onChange={(event) => { const value = event.target.value; setItemTypeFilter(value); load(value); }}><option value="">All item types</option><option value="Service">Service</option><option value="SparePart">Spare part</option></select></div>
      {showForm && <form className="border rounded p-3 mb-3" onSubmit={submit}><div className="row g-2">
        <div className="col-md-3"><label className="form-label" htmlFor="item-type">Item type</label><select id="item-type" className="form-select" value={form.itemType} onChange={(event) => setForm({...form,itemType:event.target.value,serviceTypeId:"",sparePartId:""})}><option value="Service">Service</option><option value="SparePart">Spare part</option></select></div>
        <div className="col-md-3"><label className="form-label" htmlFor="item-service">Service type ID</label><input id="item-service" className="form-control" type="number" min="1" required={form.itemType === "Service"} disabled={form.itemType !== "Service"} value={form.serviceTypeId} onChange={(event) => setForm({...form,serviceTypeId:event.target.value})} /></div>
        <div className="col-md-3"><label className="form-label" htmlFor="item-part">Spare part ID</label><input id="item-part" className="form-control" type="number" min="1" required={form.itemType === "SparePart"} disabled={form.itemType !== "SparePart"} value={form.sparePartId} onChange={(event) => setForm({...form,sparePartId:event.target.value})} /></div>
        <div className="col-md-3"><label className="form-label" htmlFor="item-quantity">Quantity</label><input id="item-quantity" className="form-control" type="number" min="1" required value={form.quantity} onChange={(event) => setForm({...form,quantity:event.target.value})} /></div>
        <div className="col-md-3"><label className="form-label" htmlFor="item-price">Unit price</label><input id="item-price" className="form-control" type="number" min="0" step="0.01" required value={form.unitPrice} onChange={(event) => setForm({...form,unitPrice:event.target.value})} /></div>
        <div className="col-md-3"><label className="form-label" htmlFor="item-hours">Labor hours</label><input id="item-hours" className="form-control" type="number" min="0" step="0.25" value={form.laborHours} onChange={(event) => setForm({...form,laborHours:event.target.value})} /></div>
        <div className="col-md-6"><label className="form-label" htmlFor="item-description">Description</label><input id="item-description" className="form-control" value={form.description || ""} onChange={(event) => setForm({...form,description:event.target.value})} /></div>
      </div><div className="d-flex gap-2 mt-3"><button className="btn btn-primary btn-sm">Save item</button><button className="btn btn-outline-secondary btn-sm" type="button" onClick={() => setShowForm(false)}>Cancel</button></div></form>}
      <div className="table-responsive"><table className="table table-sm align-middle"><thead><tr><th>Type</th><th>Description</th><th>Qty</th><th>Price</th><th>Subtotal</th><th>Actions</th></tr></thead><tbody>{items.map((item) => <tr key={item.serviceOrderItemId}><td>{formatServiceOrderItemType(item.itemType)}</td><td>{item.description || "-"}</td><td>{item.quantity}</td><td>{Number(item.unitPrice).toFixed(2)}</td><td>{Number(item.subtotal).toFixed(2)}</td><td><div className="d-flex flex-wrap gap-1"><button className="btn btn-outline-primary btn-sm" type="button" onClick={() => edit(item)}>Edit</button><button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => { const quantity = window.prompt("New quantity:", item.quantity); if (!quantity) return; try { await updateServiceOrderItemQuantity(item.serviceOrderItemId, Number(quantity)); load(itemTypeFilter); } catch (requestError) { setError(getApiErrorMessage(requestError)); } }}>Quantity</button><button className="btn btn-outline-danger btn-sm" type="button" onClick={async () => { if (!window.confirm("Delete this order item?")) return; try { await deleteServiceOrderItem(item.serviceOrderItemId); load(itemTypeFilter); } catch (requestError) { setError(getApiErrorMessage(requestError)); } }}>Delete</button></div></td></tr>)}</tbody></table></div>
    </div></div>;
}

export default ServiceOrderItemManager;
