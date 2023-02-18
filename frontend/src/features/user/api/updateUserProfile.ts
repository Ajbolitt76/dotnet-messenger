import { OtherUserProfileDto, UserContactDto, UserSkillDto } from "@/features/user/types";
import { apiClient } from "@/lib/ApiClient";
import { MutationConfig, queryClient } from "@/lib/ReactQuery";
import { useMutation } from "react-query";
import { useStore } from "@/stores/RootStore";

interface UserSkillUpdateDto extends Omit<UserSkillDto, "skill">{
  skill: {
    id: number;
  }
};

export type UpdateUserInfoDto = {
  stateId: number;
  photoUrl: string;
  aboutMe: string;
  skills: UserSkillUpdateDto[];
  contacts: UserContactDto[];
}

export const updateUserProfile = (data: UpdateUserInfoDto): Promise<OtherUserProfileDto> => {
  return apiClient.put("me", { json: data }).json<OtherUserProfileDto>();
};

type UseUpdateDiscussionOptions = {
  config?: MutationConfig<typeof updateUserProfile>;
};

const userKey = ["user"];

export const useUpdateProfile = ({ config }: UseUpdateDiscussionOptions = {}) => {
  const { notificationStore } = useStore();

  return useMutation({
    onError: (err, __, context: any) => {
      notificationStore.error(err.message, "Ошибка при сохранении профиля");
    },
    onSuccess: (data) => {
      queryClient.refetchQueries(userKey);
      notificationStore.success("Профиль успешно сохранен", "Успех");
    },
    ...config,
    mutationFn: updateUserProfile,
  });
};
