function EmptyDataMessage({ title = "No data found", message = "Nothing to show here yet." }) {
    return (
        <div className="text-center py-5 text-muted">
            <h5 className="fw-semibold">{title}</h5>
            <p className="mb-0">{message}</p>
        </div>
    );
}

export default EmptyDataMessage;