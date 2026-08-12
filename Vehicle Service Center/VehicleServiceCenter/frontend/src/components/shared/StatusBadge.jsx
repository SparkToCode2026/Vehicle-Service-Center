const STATUS_COLORS = {
    pending: "warning",
    approved: "info",
    inprogress: "primary",
    "in progress": "primary",
    completed: "success",
    cancelled: "danger",
    paid: "success",
    unpaid: "danger",
    active: "success",
    inactive: "secondary",
};

function StatusBadge({ status }) {
    const color = STATUS_COLORS[(status || "").toLowerCase()] || "secondary";
    return <span className={`badge text-bg-${color}`}>{status}</span>;
}

export default StatusBadge;