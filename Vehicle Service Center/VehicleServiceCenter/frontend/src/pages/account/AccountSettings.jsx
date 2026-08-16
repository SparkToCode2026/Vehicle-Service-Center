import { useState } from "react";
import { changePassword } from "../../api/authApi";
import { useAuth } from "../../context/AuthContext";
import { getApiErrorMessage } from "../../utils/httpErrors";
import { validatePasswordChange } from "../../utils/validation";

function AccountSettings() {
  const { user } = useAuth();
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();
    const validationError = validatePasswordChange(newPassword, confirmPassword);
    if (validationError) {
      setError(validationError);
      return;
    }

    try {
      setSaving(true); setError(""); setMessage("");
      await changePassword(user.userId, currentPassword, newPassword);
      setCurrentPassword(""); setNewPassword(""); setConfirmPassword("");
      setMessage("Password changed successfully.");
    } catch (requestError) {
      setError(getApiErrorMessage(requestError, "Could not change the password."));
    } finally {
      setSaving(false);
    }
  }

  return <section><div className="mb-4"><h2>Account Settings</h2><p className="text-secondary">Update your account password securely.</p></div>
    <div className="card content-card-md"><form className="card-body" onSubmit={handleSubmit}>
      {error && <div className="alert alert-danger">{error}</div>}{message && <div className="alert alert-success">{message}</div>}
      <div className="mb-3"><label className="form-label" htmlFor="current-password">Current password</label><input id="current-password" className="form-control" type="password" autoComplete="current-password" required value={currentPassword} onChange={(event) => setCurrentPassword(event.target.value)} /></div>
      <div className="mb-3"><label className="form-label" htmlFor="new-password">New password</label><input id="new-password" className="form-control" type="password" autoComplete="new-password" required minLength="8" value={newPassword} onChange={(event) => setNewPassword(event.target.value)} /></div>
      <div className="mb-4"><label className="form-label" htmlFor="confirm-password">Confirm new password</label><input id="confirm-password" className="form-control" type="password" autoComplete="new-password" required value={confirmPassword} onChange={(event) => setConfirmPassword(event.target.value)} /></div>
      <button className="btn btn-primary" type="submit" disabled={saving}>{saving ? "Updating..." : "Change password"}</button>
    </form></div>
  </section>;
}

export default AccountSettings;
