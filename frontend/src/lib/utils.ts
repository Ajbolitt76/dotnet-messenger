import { NotificationManager } from './../stores/RootStore';
import { IComputedValue, isComputed } from "mobx";
import { TargetType } from "@/types";
import { NotificationTypes } from '@/stores/NotificationStore';
import { isFunction } from 'lodash';
import { HTTPError } from 'ky';

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

export type Result<T, TE = Error> = {
  success: true,
  data: T
} | {
  success: false,
  error: TE,
}

export async function wrapResult<T>(t: Promise<T>): Promise<Result<T>>{
  try{
    return {
      success: true,
      data: await t
    }
  }catch(e){
    return {
      success: false,
      error: e as Error
    }
  }
}

export async function waitWithNotification<T>(t: Promise<T>, failMessage?: ((e: Error) => string) | string): Promise<Result<T>>{
  var result = await wrapResult(t);
  failMessage ??= "Произошла ошибка";
  if(!result.success){
    NotificationManager.addNotification({
      type: NotificationTypes.ERROR,
      message: isFunction(failMessage) ? failMessage(result.error) : failMessage,
      title: "Ошибка",
    });
  }
  return result
}

export function isPrimitive(
  value: any
) {
  if (typeof value === "object") {
    return value === null;
  }
  return typeof value !== "function";
}