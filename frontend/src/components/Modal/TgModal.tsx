import clsx from 'clsx';
import styles from './TgModal.module.pcss';
import React, { useContext, useState } from 'react';
import { SimpleModal } from './SimpleModal';
import { ModalContext } from './ModalContext';
import { Icon24Cancel } from '@vkontakte/icons';
import { IconButton } from '@vkontakte/vkui';
import { ModalCloseReason } from './useModal';

export interface TgModalProps extends React.PropsWithChildren {
  width?: number;
  title: string;
  className?: string;
  onClose?: (x: ModalCloseReason) => boolean | void;
  footer?: React.ReactNode;
}

export const TgModal: React.FC<TgModalProps> = ({
  width = 420,
  title,
  className,
  onClose,
  footer,
  children,
}) => {
  const modalContext = useContext(ModalContext);
  const [isOpen, setIsOpen] = useState(true);

  return (
    <SimpleModal isOpen={isOpen}>
      <div className={styles.modal}>
        <div className={styles.content} style={{ maxWidth: width }}>
          <div className={styles.titleWrapper}>
            <h3 className={styles.title}>{title}</h3>
            <IconButton onClick={() => onClose?.(ModalCloseReason.Button(undefined))} className={styles.close}>
              <Icon24Cancel />
            </IconButton>
          </div>
          <div className={styles.modalContent}>{children}</div>
          {footer && <div className={styles.footer}>{footer}</div>}
        </div>
      </div>
    </SimpleModal>
  );
};
