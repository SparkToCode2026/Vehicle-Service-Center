function EmptyDataMessage({ title = "No data found", message = "Nothing to show here yet." }) {
    return (
        <div className="empty-state">
            <div className="empty-state-icon" aria-hidden="true"><i className="bi bi-inbox" /></div>
            <h5 className="fw-semibold">{title}</h5>
            <p className="mb-0">{message}</p>
        </div>
    );
}

export default EmptyDataMessage;
