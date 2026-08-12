function LoadingSpinner({ message = "Loading...", fullPage = false }) {
    return (
        <div
            className="d-flex flex-column align-items-center justify-content-center gap-2"
            style={fullPage ? { minHeight: "60vh" } : { padding: "2rem" }}
        >
            <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading</span>
            </div>
            <p className="text-muted mb-0">{message}</p>
        </div>
    );
}

export default LoadingSpinner;