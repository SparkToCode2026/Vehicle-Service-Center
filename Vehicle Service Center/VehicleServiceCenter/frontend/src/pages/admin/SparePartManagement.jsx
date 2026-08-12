import { useCallback, useEffect, useMemo, useState } from "react";
import EntityManager from "../../components/shared/EntityManager";
import { getBranches } from "../../api/branchApi";
import {
  createSparePart, deleteSparePart, filterSparePartsByAvailability,
  getAllSpareParts, getSparePartsSortedByPrice, updateSparePart, updateSparePartStock,
} from "../../api/sparePartApi";
import { getApiErrorMessage } from "../../utils/httpErrors";

function SparePartManagement() {
  const [branches, setBranches] = useState([]);
  const loadItems = useCallback(() => getAllSpareParts(), []);
  useEffect(() => { getBranches().then((response) => setBranches(response.data)); }, []);
  const initialValues = useMemo(() => ({ branchId: "", partName: "", partNumber: "", description: "", unitPrice: 0, stockQuantity: 0, reorderLevel: 0, isAvailable: true }), []);
  const fields = useMemo(() => [
    { name: "branchId", label: "Branch", type: "select", required: true, options: branches.map((branch) => ({ value: branch.branchId, label: branch.branchName })) },
    { name: "partName", label: "Part name", required: true }, { name: "partNumber", label: "Part number", required: true },
    { name: "unitPrice", label: "Unit price", type: "number", min: 0, step: "0.01", required: true },
    { name: "stockQuantity", label: "Stock", type: "number", min: 0, required: true }, { name: "reorderLevel", label: "Reorder level", type: "number", min: 0, required: true },
    { name: "isAvailable", label: "Available", type: "checkbox" }, { name: "description", label: "Description", type: "textarea", columnClass: "col-12" },
  ], [branches]);
  const toPayload = (values) => ({ ...values, branchId: Number(values.branchId), unitPrice: Number(values.unitPrice), stockQuantity: Number(values.stockQuantity), reorderLevel: Number(values.reorderLevel) });

  return <EntityManager
    title="Spare Part Management" description="Create, edit, restock, filter, sort, and remove inventory items."
    idKey="sparePartId" loadItems={loadItems} createItem={createSparePart} updateItem={updateSparePart} deleteItem={deleteSparePart}
    initialValues={initialValues} fields={fields} prepareCreate={toPayload} prepareUpdate={toPayload}
    columns={[
      { key: "sparePartId", label: "ID" }, { key: "partName", label: "Part" }, { key: "partNumber", label: "Number" },
      { key: "branch.branchName", label: "Branch" }, { key: "unitPrice", label: "Price" },
      { key: "stockQuantity", label: "Stock", render: (item) => <>{item.stockQuantity}{item.stockQuantity <= item.reorderLevel && <span className="badge text-bg-warning ms-2">Low</span>}</> },
    ]}
    actions={({ item, reload, setError }) => <button className="btn btn-outline-warning btn-sm" type="button" onClick={async () => {
      const quantity = window.prompt("New stock quantity:", item.stockQuantity); if (quantity === null) return;
      try { await updateSparePartStock(item.sparePartId, Number(quantity)); reload(); } catch (error) { setError(getApiErrorMessage(error)); }
    }}>Update stock</button>}
    toolbar={({ setItems, reload, setError }) => <div className="card card-body shadow-sm mb-4"><div className="d-flex flex-wrap gap-2"><button className="btn btn-outline-success" type="button" onClick={async () => { try { setItems((await filterSparePartsByAvailability(true)).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Available</button><button className="btn btn-outline-primary" type="button" onClick={async () => { try { setItems((await getSparePartsSortedByPrice()).data); } catch (error) { setError(getApiErrorMessage(error)); } }}>Sort by price</button><button className="btn btn-outline-secondary" type="button" onClick={reload}>Reset</button></div></div>}
  />;
}

export default SparePartManagement;
