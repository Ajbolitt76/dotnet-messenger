import { OtherUserProfileDto, UserContactDto, UserSkillDto } from "@/features/user/types";
import { apiClient } from "@/lib/ApiClient";
import { MutationConfig, queryClient } from "@/lib/ReactQuery";
import { useMutation } from "react-query";
import { useStore } from "@/stores/RootStore";

type CreateTeamDto = {
  name: string;
}

export const createTeam = (data: CreateTeamDto): Promise<{id: number}> => {
  return apiClient.post("team", { json: data }).json();
};

type UseCreateTeam = {
  onCreated: (teamId: number) => void;
  config?: MutationConfig<typeof createTeam>;
};

const key = ["user"];

export const useCreateTeam = ({ config, onCreated }: UseCreateTeam) => {
  const { notificationStore } = useStore();

  return useMutation({
    onError: (err, __, context: any) => {
      notificationStore.error(err.message, "Ошибка при создании команды");
    },
    onSuccess: (data) => {
      queryClient.refetchQueries(["myTeams"]);
      notificationStore.success("Команада успешно сохранен", "Успех");
      onCreated(data.id);
    },
    ...config,
    mutationFn: createTeam,
  });
};
