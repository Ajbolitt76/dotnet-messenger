import { apiClient } from "@/lib/ApiClient";
import { ConversationTypes } from "../types";
import { AsyncReturnType } from "type-fest";
import { TgQueryConfig } from '@/types';
import { ChatCacheKeys } from './chatCacheKeys';
import { useQuery } from "@tanstack/react-query";
import { ConversationInfoInstance } from "../stores/MessagesStore";
import { useStore } from "@/stores/RootStore";

export interface GetChatListResponse {
  items: [{
    id: string;
    title: string;
    lastMessage: {
      authorName: string;
      text: string;
      sentAt: string;
    };
    type: ConversationTypes;
  }];
}

export const getConversations = (): Promise<GetChatListResponse> => {
  return apiClient.get("c/conv-list").json();
}

type ReturnType = ConversationInfoInstance[];

type UseConversationsOptions = {
  config?: TgQueryConfig<ReturnType>;
};

export const useConversationList = ({ config }: UseConversationsOptions = {}) => {
  const { conversationsStore: conversations } = useStore();

  return useQuery<ReturnType, Error>({
    ...config,
    queryKey: ChatCacheKeys.conversationList,
    queryFn: () => conversations.loadConversationList(),
  });
}

