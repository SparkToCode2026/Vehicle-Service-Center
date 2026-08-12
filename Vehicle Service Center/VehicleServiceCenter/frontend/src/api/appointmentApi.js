import api from "./api";

// Get all appointments
export const getAppointments = () => {
    return api
        .get("/Appointment")
        .then((response) => response.data);
};

// Get a single appointment by its ID
export const getAppointmentById = (id) => {
    return api
        .get(`/Appointment/${id}`)
        .then((response) => response.data);
};

// Create a new appointment
export const createAppointment = (appointmentData) => {
    return api
        .post("/Appointment", appointmentData)
        .then((response) => response.data);
};

// Update an existing appointment
export const updateAppointment = (id, appointmentData) => {
    return api
        .put(`/Appointment/${id}`, appointmentData)
        .then((response) => response.data);
};

// Change the status of an appointment
export const updateAppointmentStatus = (id, status) => {
    return api
        .patch(`/Appointment/${id}/status`, null, {
            params: {
                status: status,
            },
        })
        .then((response) => response.data);
};

// Delete an appointment
export const deleteAppointment = (id) => {
    return api
        .delete(`/Appointment/${id}`)
        .then((response) => response.data);
};

// Filter appointments by status
export const filterAppointmentsByStatus = (status) => {
    return api
        .get("/Appointment/filter", {
            params: {
                status: status,
            },
        })
        .then((response) => response.data);
};

