import { apiClient } from "@/lib/ApiClient";
import { MutationConfig, queryClient } from "@/lib/ReactQuery";
import { useStore } from "@/stores/RootStore";
import { FileOwnershipSignedData } from "@/features/files/types";
import { useMutation } from "@tanstack/react-query";
import { HTTPError } from "ky";

export type UpdateUserInfoDto = {
  name: string;
  dateOfBirth: string;
  profilePicture: FileOwnershipSignedData;
}

export const updateUserProfile = (data: UpdateUserInfoDto): Promise<boolean> => {
  return apiClient.put("me", { json: data }).json<boolean>();
};

type UseUpdateDiscussionOptions = {
  config?: MutationConfig<typeof updateUserProfile>;
};

const userKey = ["user"];

export const useUpdateProfile = ({ config }: UseUpdateDiscussionOptions = {}) => {
  const { notificationStore } = useStore();

  return useMutation({
    onError: (err, __, context: any) => {
      notificationStore.error((err as HTTPError).message, "Ошибка при сохранении профиля");
    },
    onSuccess: (data) => {
      queryClient.refetchQueries({ queryKey: userKey });
      notificationStore.success("Профиль успешно сохранен", "Успех");
    },
    ...config,
    mutationFn: updateUserProfile,
  });
};
