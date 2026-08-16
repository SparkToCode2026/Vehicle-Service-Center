function ConfirmationModal({
                               show,
                               title = "Are you sure?",
                               message = "This action cannot be undone.",
                               confirmLabel = "Confirm",
                               cancelLabel = "Cancel",
                               onConfirm,
                               onCancel,
                           }) {
    if (!show) return null;

    return (
        <>
            <div className="modal-backdrop fade show"></div>
            <div className="modal fade show d-block" tabIndex="-1" role="dialog" aria-modal="true" aria-labelledby="confirmation-title">
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="confirmation-title">{title}</h5>
                            <button type="button" className="btn-close" onClick={onCancel} aria-label="Close confirmation"></button>
                        </div>
                        <div className="modal-body">
                            <p className="mb-0">{message}</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-outline-secondary" onClick={onCancel}>
                                {cancelLabel}
                            </button>
                            <button type="button" className="btn btn-danger btn-icon-label" onClick={onConfirm}>
                                <i className="bi bi-trash" aria-hidden="true" />
                                {confirmLabel}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default ConfirmationModal;
