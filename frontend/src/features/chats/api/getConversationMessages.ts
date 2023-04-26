import { apiClient } from "@/lib/ApiClient";
import { TgQueryConfig } from "@/types";
import { InfiniteData, QueryKey, useInfiniteQuery } from "@tanstack/react-query";
import { AsyncReturnType } from "type-fest";
import { ChatCacheKeys } from "./chatCacheKeys";
import { useStore } from "@/stores/RootStore";
import { ConversationMessageInstance } from "../stores/MessagesStore";
import { faker } from "@faker-js/faker";

export interface GetMessagesRequestParams extends Record<string, string | number | undefined> {
  Count: number,
  MessagePointer?: string
}

export interface GetMessagesResponse {
  messages: [{
    messageId: string,
    sentBy: string,
    content: string,
    sentAt: string,
    editedAt: string,
    positon: number
  }];
}

export const getConversationMessages = (id: string, { MessagePointer, ...rest }: GetMessagesRequestParams): Promise<GetMessagesResponse> => {
  const p = rest;
  if (MessagePointer)
    p.MessagePointer = MessagePointer;

  return apiClient.get(`c/${id}/messages`, {
    searchParams: p as Record<string, string | number>
  }).json();
}

type ReturnType = InfiniteData<ConversationMessageInstance[]>;

type UseConversationMessagesOptions = {
  config?: TgQueryConfig<ReturnType>;
};

export const useConversationMessages = (conversationId: string, count: number = 40) => {
  const { conversationsStore } = useStore();
  const conversation = conversationsStore.conversations.get(conversationId);
  const messageStore = conversationsStore.messageStore.get(conversationId);

  return useInfiniteQuery<ConversationMessageInstance[], unknown, ReturnType, QueryKey, string | undefined>({
    // ...config
    refetchOnReconnect: false,
    queryKey: ChatCacheKeys.conversationMessages(conversationId),
    queryFn: ({ pageParam, direction }) => conversation!.loadMessagesFromPointer({
      Count: direction === 'forward' ? count : count * -1,
      MessagePointer: pageParam
    }),
    defaultPageParam: undefined as string | undefined,
    getNextPageParam: (x) =>
      x.length == count
        ? x[x.length - 1].messageId
        : undefined,
    enabled: !!conversation && messageStore?.messages.size == 0
  })
};


