export function normalizeServiceOrderStatus(status) {
  return status === "In Progress" ? "InProgress" : status || "";
}

export function formatServiceOrderStatus(status) {
  return normalizeServiceOrderStatus(status).replace(
    /([a-z])([A-Z])/g,
    "$1 $2"
  );
}

export function normalizeServiceOrderItemType(itemType) {
  return itemType === "Part" ? "SparePart" : itemType;
}

export function formatServiceOrderItemType(itemType) {
  return normalizeServiceOrderItemType(itemType) === "SparePart"
    ? "Spare part"
    : itemType;
}
