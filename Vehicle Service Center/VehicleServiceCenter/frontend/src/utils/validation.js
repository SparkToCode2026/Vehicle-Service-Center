export function validatePasswordChange(newPassword, confirmation) {
  if (newPassword.length < 8) {
    return "The new password must contain at least 8 characters.";
  }

  if (newPassword !== confirmation) {
    return "The password confirmation does not match.";
  }

  return "";
}
