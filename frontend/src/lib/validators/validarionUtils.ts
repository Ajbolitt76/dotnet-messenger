import type { ValueValidationResult } from "@/stores/Extensions/Validation";
import { ReactNode } from "react";
import { IComputedValue, isComputed } from "mobx";
import { isComputedValue } from "@/lib/utils";


/** Хелпер, выдающий true если хотя бы одна валидация провалилась */
export function isAnyError(x?: ValueValidationResult<any>[]): boolean {
  return (
    x?.some((y) => {
      if (y == null) {
        return false;
      }
      if (typeof y === 'string') {
        return true;
      }
      if (Array.isArray(y)) {
        return isAnyError(y);
      }
      if (typeof y === 'object') {
        // @ts-ignore
        return Object.values(y).some(isAnyError);
      }
      return true;
    }) ?? false
  );
}

/** Получить строку со всеми ошибками */
export function getJoinedDisplayErrors(
  x?: ValueValidationResult<any>[]
): string | undefined {
  return x?.filter((y) => y != null && typeof y === 'string').join('\n');
}

/**
 * Получить пропсы для FormItem по результату валидации
 * @param x Результаты валидации
 * @param bottomMessage Дополнительное сообщение для bottom, если валидация успешная
 * @returns Пропсы для FormItem по результату валидации
 */
export function validationFormItemProps<T>(
  x?: ValueValidationResult<T>[] | IComputedValue<ValueValidationResult<T>[]>,
  bottomMessage?: ReactNode
): {
  status: 'error' | 'default';
  bottom: ReactNode | undefined;
} {
  const error = isComputedValue(x) ? x.get() : x;

  const hasErrors = isAnyError(error);
  return {
    status: hasErrors ? 'error' : 'default',
    bottom: hasErrors ? getJoinedDisplayErrors(error) : bottomMessage
  };
}
