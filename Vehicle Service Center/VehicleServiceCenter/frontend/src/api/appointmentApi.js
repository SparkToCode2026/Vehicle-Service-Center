import api from "./api";

export const getAppointments = () => {
    return api
        .get("/Appointment")
        .then((response) => response.data);
};