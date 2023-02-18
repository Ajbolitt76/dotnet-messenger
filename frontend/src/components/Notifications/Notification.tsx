import React, { useRef } from "react";
import { Icon24InfoCircleOutline } from '@vkontakte/icons';
import { Icon24CheckCircleOutline } from '@vkontakte/icons';
import { Icon24ErrorCircleOutline } from '@vkontakte/icons';
import { Icon24Cancel } from '@vkontakte/icons';
import { CSSTransition } from 'react-transition-group';
import { NotificationModel, NotificationTypes } from "@/stores/NotificationStore";
import { observer } from "mobx-react-lite";
import { isAlive } from "mobx-state-tree";

const icons: Record<NotificationTypes, React.ReactNode> = {
  info: <Icon24InfoCircleOutline className="text-blue-bright" aria-hidden="true"/>,
  success: <Icon24CheckCircleOutline className="text-green-dark" aria-hidden="true"/>,
  warning: <Icon24ErrorCircleOutline className="text-yellow-500" aria-hidden="true"/>,
  error: <Icon24ErrorCircleOutline className="text-destructive" aria-hidden="true"/>,
};


export type NotificationContentProps = {
  notification: NotificationModel;
};

export const Notification = React.forwardRef<HTMLDivElement, NotificationContentProps>(
  ({ notification }, ref) => {
    return (
      <div ref={ref} className="w-full flex flex-col items-center space-y-4 sm:items-end">
        <div className="notification-container max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto ring-1 ring-black ring-opacity-5 overflow-hidden">
          <div className="p-4" role="alert" aria-label={notification.title}>
            <div className="flex items-start">
              <div className="flex-shrink-0 self-center">{icons[notification.type]}</div>
              <div className="ml-3 w-0 flex-1 pt-0.5">
                <p className="text-sm font-medium text-gray-900">{notification.title}</p>
                <p className="mt-1 text-sm text-gray-500">{notification.message}</p>
              </div>
              <div className="ml-4 flex-shrink-0 flex">
                <button
                  className="bg-white rounded-md inline-flex text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
                  onClick={() => {
                    notification.close();
                  }}
                >
                  <span className="sr-only">Close</span>
                  <Icon24Cancel className="h-5 w-5" aria-hidden="true"/>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  });
