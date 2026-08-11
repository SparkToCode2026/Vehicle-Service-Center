
function EmptyDataMessage({
                              title = "No data found",
                              message = "There's nothing to show here yet.",
                              actionLabel,
                              onAction,
                          }) {
    return (
        <div className="text-center py-5">
            <svg
                width="64"
                height="64"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="1.5"
                className="text-secondary mb-3"
            >
                <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M3 7.5V6a2 2 0 0 1 2-2h4l2 2h8a2 2 0 0 1 2 2v9.5a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-8Z"
                />
            </svg>
            <h5 className="fw-semibold">{title}</h5>
            <p className="text-muted mb-3">{message}</p>
            {actionLabel && onAction && (
                <button type="button" className="btn btn-primary btn-sm" onClick={onAction}>
                    {actionLabel}
                </button>
            )}
        </div>
    );
}

export default EmptyDataMessage;