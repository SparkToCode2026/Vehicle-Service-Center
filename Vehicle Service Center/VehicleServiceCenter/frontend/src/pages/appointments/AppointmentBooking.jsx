import { useEffect, useState } from "react";
import { createAppointment } from "../../api/appointmentApi";
import { getCustomerProfileByUserId } from "../../api/customerProfileApi";
import { getAllVehicles } from "../../api/vehicleApi";
import { getActiveBranches } from "../../api/branchApi";
import { getActiveServiceTypes } from "../../api/serviceTypeApi";
import { useAuth } from "../../context/AuthContext";

function AppointmentBooking() {
    const { user } = useAuth();

    const [formData, setFormData] = useState({
        vehicleId: "",
        serviceTypeId: "",
        branchId: "",
        appointmentDate: "",
        notes: "",
    });

    const [vehicles, setVehicles] = useState([]);
    const [services, setServices] = useState([]);
    const [branches, setBranches] = useState([]);

    const [customerProfileId, setCustomerProfileId] = useState(null);

    const [errors, setErrors] = useState({});
    const [apiError, setApiError] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [loading, setLoading] = useState(true);
    const [submitting, setSubmitting] = useState(false);

    // Load customer data and booking options
    useEffect(() => {
        async function loadBookingData() {
            try {
                setLoading(true);
                setApiError("");

                const [
                    customerResponse,
                    vehiclesResponse,
                    servicesResponse,
                    branchesResponse,
                ] = await Promise.all([
                    getCustomerProfileByUserId(user.userId),
                    getAllVehicles(),
                    getActiveServiceTypes(),
                    getActiveBranches(),
                ]);

                setCustomerProfileId(customerResponse.data.customerProfileId);

                setVehicles(vehiclesResponse.data);
                setServices(servicesResponse.data);
                setBranches(branchesResponse.data);
            } catch (error) {
                console.error(error);

                setApiError(
                    error.response?.data ||
                    "Failed to load appointment booking data."
                );
            } finally {
                setLoading(false);
            }
        }

        if (user?.userId) {
            loadBookingData();
        }
    }, [user]);

    function handleChange(event) {
        const { name, value } = event.target;

        setFormData({
            ...formData,
            [name]: value,
        });

        setErrors({
            ...errors,
            [name]: "",
        });

        setApiError("");
        setSuccessMessage("");
    }

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

    async function handleSubmit(event) {
        event.preventDefault();

        setSuccessMessage("");
        setApiError("");

        if (!validateForm()) {
            return;
        }

        if (!customerProfileId) {
            setApiError("Customer profile could not be found.");
            return;
        }

        try {
            setSubmitting(true);

            const appointmentData = {
                customerProfileId: customerProfileId,
                vehicleId: Number(formData.vehicleId),
                serviceTypeId: Number(formData.serviceTypeId),
                branchId: Number(formData.branchId),
                appointmentDate: formData.appointmentDate,
                status: "Pending",
                notes: formData.notes || null,
            };

            await createAppointment(appointmentData);

            setSuccessMessage(
                "Appointment booked successfully. A confirmation email has been sent when an email address is available."
            );

            setFormData({
                vehicleId: "",
                serviceTypeId: "",
                branchId: "",
                appointmentDate: "",
                notes: "",
            });

            setErrors({});
        } catch (error) {
            console.error(error);

            setApiError(
                error.response?.data ||
                "Failed to book the appointment."
            );
        } finally {
            setSubmitting(false);
        }
    }

    if (loading) {
        return (
            <div className="container mt-5">
                <p>Loading booking information...</p>
            </div>
        );
    }

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Book an Appointment</h2>

            {apiError && (
                <div className="alert alert-danger">
                    {apiError}
                </div>
            )}

            {successMessage && (
                <div className="alert alert-success">
                    {successMessage}
                </div>
            )}

            <form onSubmit={handleSubmit}>

                {/* Vehicle */}
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

                        {vehicles.map((vehicle) => (
                            <option
                                key={vehicle.vehicleId}
                                value={vehicle.vehicleId}
                            >
                                {vehicle.make} {vehicle.model} -{" "}
                                {vehicle.plateNumber}
                            </option>
                        ))}
                    </select>

                    {errors.vehicleId && (
                        <div className="text-danger mt-1">
                            {errors.vehicleId}
                        </div>
                    )}
                </div>

                {/* Service */}
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

                        {services.map((service) => (
                            <option
                                key={service.serviceTypeId}
                                value={service.serviceTypeId}
                            >
                                {service.name}
                            </option>
                        ))}
                    </select>

                    {errors.serviceTypeId && (
                        <div className="text-danger mt-1">
                            {errors.serviceTypeId}
                        </div>
                    )}
                </div>

                {/* Branch */}
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

                        {branches.map((branch) => (
                            <option
                                key={branch.branchId}
                                value={branch.branchId}
                            >
                                {branch.branchName}
                            </option>
                        ))}
                    </select>

                    {errors.branchId && (
                        <div className="text-danger mt-1">
                            {errors.branchId}
                        </div>
                    )}
                </div>

                {/* Appointment date */}
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

                {/* Notes */}
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

                <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={submitting}
                >
                    {submitting
                        ? "Booking..."
                        : "Book Appointment"}
                </button>

            </form>
        </div>
    );
}

export default AppointmentBooking;
