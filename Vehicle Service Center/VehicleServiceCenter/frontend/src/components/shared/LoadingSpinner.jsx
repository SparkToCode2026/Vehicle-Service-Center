function LoadingSpinner({ message = "Loading...", fullPage = false }) {
    return (
        <div
            className={`loading-state d-flex flex-column align-items-center justify-content-center gap-2 ${fullPage ? "loading-state-full" : ""}`}
        >
            <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading</span>
            </div>
            <p className="text-muted mb-0">{message}</p>
        </div>
    );
}

export default LoadingSpinner;
