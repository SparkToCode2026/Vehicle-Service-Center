const STATUS_COLORS = {
    pending: "warning",
    approved: "info",
    confirmed: "primary",
    inprogress: "primary",
    "in progress": "primary",
    completed: "success",
    cancelled: "danger",
    canceled: "danger",
    rejected: "danger",
    failed: "danger",
    paid: "success",
    partiallypaid: "warning",
    "partially paid": "warning",
    issued: "info",
    overdue: "danger",
    refunded: "secondary",
    unpaid: "danger",
    active: "success",
    inactive: "secondary",
    available: "success",
    unavailable: "secondary",
};

function StatusBadge({ status }) {
    const color = STATUS_COLORS[(status || "").toLowerCase()] || "secondary";
    return <span className={`badge text-bg-${color}`}>{status}</span>;
}

export default StatusBadge;
