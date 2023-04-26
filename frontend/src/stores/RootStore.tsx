import { Instance, getRoot, types } from "mobx-state-tree";
import { NotificationsListModel } from "@/stores/NotificationStore";
import { createContext, FC, useContext } from "react";
import { AuthStore } from "@/features/user/stores";
import { ConversationsStore } from "@/features/chats/stores/MessagesStore";

const RootModel = types.model({
  notificationStore: NotificationsListModel,
  authStore: AuthStore,
  conversationsStore: ConversationsStore
});

export type RootModelType = typeof RootModel;

const RootStore = RootModel.create({
  notificationStore: { notifications: [] },
  authStore: {},
  conversationsStore: {}
});

export const NotificationManager = RootStore.notificationStore;

export type RootInstance = Instance<typeof RootModel>;

const RootStoreContext = createContext<null | RootInstance>(null);

export const StoreProvider: FC<{
  children: React.ReactNode;
}> = ({children}) => (<RootStoreContext.Provider value={RootStore}> {children} </RootStoreContext.Provider>);

console.log(RootStore);

export const useStore = () => {
  const store = useContext(RootStoreContext);
  if (store === null) {
    throw new Error("Store cannot be null, please add a context provider");
  }
  return store;
}
