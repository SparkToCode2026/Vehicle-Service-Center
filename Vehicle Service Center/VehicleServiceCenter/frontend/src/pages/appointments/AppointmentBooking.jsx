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
