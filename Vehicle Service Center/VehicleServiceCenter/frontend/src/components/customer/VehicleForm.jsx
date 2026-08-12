import { useEffect, useState } from "react";

const currentYear = new Date().getFullYear();

const emptyVehicle = {
  make: "",
  model: "",
  year: "",
  plateNumber: "",
  vin: "",
  color: "",
  mileage: "",
};

function createFormData(vehicle = {}) {
  return {
    make: vehicle.make ?? "",
    model: vehicle.model ?? "",
    year: vehicle.year ?? "",
    plateNumber: vehicle.plateNumber ?? "",
    vin: vehicle.vin ?? "",
    color: vehicle.color ?? "",
    mileage: vehicle.mileage ?? "",
  };
}

function validateVehicle(vehicle) {
  const validationErrors = {};
  const year = Number(vehicle.year);
  const mileage = Number(vehicle.mileage);

  if (!vehicle.make.trim()) {
    validationErrors.make = "Make is required.";
  } else if (vehicle.make.trim().length > 50) {
    validationErrors.make = "Make cannot exceed 50 characters.";
  }

  if (!vehicle.model.trim()) {
    validationErrors.model = "Model is required.";
  } else if (vehicle.model.trim().length > 50) {
    validationErrors.model = "Model cannot exceed 50 characters.";
  }

  if (!vehicle.year) {
    validationErrors.year = "Year is required.";
  } else if (!Number.isInteger(year)) {
    validationErrors.year = "Enter a valid year.";
  } else if (year < 1886 || year > currentYear + 1) {
    validationErrors.year = `Year must be between 1886 and ${currentYear + 1}.`;
  }

  if (!vehicle.plateNumber.trim()) {
    validationErrors.plateNumber = "Plate number is required.";
  } else if (vehicle.plateNumber.trim().length > 30) {
    validationErrors.plateNumber =
      "Plate number cannot exceed 30 characters.";
  }

  if (vehicle.vin.trim().length > 50) {
    validationErrors.vin = "VIN cannot exceed 50 characters.";
  }

  if (vehicle.color.trim().length > 30) {
    validationErrors.color = "Color cannot exceed 30 characters.";
  }

  if (vehicle.mileage !== "" && (!Number.isFinite(mileage) || mileage < 0)) {
    validationErrors.mileage = "Mileage must be zero or greater.";
  }

  return validationErrors;
}

function VehicleForm({
  initialValues = emptyVehicle,
  customerProfileId,
  onSubmit,
  onCancel,
  submitLabel = "Save Vehicle",
  isSubmitting = false,
  error = "",
}) {
  const [formData, setFormData] = useState(() =>
    createFormData(initialValues)
  );
  const [validationErrors, setValidationErrors] = useState({});

  useEffect(() => {
    setFormData(createFormData(initialValues));
    setValidationErrors({});
  }, [initialValues]);

  function handleChange(event) {
    const { name, value } = event.target;

    setFormData((currentData) => ({
      ...currentData,
      [name]: value,
    }));

    setValidationErrors((currentErrors) => ({
      ...currentErrors,
      [name]: "",
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();

    const nextErrors = validateVehicle(formData);

    if (Object.keys(nextErrors).length > 0) {
      setValidationErrors(nextErrors);
      return;
    }

    const vehicleData = {
      customerProfileId:
        customerProfileId ?? initialValues.customerProfileId,
      make: formData.make.trim(),
      model: formData.model.trim(),
      year: Number(formData.year),
      plateNumber: formData.plateNumber.trim(),
      vin: formData.vin.trim() || null,
      color: formData.color.trim() || null,
      mileage:
        formData.mileage === "" ? null : Number(formData.mileage),
    };

    await onSubmit(vehicleData);
  }

  function fieldClass(fieldName) {
    return `form-control${
      validationErrors[fieldName] ? " is-invalid" : ""
    }`;
  }

  return (
    <form onSubmit={handleSubmit} noValidate>
      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      <div className="row g-3">
        <div className="col-md-6">
          <label className="form-label" htmlFor="vehicle-make">
            Make
          </label>
          <input
            id="vehicle-make"
            className={fieldClass("make")}
            type="text"
            name="make"
            value={formData.make}
            onChange={handleChange}
            maxLength={50}
            disabled={isSubmitting}
            required
          />
          {validationErrors.make && (
            <div className="invalid-feedback">{validationErrors.make}</div>
          )}
        </div>

        <div className="col-md-6">
          <label className="form-label" htmlFor="vehicle-model">
            Model
          </label>
          <input
            id="vehicle-model"
            className={fieldClass("model")}
            type="text"
            name="model"
            value={formData.model}
            onChange={handleChange}
            maxLength={50}
            disabled={isSubmitting}
            required
          />
          {validationErrors.model && (
            <div className="invalid-feedback">{validationErrors.model}</div>
          )}
        </div>

        <div className="col-md-4">
          <label className="form-label" htmlFor="vehicle-year">
            Year
          </label>
          <input
            id="vehicle-year"
            className={fieldClass("year")}
            type="number"
            name="year"
            value={formData.year}
            onChange={handleChange}
            min={1886}
            max={currentYear + 1}
            disabled={isSubmitting}
            required
          />
          {validationErrors.year && (
            <div className="invalid-feedback">{validationErrors.year}</div>
          )}
        </div>

        <div className="col-md-4">
          <label className="form-label" htmlFor="vehicle-plate-number">
            Plate Number
          </label>
          <input
            id="vehicle-plate-number"
            className={fieldClass("plateNumber")}
            type="text"
            name="plateNumber"
            value={formData.plateNumber}
            onChange={handleChange}
            maxLength={30}
            disabled={isSubmitting}
            required
          />
          {validationErrors.plateNumber && (
            <div className="invalid-feedback">
              {validationErrors.plateNumber}
            </div>
          )}
        </div>

        <div className="col-md-4">
          <label className="form-label" htmlFor="vehicle-color">
            Color
          </label>
          <input
            id="vehicle-color"
            className={fieldClass("color")}
            type="text"
            name="color"
            value={formData.color}
            onChange={handleChange}
            maxLength={30}
            disabled={isSubmitting}
          />
          {validationErrors.color && (
            <div className="invalid-feedback">{validationErrors.color}</div>
          )}
        </div>

        <div className="col-md-6">
          <label className="form-label" htmlFor="vehicle-vin">
            VIN
          </label>
          <input
            id="vehicle-vin"
            className={fieldClass("vin")}
            type="text"
            name="vin"
            value={formData.vin}
            onChange={handleChange}
            maxLength={50}
            disabled={isSubmitting}
          />
          {validationErrors.vin && (
            <div className="invalid-feedback">{validationErrors.vin}</div>
          )}
        </div>

        <div className="col-md-6">
          <label className="form-label" htmlFor="vehicle-mileage">
            Mileage
          </label>
          <input
            id="vehicle-mileage"
            className={fieldClass("mileage")}
            type="number"
            name="mileage"
            value={formData.mileage}
            onChange={handleChange}
            min={0}
            step="0.01"
            disabled={isSubmitting}
          />
          {validationErrors.mileage && (
            <div className="invalid-feedback">
              {validationErrors.mileage}
            </div>
          )}
        </div>
      </div>

      <div className="d-flex gap-2 mt-4">
        <button
          className="btn btn-primary"
          type="submit"
          disabled={isSubmitting}
        >
          {isSubmitting ? "Saving..." : submitLabel}
        </button>

        {onCancel && (
          <button
            className="btn btn-outline-secondary"
            type="button"
            onClick={onCancel}
            disabled={isSubmitting}
          >
            Cancel
          </button>
        )}
      </div>
    </form>
  );
}

export default VehicleForm;
