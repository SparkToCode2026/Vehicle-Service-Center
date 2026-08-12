export function getApiErrorMessage(error, fallback = "The request failed.") {
  const data = error?.response?.data;

  if (typeof data === "string" && data.trim()) {
    return data;
  }

  if (data?.message) {
    return data.message;
  }

  if (data?.title) {
    const validationMessages = data.errors
      ? Object.values(data.errors).flat().join(" ")
      : "";

    return validationMessages || data.title;
  }

  return fallback;
}

export function classifyHttpError(status) {
  if (status === 401) return "unauthorized";
  if (status === 403) return "forbidden";
  return "other";
}
