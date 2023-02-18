import { StatusDto } from "@/lib/ApiTypes"
import { apiClient } from "@/lib/ApiClient";
import { QueryConfig } from "@/lib/ReactQuery";
import { AsyncReturnType } from "type-fest";
import { useQuery } from "react-query";

export type KnownEntity = "user" | "project"

export const getStatusesForEntity = (entityName: KnownEntity): Promise<StatusDto[]> =>  {
  return apiClient.get("statuses", {
    searchParams: {
      type: entityName,
    }
  }).json();
}

type ReturnType = AsyncReturnType<typeof getStatusesForEntity>;

type UseStatusesForEntityOptions = {
  type: KnownEntity;
  config?: QueryConfig<ReturnType>;
};

export const useStatusesForEntity  = ({config, type}: UseStatusesForEntityOptions) => {
  return useQuery<ReturnType>( {
    ...config,
    queryKey: ["statuses", type],
    queryFn: () => getStatusesForEntity(type),
  });
}
