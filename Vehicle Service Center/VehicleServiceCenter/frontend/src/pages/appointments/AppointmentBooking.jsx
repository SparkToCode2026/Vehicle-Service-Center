import { useState } from "react";

function AppointmentBooking() {
    // Store the appointment form data
    const [formData, setFormData] = useState({
        vehicleId: "",
        serviceTypeId: "",
        branchId: "",
        appointmentDate: "",
        notes: "",
    });

    // Store validation error messages
    const [errors, setErrors] = useState({});
    
    // Handle changes in the form fields
    function handleChange(event) {
        const { name, value } = event.target;

        setFormData({
            ...formData,
            [name]: value,
        });
        
        

    // Remove the error when the user starts correcting the field
    setErrors({
        ...errors,
        [name]: "",
    });
}

    // Validate the appointment form
    function validateForm() {
        const newErrors = {};

        if (!formData.vehicleId) {
            newErrors.vehicleId = "Please select a vehicle.";
        }

        if (!formData.serviceTypeId) {
            newErrors.serviceTypeId = "Please select a service.";
        }

        if (!formData.branchId) {
            newErrors.branchId = "Please select a branch.";
        }

        if (!formData.appointmentDate) {
            newErrors.appointmentDate =
                "Please select an appointment date and time.";
        }

        setErrors(newErrors);

        return Object.keys(newErrors).length === 0;
    }

    // Handle appointment booking
    function handleSubmit(event) {
        event.preventDefault();

        // Validate the form before submitting
        const isValid = validateForm();

        if (!isValid) {
            return;
        }

        // We will connect this form to the backend API next
        console.log("Appointment data:", formData);
    }

    return (
        <div className="container mt-5">
            {/* Page title */}
            <h2 className="mb-4">Book an Appointment</h2>

            <form onSubmit={handleSubmit}>

                {/* Vehicle selection */}
                <div className="mb-3">
                    <label className="form-label">
                        Vehicle
                    </label>

                    <select
                        className="form-select"
                        name="vehicleId"
                        value={formData.vehicleId}
                        onChange={handleChange}
                    >
                        <option value="">
                            Select your vehicle
                        </option>

                        {/* Vehicle data will come from the API later */}
                        <option value="1">
                            Vehicle 1
                        </option>
                    </select>

                    {errors.vehicleId && (
                        <div className="text-danger mt-1">
                            {errors.vehicleId}
                        </div>
                    )}
                </div>

                {/* Service selection */}
                <div className="mb-3">
                    <label className="form-label">
                        Service
                    </label>

                    <select
                        className="form-select"
                        name="serviceTypeId"
                        value={formData.serviceTypeId}
                        onChange={handleChange}
                    >
                        <option value="">
                            Select a service
                        </option>

                        {/* Service data will come from the API later */}
                        <option value="1">
                            Oil Change
                        </option>
                    </select>

                    {errors.serviceTypeId && (
                        <div className="text-danger mt-1">
                            {errors.serviceTypeId}
                        </div>
                    )}
                </div>

                {/* Branch selection */}
                <div className="mb-3">
                    <label className="form-label">
                        Branch
                    </label>

                    <select
                        className="form-select"
                        name="branchId"
                        value={formData.branchId}
                        onChange={handleChange}
                    >
                        <option value="">
                            Select a branch
                        </option>

                        {/* Branch data will come from the API later */}
                        <option value="1">
                            Main Branch
                        </option>
                    </select>

                    {errors.branchId && (
                        <div className="text-danger mt-1">
                            {errors.branchId}
                        </div>
                    )}
                </div>

                {/* Appointment date and time */}
                <div className="mb-3">
                    <label className="form-label">
                        Appointment Date & Time
                    </label>

                    <input
                        type="datetime-local"
                        className="form-control"
                        name="appointmentDate"
                        value={formData.appointmentDate}
                        onChange={handleChange}
                    />

                    {errors.appointmentDate && (
                        <div className="text-danger mt-1">
                            {errors.appointmentDate}
                        </div>
                    )}
                </div>

                {/* Optional notes */}
                <div className="mb-3">
                    <label className="form-label">
                        Notes
                    </label>

                    <textarea
                        className="form-control"
                        name="notes"
                        rows="4"
                        maxLength="500"
                        placeholder="Add any notes about your appointment..."
                        value={formData.notes}
                        onChange={handleChange}
                    />
                </div>

                {/* Submit button */}
                <button
                    type="submit"
                    className="btn btn-primary"
                >
                    Book Appointment
                </button>

            </form>
        </div>
    );
}

export default AppointmentBooking;