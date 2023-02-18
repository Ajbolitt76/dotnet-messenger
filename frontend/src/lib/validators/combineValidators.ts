export function combineValidators<T>(
  ...validators: ((value: T, message?: string) => string | null)[]
): (value: T) => string | null {
  return (value: T) => {
    for (const validator of validators) {
      const error = validator(value);
      if (error != null) {
        return error;
      }
    }
    return null;
  };
}
