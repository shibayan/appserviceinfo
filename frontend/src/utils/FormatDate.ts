export function formatRelativeTime(value: string) {
    const date = new Date(value);
    const diff = (Date.now() - date.getTime()) / 1000;
  
    let relativeTime = "";
  
    if (diff < 60) {
      relativeTime = `${diff} second${diff === 1 ? "" : "s"} ago`
    } else if (diff < 60 * 60) {
      const relativeMinutes = Math.round(diff / 60);
      relativeTime = `${relativeMinutes} minute${relativeMinutes === 1 ? "" : "s"} ago`;
    } else if (diff < 60 * 60 * 24) {
      const relativeHours = Math.round(diff / (60 * 60));
      relativeTime = `${relativeHours} hour${relativeHours === 1 ? "" : "s"} ago`;
    } else {
      const relativeDays = Math.round(diff / (60 * 60 * 24));
      relativeTime = `${relativeDays} day${relativeDays === 1 ? "" : "s"} ago`;
    }
  
    return `${date.toLocaleString()} (${relativeTime})`;
  }