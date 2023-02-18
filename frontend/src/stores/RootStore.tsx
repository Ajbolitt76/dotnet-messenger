import { Instance, types } from "mobx-state-tree";
import { NotificationsListModel } from "@/stores/NotificationStore";
import { createContext, FC, useContext } from "react";

const RootModel = types.model({
  notificationStore: NotificationsListModel
});

const RootStore = RootModel.create({
  notificationStore: { notifications: [] }
});

export type RootInstance = Instance<typeof RootModel>;
const RootStoreContext = createContext<null | RootInstance>(null);

export const StoreProvider: FC<{
  children: React.ReactNode;
}> = ({children}) => (<RootStoreContext.Provider value={RootStore}> {children} </RootStoreContext.Provider>);
export const useStore = () => {
  const store = useContext(RootStoreContext);
  if (store === null) {
    throw new Error("Store cannot be null, please add a context provider");
  }
  return store;
}
