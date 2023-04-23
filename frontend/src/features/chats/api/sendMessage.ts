import { useMutation } from "@tanstack/react-query";
import { MutationConfig, queryClient } from "@/lib/ReactQuery";
import { useQueryResultHandler } from "@/lib/hooks/useQueryResultHandler";
import { ChatCacheKeys } from './chatCacheKeys';
import { apiClient } from '@/lib/ApiClient';
import { useStore } from "@/stores/RootStore";
import { useAuth } from "@/lib/AuthProvider";

export interface SendMessageRequest {
  conversationId: string;
  message: string;
}

export interface SendMessageResponse {
  sent: boolean;
  messageId: string;
}

export const sendMessageToChat = ({conversationId, ...data}: SendMessageRequest): Promise<SendMessageResponse> => {
  return apiClient.post(`c/${conversationId}/send`, {
    json: data
  }).json<SendMessageResponse>();
};
