export function formatRelativeTime(value: string) {
  if (!value) return ''

  const date = new Date(value)

  if (isNaN(date.getTime())) return ''

  const diff = (Date.now() - date.getTime()) / 1000

  let relativeTime: string

  if (diff < 60) {
    const seconds = Math.round(diff)
    relativeTime = `${seconds} second${seconds === 1 ? '' : 's'} ago`
  }
  else if (diff < 60 * 60) {
    const relativeMinutes = Math.round(diff / 60)
    relativeTime = `${relativeMinutes} minute${relativeMinutes === 1 ? '' : 's'} ago`
  }
  else if (diff < 60 * 60 * 24) {
    const relativeHours = Math.round(diff / (60 * 60))
    relativeTime = `${relativeHours} hour${relativeHours === 1 ? '' : 's'} ago`
  }
  else {
    const relativeDays = Math.round(diff / (60 * 60 * 24))
    relativeTime = `${relativeDays} day${relativeDays === 1 ? '' : 's'} ago`
  }

  return `${date.toLocaleString()} (${relativeTime})`
}

export function getUpdatedAt(response: Response): string {
  const lastModified = response.headers.get('Last-Modified')

  if (!lastModified) return ''

  const parsed = new Date(lastModified)

  return Number.isNaN(parsed.getTime()) ? '' : parsed.toISOString()
}

export function getNewestTimestamp(values: string[]): string {
  return values
    .filter(Boolean)
    .sort((left, right) => new Date(right).getTime() - new Date(left).getTime())[0] ?? ''
}
