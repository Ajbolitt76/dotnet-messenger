import { IComputedValue, isComputed } from "mobx";
import { TargetType } from "@/types";

export function getTargetElement(target: TargetType) : HTMLElement | Window | Document | null
{
  if (!target) return null;

  if (target === 'document') {
    return document;
  } else if (target === 'window') {
    return window;
  } else if (typeof target === 'object' && target.hasOwnProperty('current')) {
    return target.current;
  }

  return null;
}

export function isComputedValue<T>(x: T | IComputedValue<T>): x is IComputedValue<T> {
  return isComputed(x);
}
