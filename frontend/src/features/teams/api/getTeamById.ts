import {apiClient} from "@/lib/ApiClient";
import {AsyncReturnType} from "type-fest";
import {QueryConfig} from "@/lib/ReactQuery";
import {useQuery} from "react-query";
import {TeamByIdDto} from "@/features/teams/types";

export const getTeamById = (id: number): Promise<TeamByIdDto> => {
  return apiClient.get("team", {
    searchParams: {
      id
    }
  }).json();
}

type ReturnType = AsyncReturnType<typeof getTeamById>;

type UseGetTeamById = {
  teamId: number;
  config?: QueryConfig<ReturnType>;
};

export const useGetTeamById = ({config, teamId}: UseGetTeamById) => {
  return useQuery<ReturnType>( {
    ...config,
    queryKey: ["team", teamId],
    queryFn: () => getTeamById(teamId),
  });
}
