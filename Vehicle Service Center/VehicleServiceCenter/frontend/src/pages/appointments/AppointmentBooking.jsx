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