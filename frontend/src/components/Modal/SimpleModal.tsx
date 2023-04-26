import React from 'react';
import ReactModal, { Classes } from 'react-modal';
import styles from './SimpleModal.module.pcss';

/** Классы для контента */
const contentClass: Classes = {
  base: 'modal',
  afterOpen: 'modal-open',
  beforeClose: 'modal-closing'
};

/** Классы для оверлея */
const overlayClass: Classes = {
  base: 'overlay',
  afterOpen: 'overlay-open',
  beforeClose: 'overlay-closing'
};

ReactModal.setAppElement('#root');

/** Простое модальное окно */
export const SimpleModal: React.FC<ReactModal.Props> = (data) => {
  const { children, ...props } = data;

  return (
    <ReactModal
    className={contentClass}
    overlayClassName={styles.overlay}
    portalClassName={styles.root}
    {...props}
    >
      {children}
    </ReactModal>
  );
};
