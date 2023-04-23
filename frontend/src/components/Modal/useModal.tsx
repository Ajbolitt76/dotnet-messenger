import React, { useCallback, useContext } from 'react';
import { ModalContext } from './ModalContext';
import { throws } from 'assert';

export class ModalCloseReason<T = unknown> {
  public constructor(
    public readonly Data: T,
    public readonly Source?: "button" | "keyboard",
  ) { };

  public static Button<T = undefined>(data: T) {
    return new ModalCloseReason(data, "button")
  };
}

export interface ModalProps<R = void, E = unknown> {
  resolve: (value: R) => void;
  reject: (reason: ModalCloseReason<E>) => void;
  modalId: number;
}

let idCounter = 0;

type ModalReturnType<R, E = unknown, Throws = false> = Throws extends true
  ? R
  : R | ModalCloseReason<E>

export const useModal = <P, R = void, E = unknown, Throws extends boolean = false>(
  Modal: React.ComponentType<ModalProps<R, E> & P>,
  throws: Throws
): ((props: P) => Promise<ModalReturnType<R, E, Throws>>) => {
  const modalContext = useContext(ModalContext);

  if (!modalContext) {
    throw new Error('Missing <ModalProvider />');
  }

  return useCallback(
    (props: P) => {
      const id = idCounter++;

      let onResolve: (value: R) => void;
      let onReject: (reason: ModalCloseReason<E>) => void;

      const promise = new Promise<ModalReturnType<R, E, Throws>>((res, rej) => {
        onResolve = result => {
          modalContext.closeModal(id);
          res(result);
        };
        onReject = reason => {
          modalContext.closeModal(id);
          if (throws) {
            rej(reason);
          } else {
            res(reason as any);
          }
        };
      });

      modalContext.setModal(
        id,
        <Modal resolve={onResolve!} reject={onReject!} {...props} key={id} modalId={id} />
      );

      return promise;
    },
    [Modal]
  );
};
