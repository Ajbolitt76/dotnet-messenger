import React from "react";
import { UserProfileDto } from "@/features/user/types";
import {
  QueryObserverResult,
  RefetchOptions,
  UseMutateAsyncFunction,
  useMutation,
  useQuery,
  useQueryClient
} from "react-query";
import { BaseError } from "@/lib/ApiTypes";
import { login, LoginRequestDto, LoginResponseDto } from "@/features/user/api/login";
import { getMyUser } from "@/features/user/api/getMyUser";
import { tokenStore } from "@/lib/ApiClient";
import { register, RegisterRequestDto, RegisterResponseDto } from "@/features/user/api/register";
import { Spinner } from "@vkontakte/vkui";
import { HTTPError } from "ky";

type AuthContextValue = {
  user: UserProfileDto | null
  login: UseMutateAsyncFunction<LoginResponseDto, BaseError, LoginRequestDto>
  register: UseMutateAsyncFunction<RegisterResponseDto, BaseError, RegisterRequestDto>
  logout: () => void;
  isLoggingIn: boolean;
  isRegistering: boolean;
  refetchUser: (
    options?: RefetchOptions | undefined
  ) => Promise<QueryObserverResult<UserProfileDto | null, BaseError>>;
}

const AuthContext = React.createContext<AuthContextValue | null>(null);

export interface AuthProviderProps {
  children: React.ReactNode;
}

const userKey = ["user", "me"];
export const AuthProvider = ({ children }: AuthProviderProps) => {
  const queryClient = useQueryClient();

  const {
    data: user,
    error,
    status,
    isLoading,
    isIdle,
    isSuccess,
    refetch
  } = useQuery<UserProfileDto | null, BaseError>({
    queryKey: userKey,
    queryFn: async () => {
      if (tokenStore.Token) {
        try {
          return await getMyUser();
        } catch (e) {
          if(e instanceof HTTPError && e.response.status === 401){
            tokenStore.Token = null;
          }
        }
      }
      return null;
    },
  });

  const setUser = React.useCallback(
    (data: UserProfileDto) => queryClient.setQueryData(userKey, data),
    [queryClient]
  );

  const loginMutation = useMutation<LoginResponseDto, BaseError, LoginRequestDto>({
    mutationFn: login,
    onSuccess: response => {
      tokenStore.Token = response.token;
      queryClient.invalidateQueries(userKey);
    },
  });

  const registerMutation = useMutation<RegisterResponseDto, BaseError, RegisterRequestDto>({
    mutationFn: register,
    onSuccess: response => {
      tokenStore.Token = response.token;
      queryClient.invalidateQueries(userKey);
    },
  });

  const logout = React.useCallback(() => {
    tokenStore.Token = null;
    queryClient.invalidateQueries(userKey);
  }, [queryClient]);


  const value = React.useMemo<AuthContextValue>(
    () => ({
      user: user ?? null,
      error,
      refetchUser: refetch,
      login: loginMutation.mutateAsync,
      register: registerMutation.mutateAsync,
      isLoggingIn: loginMutation.isLoading,
      logout,
      isRegistering: registerMutation.isLoading,
    }),
    [
      user,
      error,
      refetch,
      loginMutation.mutateAsync,
      loginMutation.isLoading,
      registerMutation.mutateAsync,
      registerMutation.isLoading,
    ]
  );

  if (isSuccess) {
    return (
      <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
    );
  }

  if (isLoading || isIdle) {
    return (<div className="w-screen h-screen flex justify-center items-center">
     213123
    </div>)
  }

  if (error) {
    return (
      <div style={{ color: 'tomato' }}>{JSON.stringify(error, null, 2)}</div>
    )
  }
  return <div>Unhandled status: {status}</div>;
}

export const useAuth = () => {
  const context = React.useContext(AuthContext);
  if (context === null) {
    throw new Error("useAuth must be used within a AuthProvider");
  }
  return context;
}
