import { useMutation } from "@tanstack/react-query";
import { apiClient } from "@/lib/ApiClient";
import { MutationConfig, queryClient } from "@/lib/ReactQuery";
import { ChatCacheKeys } from "../chatCacheKeys";
import { useQueryResultHandler } from '@/lib/hooks/useQueryResultHandler';

export interface CreatePersonalConversationResponse {
  created: boolean;
  id: string;
}

export interface CreatePersonalConversationRequest {
  receiverId: string
}

export const createPersonalChat = (data: CreatePersonalConversationRequest): Promise<CreatePersonalConversationResponse> => {
  return apiClient.post("pms/create", {
    searchParams: {
      "receiverId": data.receiverId,
    }
  }).json<CreatePersonalConversationResponse>();
};

type UseCreatePersonalChatOptions = {
  config?: MutationConfig<typeof createPersonalChat>;
};
export const useCreatePersonalChat = ({ config }: UseCreatePersonalChatOptions = {}) => {
  const { handleError } = useQueryResultHandler("Ошибка при создании переписки");
  return useMutation<CreatePersonalConversationResponse, unknown, CreatePersonalConversationRequest>({
    onError: handleError,
    onSuccess: () => {
      queryClient.refetchQueries({ queryKey: ChatCacheKeys.conversationList });
    },
    ...config,
    mutationFn: createPersonalChat,
  });
};

