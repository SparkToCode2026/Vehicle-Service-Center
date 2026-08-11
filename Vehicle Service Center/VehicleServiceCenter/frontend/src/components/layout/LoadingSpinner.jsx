
function LoadingSpinner({ message = "Loading...", fullPage = false, size = "md" }) {
    const spinnerSizeClass = size === "sm" ? "spinner-border-sm" : "";

    const content = (
        <div className="d-flex flex-column align-items-center justify-content-center gap-3 py-4">
            <div className={`spinner-border text-primary ${spinnerSizeClass}`} role="status">
                <span className="visually-hidden">Loading</span>
            </div>
            {message && <p className="text-muted mb-0">{message}</p>}
        </div>
    );

    if (fullPage) {
        return (
            <div className="d-flex align-items-center justify-content-center" style={{ minHeight: "60vh" }}>
                {content}
            </div>
        );
    }

    return content;
}

export default LoadingSpinner;