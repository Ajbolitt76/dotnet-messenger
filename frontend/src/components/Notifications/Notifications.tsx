import { Notification } from './Notification';
import { useStore } from "@/stores/RootStore";
import { observer } from "mobx-react-lite";
import React from "react";
import { TransitionGroup, CSSTransition } from "react-transition-group";
import "./Notification.pcss";

export const Notifications = observer(() => {
  const { notificationStore } = useStore();
  return (
    <TransitionGroup
      aria-live="assertive"
      className="z-50 flex flex-col fixed inset-0 space-y-4 items-end px-4 py-6 pointer-events-none sm:p-6 sm:items-start"
    >
      {
        notificationStore.notifications.filter(x => x.isShown).map((notification) => (
          <CSSTransition
            classNames="notification"
            timeout={
              {
                enter: 300,
                exit: 100,
              }
            }
            onExited={() => {
              notification.onExited();
            }}
            mountOnEnter
            unmountOnExit
            key={notification.id}
            nodeRef={notification.ref}
          >
            <Notification notification={notification} ref={notification.ref}/>
          </CSSTransition>
        ))
      }
    </TransitionGroup>
  );
});
