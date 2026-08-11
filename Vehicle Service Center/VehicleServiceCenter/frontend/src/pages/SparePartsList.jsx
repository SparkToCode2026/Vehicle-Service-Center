import { useEffect, useState } from "react";
import {
  filterSparePartsByAvailability,
  getAllSpareParts,
  getSparePartById,
  getSparePartsSortedByPrice,
} from "../api/sparePartApi";

function formatAmount(amount) {
  return Number(amount || 0).toFixed(2);
}

function SparePartsList() {
  const [spareParts, setSpareParts] = useState([]);
  const [availability, setAvailability] = useState("all");
  const [selectedPart, setSelectedPart] = useState(null);
  const [loading, setLoading] = useState(true);
  const [detailsLoading, setDetailsLoading] = useState(false);
  const [error, setError] = useState("");

  async function loadAllSpareParts() {
    try {
      setLoading(true);
      setError("");

      const response = await getAllSpareParts();
      setSpareParts(response.data);
    } catch {
      setError("Could not load the spare parts.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadAllSpareParts();
  }, []);

  async function handleAvailabilityChange(event) {
    const selectedAvailability = event.target.value;
    setAvailability(selectedAvailability);

    if (selectedAvailability === "all") {
      await loadAllSpareParts();
      return;
    }

    try {
      setLoading(true);
      setError("");

      const isAvailable = selectedAvailability === "available";
      const response = await filterSparePartsByAvailability(isAvailable);

      setSpareParts(response.data);
    } catch {
      setError("Could not filter the spare parts.");
    } finally {
      setLoading(false);
    }
  }

  async function handleSortByPrice() {
    try {
      setLoading(true);
      setError("");
      setAvailability("all");

      const response = await getSparePartsSortedByPrice();
      setSpareParts(response.data);
    } catch {
      setError("Could not sort the spare parts.");
    } finally {
      setLoading(false);
    }
  }

  async function handleViewDetails(sparePartId) {
    try {
      setDetailsLoading(true);
      setError("");

      const response = await getSparePartById(sparePartId);
      setSelectedPart(response.data);
    } catch {
      setError("Could not load the spare-part details.");
    } finally {
      setDetailsLoading(false);
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h2 className="mb-1">Spare Parts</h2>
          <p className="text-secondary mb-0">
            View spare-part prices and stock levels
          </p>
        </div>

        <div className="d-flex gap-2">
          <select
            className="form-select"
            aria-label="Filter by availability"
            value={availability}
            onChange={handleAvailabilityChange}
          >
            <option value="all">All parts</option>
            <option value="available">Available</option>
            <option value="unavailable">Unavailable</option>
          </select>

          <button
            className="btn btn-outline-primary text-nowrap"
            type="button"
            onClick={handleSortByPrice}
          >
            Sort by Price
          </button>
        </div>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {selectedPart && (
        <div className="card border-primary shadow-sm mb-4">
          <div className="card-header bg-white d-flex justify-content-between align-items-center">
            <h3 className="h5 mb-0">{selectedPart.partName}</h3>
            <button
              className="btn-close"
              type="button"
              aria-label="Close details"
              onClick={() => setSelectedPart(null)}
            />
          </div>

          <div className="card-body">
            <div className="row g-3">
              <div className="col-md-3">
                <p className="text-secondary mb-1">Part Number</p>
                <p className="mb-0">{selectedPart.partNumber}</p>
              </div>
              <div className="col-md-3">
                <p className="text-secondary mb-1">Branch</p>
                <p className="mb-0">
                  {selectedPart.branch?.branchName ||
                    `Branch #${selectedPart.branchId}`}
                </p>
              </div>
              <div className="col-md-2">
                <p className="text-secondary mb-1">Unit Price</p>
                <p className="mb-0">
                  {formatAmount(selectedPart.unitPrice)}
                </p>
              </div>
              <div className="col-md-2">
                <p className="text-secondary mb-1">Stock</p>
                <p className="mb-0">{selectedPart.stockQuantity}</p>
              </div>
              <div className="col-md-2">
                <p className="text-secondary mb-1">Reorder Level</p>
                <p className="mb-0">{selectedPart.reorderLevel}</p>
              </div>
              <div className="col-12">
                <p className="text-secondary mb-1">Description</p>
                <p className="mb-0">
                  {selectedPart.description || "No description provided"}
                </p>
              </div>
            </div>
          </div>
        </div>
      )}

      {detailsLoading && (
        <div className="alert alert-info">Loading part details...</div>
      )}

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-2">Loading spare parts...</p>
        </div>
      ) : (
        <div className="card shadow-sm">
          <div className="card-body p-0">
            {spareParts.length === 0 ? (
              <p className="text-secondary text-center p-4 mb-0">
                No spare parts were found.
              </p>
            ) : (
              <div className="table-responsive">
                <table className="table table-hover align-middle mb-0">
                  <thead className="table-light">
                    <tr>
                      <th>Part</th>
                      <th>Part Number</th>
                      <th>Branch</th>
                      <th>Unit Price</th>
                      <th>Stock</th>
                      <th>Availability</th>
                      <th>Action</th>
                    </tr>
                  </thead>
                  <tbody>
                    {spareParts.map((part) => {
                      const hasLowStock =
                        part.stockQuantity <= part.reorderLevel;

                      return (
                        <tr key={part.sparePartId}>
                          <td>
                            <strong>{part.partName}</strong>
                            {hasLowStock && (
                              <span className="badge text-bg-warning ms-2">
                                Low stock
                              </span>
                            )}
                          </td>
                          <td>{part.partNumber}</td>
                          <td>
                            {part.branch?.branchName ||
                              `Branch #${part.branchId}`}
                          </td>
                          <td>{formatAmount(part.unitPrice)}</td>
                          <td>{part.stockQuantity}</td>
                          <td>
                            <span
                              className={`badge text-bg-${
                                part.isAvailable ? "success" : "secondary"
                              }`}
                            >
                              {part.isAvailable ? "Available" : "Unavailable"}
                            </span>
                          </td>
                          <td>
                            <button
                              className="btn btn-outline-primary btn-sm"
                              type="button"
                              disabled={detailsLoading}
                              onClick={() =>
                                handleViewDetails(part.sparePartId)
                              }
                            >
                              View
                            </button>
                          </td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      )}
    </section>
  );
}

export default SparePartsList;
