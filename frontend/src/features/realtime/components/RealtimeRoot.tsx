import { PropsWithChildren, useEffect } from "react"
import { realtimeConnection, useRealtimeConnectionStatus } from "../utils/realtimeConnection"
import { useAuth } from "@/lib/AuthProvider";
import { tokenStore } from "@/lib/ApiClient";
import { RootInstance, useStore } from "@/stores/RootStore";
import { NewMessageRealtimeUpdate, RealtimeUpdateTypes, UpdateMessage } from "@/features/chats/types";

let connecting = false;
export const RealtimeRoot: React.FC<PropsWithChildren> = ({ children }) => {
  const [state, stateChanged, failed] = useRealtimeConnectionStatus();
  const { user } = useAuth();
  const store = useStore();

  useEffect(() => {
    if(!user)
      return;

    if (!realtimeConnection.isInitialized && !connecting) {
      connecting = true;
      realtimeConnection.setAccessToken(tokenStore.Token!);
      realtimeConnection.start().then(x => {
        stateChanged();
        connecting = false;
      }).catch(x => {
        console.error("Error while connecting to server", x)
        failed(x)
      });

      stateChanged();
    }
  }, [user])

  useEffect(() => {
    const signal = realtimeConnection.connection;

    function handleOneMessage(message: UpdateMessage){
      switch(message.$type){
        case RealtimeUpdateTypes.NewMessage:
          handleNewMessage(store, message);
          break;
      }
    }

    function handleBatch(message: UpdateMessage[]){
      for(let i = 0; i < message.length; i++){
        handleOneMessage(message[i])
      }
    }

    signal.on("HandleUpdates", handleBatch)
    return () => {
      signal.off("HandleUpdates", handleBatch)
    }
  }, [store])

  return (<>{children}</>)
}

function handleNewMessage(store: RootInstance, update: NewMessageRealtimeUpdate){
  const messages = store.conversationsStore.messageStore.get(update.conversationId);
  if(!messages)
    return;
  messages.addNewMessages({messages: [update.data]});
}