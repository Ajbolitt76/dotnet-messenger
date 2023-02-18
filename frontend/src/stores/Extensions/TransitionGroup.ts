import { destroy, IAnyModelType, IModelType, Instance, ModelProperties } from "mobx-state-tree";
import { createRef } from "react";
import { observable } from "mobx";

type ExtendedWithAnimation<
  T extends IModelType<PROPS, OTHERS, CustomC, CustomS>,
  PROPS extends ModelProperties = T extends IModelType<infer P, any, any, any> ? P : never,
  OTHERS = T extends IModelType<any, infer O, any, any> ? O : never,
  CustomC = T extends IModelType<any, any, infer C, any> ? C : never,
  CustomS = T extends IModelType<any, any, any, infer S> ? S : never,
> = IModelType<PROPS, OTHERS & {
  get ref(): React.RefObject<any>;
  get isShown(): boolean;
  setShown(value: boolean): void;
  onExited(): void;
}, CustomC, CustomS>;

export function addAnimationSupport<
  T extends IModelType<PROPS, OTHERS, CustomC, CustomS>,
  PROPS extends ModelProperties = T extends IModelType<infer P, any, any, any> ? P : never,
  OTHERS = T extends IModelType<any, infer O, any, any> ? O : never,
  CustomC = T extends IModelType<any, any, infer C, any> ? C : never,
  CustomS = T extends IModelType<any, any, any, infer S> ? S : never,
>(store: T, onExited: (self: Instance<T>) => void): ExtendedWithAnimation<T, PROPS, OTHERS, CustomC, CustomS> {
  return store.extend(self => {
    const refInstance = createRef<any>();
    let shown = observable.box(true);

    return {
      views: {
        get isShown() {
          return shown.get();
        },
        get ref() {
          return refInstance;
        }
      },
      actions: {
        setShown(value: boolean) {
          shown.set(value);
        },
        onExited() {
          onExited(self);
          // destroy(self);
        }
      }
    }
  })
}
