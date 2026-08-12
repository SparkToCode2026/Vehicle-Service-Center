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
            <div className="modal fade show d-block" tabIndex="-1">
                <div className="modal-dialog modal-dialog-centered">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{title}</h5>
                            <button type="button" className="btn-close" onClick={onCancel}></button>
                        </div>
                        <div className="modal-body">
                            <p className="mb-0">{message}</p>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-outline-secondary" onClick={onCancel}>
                                {cancelLabel}
                            </button>
                            <button type="button" className="btn btn-danger" onClick={onConfirm}>
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