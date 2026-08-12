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
