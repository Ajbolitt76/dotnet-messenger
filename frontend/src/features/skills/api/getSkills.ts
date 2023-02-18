import { apiClient } from "@/lib/ApiClient";
import { SkillDto } from "@/lib/ApiTypes";
import { useQuery, UseQueryResult } from "react-query";
import { ExtractFnReturnType, QueryConfig } from "@/lib/ReactQuery";
import { AsyncReturnType } from "type-fest";

export const getSkills = (): Promise<SkillDto[]> => {
  return apiClient.get("skills").json();
}

type ReturnType = AsyncReturnType<typeof getSkills>;

type UseSkillsOptions = {
  config?: QueryConfig<ReturnType>;
};

export const useSkills = ({config}: UseSkillsOptions = {}) => {
  return useQuery<ReturnType>( {
    ...config,
    queryKey: ["skills"],
    queryFn: () => getSkills(),
  });
}
