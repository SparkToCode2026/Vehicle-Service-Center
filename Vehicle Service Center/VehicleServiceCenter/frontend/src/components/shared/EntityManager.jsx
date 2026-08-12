import { useCallback, useEffect, useMemo, useState } from "react";
import ConfirmationModal from "./ConfirmationModal";
import { getApiErrorMessage } from "../../utils/httpErrors";

function displayValue(value) {
  if (value === null || value === undefined || value === "") return "-";
  if (typeof value === "boolean") return value ? "Yes" : "No";
  if (typeof value === "object") return JSON.stringify(value);
  return String(value);
}

function getNestedValue(item, path) {
  return path.split(".").reduce((value, key) => value?.[key], item);
}

function EntityManager({
  title,
  description,
  idKey,
  fields,
  columns,
  loadItems,
  createItem,
  updateItem,
  deleteItem,
  initialValues,
  prepareCreate = (values) => values,
  prepareUpdate = (values) => values,
  normalizeForEdit = (item) => item,
  actions,
  toolbar,
  createLabel = "Add new",
  emptyMessage = "No records were found.",
}) {
  const blankValues = useMemo(() => initialValues, [initialValues]);
  const [items, setItems] = useState([]);
  const [formValues, setFormValues] = useState(blankValues);
  const [editingItem, setEditingItem] = useState(null);
  const [deletingItem, setDeletingItem] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const reload = useCallback(async () => {
    try {
      setLoading(true);
      setError("");
      const response = await loadItems();
      setItems(response.data || []);
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, `Could not load ${title}.`));
    } finally {
      setLoading(false);
    }
  }, [loadItems, title]);

  useEffect(() => {
    reload();
  }, [reload]);

  function openCreateForm() {
    setEditingItem(null);
    setFormValues(blankValues);
    setShowForm(true);
    setError("");
    setMessage("");
  }

  function openEditForm(item) {
    setEditingItem(item);
    setFormValues({ ...blankValues, ...normalizeForEdit(item) });
    setShowForm(true);
    setError("");
    setMessage("");
  }

  function closeForm() {
    setShowForm(false);
    setEditingItem(null);
    setFormValues(blankValues);
  }

  function updateField(field, event) {
    const value = field.type === "checkbox"
      ? event.target.checked
      : event.target.value;

    setFormValues((current) => ({ ...current, [field.name]: value }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");
      setMessage("");

      if (editingItem) {
        await updateItem(
          editingItem[idKey],
          prepareUpdate(formValues, editingItem)
        );
        setMessage(`${title} record updated successfully.`);
      } else {
        await createItem(prepareCreate(formValues));
        setMessage(`${title} record created successfully.`);
      }

      closeForm();
      await reload();
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, `Could not save ${title}.`));
    } finally {
      setSaving(false);
    }
  }

  async function confirmDelete() {
    try {
      setSaving(true);
      setError("");
      await deleteItem(deletingItem[idKey]);
      setDeletingItem(null);
      setMessage(`${title} record deleted successfully.`);
      await reload();
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, `Could not delete ${title}.`));
      setDeletingItem(null);
    } finally {
      setSaving(false);
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-start gap-3 mb-4">
        <div>
          <h2 className="mb-1">{title}</h2>
          <p className="text-secondary mb-0">{description}</p>
        </div>
        {createItem && (
          <button className="btn btn-primary" type="button" onClick={openCreateForm}>
            {createLabel}
          </button>
        )}
      </div>

      {toolbar?.({ items, setItems, reload, setError, setMessage })}

      {error && <div className="alert alert-danger" role="alert">{error}</div>}
      {message && <div className="alert alert-success" role="status">{message}</div>}

      {showForm && (
        <div className="card shadow-sm mb-4">
          <div className="card-header bg-white d-flex justify-content-between align-items-center">
            <h3 className="h5 mb-0">{editingItem ? "Edit record" : createLabel}</h3>
            <button className="btn-close" type="button" aria-label="Close" onClick={closeForm} />
          </div>
          <form className="card-body" onSubmit={handleSubmit}>
            <div className="row g-3">
              {fields
                .filter((field) => !field.hidden?.(Boolean(editingItem)))
                .map((field) => (
                  <div className={field.columnClass || "col-md-6"} key={field.name}>
                    {field.type === "checkbox" ? (
                      <div className="form-check mt-4">
                        <input
                          className="form-check-input"
                          id={`${title}-${field.name}`}
                          type="checkbox"
                          checked={Boolean(formValues[field.name])}
                          onChange={(event) => updateField(field, event)}
                        />
                        <label className="form-check-label" htmlFor={`${title}-${field.name}`}>
                          {field.label}
                        </label>
                      </div>
                    ) : (
                      <>
                        <label className="form-label" htmlFor={`${title}-${field.name}`}>
                          {field.label}
                        </label>
                        {field.type === "textarea" ? (
                          <textarea
                            className="form-control"
                            id={`${title}-${field.name}`}
                            value={formValues[field.name] ?? ""}
                            required={field.required}
                            maxLength={field.maxLength}
                            minLength={field.minLength}
                            onChange={(event) => updateField(field, event)}
                          />
                        ) : field.type === "select" ? (
                          <select
                            className="form-select"
                            id={`${title}-${field.name}`}
                            value={formValues[field.name] ?? ""}
                            required={field.required}
                            onChange={(event) => updateField(field, event)}
                          >
                            <option value="">Select...</option>
                            {(field.options || []).map((option) => (
                              <option key={option.value} value={option.value}>
                                {option.label}
                              </option>
                            ))}
                          </select>
                        ) : (
                          <input
                            className="form-control"
                            id={`${title}-${field.name}`}
                            type={field.type || "text"}
                            value={formValues[field.name] ?? ""}
                            required={field.required}
                            min={field.min}
                            max={field.max}
                            step={field.step}
                            maxLength={field.maxLength}
                            onChange={(event) => updateField(field, event)}
                          />
                        )}
                        {field.help && <div className="form-text">{field.help}</div>}
                      </>
                    )}
                  </div>
                ))}
            </div>
            <div className="d-flex gap-2 mt-4">
              <button className="btn btn-primary" type="submit" disabled={saving}>
                {saving ? "Saving..." : "Save"}
              </button>
              <button className="btn btn-outline-secondary" type="button" onClick={closeForm}>
                Cancel
              </button>
            </div>
          </form>
        </div>
      )}

      <div className="card shadow-sm">
        <div className="card-body p-0">
          {loading ? (
            <div className="text-center py-5">
              <div className="spinner-border text-primary" role="status" />
              <p className="mt-2">Loading...</p>
            </div>
          ) : items.length === 0 ? (
            <p className="text-secondary text-center p-4 mb-0">{emptyMessage}</p>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover align-middle mb-0">
                <thead className="table-light">
                  <tr>
                    {columns.map((column) => <th key={column.key}>{column.label}</th>)}
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {items.map((item) => (
                    <tr key={item[idKey]}>
                      {columns.map((column) => (
                        <td key={column.key}>
                          {column.render
                            ? column.render(item)
                            : displayValue(getNestedValue(item, column.key))}
                        </td>
                      ))}
                      <td>
                        <div className="d-flex flex-wrap gap-2">
                          {updateItem && (
                            <button className="btn btn-outline-primary btn-sm" type="button" onClick={() => openEditForm(item)}>
                              View / Edit
                            </button>
                          )}
                          {actions?.({ item, reload, setError, setMessage })}
                          {deleteItem && (
                            <button className="btn btn-outline-danger btn-sm" type="button" onClick={() => setDeletingItem(item)}>
                              Delete
                            </button>
                          )}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      <ConfirmationModal
        show={Boolean(deletingItem)}
        title={`Delete ${title} record`}
        message="This action cannot be undone. Continue?"
        confirmLabel={saving ? "Deleting..." : "Delete"}
        onCancel={() => setDeletingItem(null)}
        onConfirm={confirmDelete}
      />
    </section>
  );
}

export default EntityManager;
