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
