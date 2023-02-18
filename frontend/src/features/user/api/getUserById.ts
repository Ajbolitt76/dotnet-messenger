import {AsyncReturnType} from "type-fest";
import {useQuery} from "react-query";
import {apiClient} from "@/lib/ApiClient";
import {OtherUserProfileDto} from "@/features/user/types";
import {QueryConfig} from "@/lib/ReactQuery";

export const getUserById = (id: number): Promise<OtherUserProfileDto> => {
    return apiClient.get("user", {
        searchParams: {
            id
        }
    }).json();
}

type ReturnType = AsyncReturnType<typeof getUserById>;

type UseGetUserById = {
    userId: number;
    config?: QueryConfig<ReturnType>;
};

export const useGetUserById = ({config, userId}: UseGetUserById) => {
    return useQuery<ReturnType>( {
        ...config,
        queryKey: ["user", userId],
        queryFn: () => getUserById(userId),
    });
}
