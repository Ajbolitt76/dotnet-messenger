export function notNullValidator<T>(value: T, message?: string): string | null {
  return value != null ? null : message ?? 'Поле не должно быть пустым';
}
