import api from "./api";

export function getMechanicProfileByUserId(userId) {
    return api.get("/MechanicProfile/GetByUserId", {
        params: {
            userId,
        },
    });
}

export function updateMechanicAvailability(
    mechanicProfileId,
    isAvailable
) {
    return api.patch("/MechanicProfile/UpdateAvailability", null, {
        params: {
            id: mechanicProfileId,
            isAvailable,
        },
    });
}