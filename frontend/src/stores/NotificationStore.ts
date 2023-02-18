import { cast, destroy, detach, getParent, hasParent, Instance, SnapshotIn, types } from "mobx-state-tree";
import { nanoid } from "nanoid";
import { createRef } from "react";
import { addAnimationSupport } from "@/stores/Extensions/TransitionGroup";

export enum NotificationTypes {
  INFO = "info",
  SUCCESS = "success",
  WARNING = "warning",
  ERROR = "error",
}

const NotificationModel = addAnimationSupport(types.model('Notification', {
  id: types.optional(types.identifier, () => nanoid()),
  message: types.string,
  title: types.string,
  type: types.enumeration<NotificationTypes>('Type', Object.values(NotificationTypes)),
}), (self) => {
  getParent<typeof NotificationsListModel>(self, 2).removeNotification(self);
}).actions(self => ({
  close() {
    self.setShown(false);
  }
}));


export const NotificationsListModel = types.model('Notifications', {
  notifications: types.optional(types.array(NotificationModel), [])
}).actions(self => ({
  addNotification(notification: SnapshotIn<typeof NotificationModel>, timeout = 3000) {
    notification.id ??= nanoid();
    self.notifications.push(notification);
    setTimeout(() => {
      const notif = self.notifications.find(n => n.id === notification.id);
      if (notif) {
        notif.close();
      }
    }, timeout);
  },
  removeNotification(notification: SnapshotIn<typeof NotificationModel>) {
    self.notifications = cast(self.notifications.filter(x => x != notification));
  }
})).actions(self => ({
  error(message: string, title: string, timeout = 3000) {
    self.addNotification({ message, title, type: NotificationTypes.ERROR }, timeout);
  },
  success(message: string, title: string, timeout = 3000) {
    self.addNotification({ message, title, type: NotificationTypes.SUCCESS }, timeout);
  },
}));

export type NotificationModel = Instance<typeof NotificationModel>;
