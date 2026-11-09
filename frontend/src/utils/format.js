// The API stores times in UTC but sends them without a "Z", so we add it before showing local time.
export function formatDateTime(value) {
    if (!value) return '';
    const hasZone = /(Z|[+-]\d{2}:\d{2})$/.test(value);
    return new Date(hasZone ? value : `${value}Z`).toLocaleString();
  }
  
  export function truncate(text, max = 80) {
    if (!text) return '';
    return text.length > max ? `${text.slice(0, max)}…` : text;
  }
  
  export function levelClass(level) {
    return String(level).toLowerCase();
  }