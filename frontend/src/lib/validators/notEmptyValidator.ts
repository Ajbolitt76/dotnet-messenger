export function notEmpty(value: string, message?: string): string | null {
  return !!value && value.length > 0 ? null : message ?? 'Поле не должно быть пустым';
}
