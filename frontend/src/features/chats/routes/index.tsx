import { ChatPage } from './ChatPage';
import { ModuleRoutes } from "@/types";
import { ConversationView } from './ConversationView';

export const ChatModuleRoutes: ModuleRoutes = {
  common: [],
  anonymous: [],
  authenticated: [
    {
      path: "/chats",
      element: <ChatPage title='Чаты'/>,
      children: [
        {
          path: ":chatId",
          element: <ConversationView />
        }
      ]
    },
  ]
}
