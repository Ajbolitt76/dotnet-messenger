import React, { useState } from 'react';
import { ModalContext } from './ModalContext';

export const ModalProvider: React.FC<React.PropsWithChildren> = ({
  children,
}) => {
  const [visibleModals, setVisibleModals] = useState<
    Record<number, React.ReactElement>
  >({});

  const setModal = (id: number, modal: React.ReactElement) => {
    setVisibleModals(current => {
      return {
        ...current,
        [id]: modal,
      };
    });
  };

  const closeModal = (id: number) => {
    setVisibleModals(current => {
      const { [id]: _r, ...rest } = current;
      return rest;
    });
  };

  const modalArray = Object.values(visibleModals).map(m =>
    React.cloneElement(m)
  );

  return (
    <ModalContext.Provider value={{ setModal, closeModal }}>
      {children}
      {modalArray}
    </ModalContext.Provider>
  );
};
