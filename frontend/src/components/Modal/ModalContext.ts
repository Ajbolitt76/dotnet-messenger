import React, { createContext } from 'react';

interface ModalContextValue {
  setModal: (id: number, modal: React.ReactElement) => void;
  closeModal: (id: number) => void;
}

export const ModalContext = createContext<ModalContextValue | null>(null);

