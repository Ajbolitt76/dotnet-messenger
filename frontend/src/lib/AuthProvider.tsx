import React from "react";
import { MeUserInfo } from "@/features/user/types";
import {
  QueryObserverResult,
  RefetchOptions,
  UseMutateAsyncFunction,
  useMutation,
  useQuery,
  useQueryClient
} from "@tanstack/react-query";
import { BaseError } from "@/lib/ApiTypes";
import { login, LoginRequestDto, LoginResponseDto } from "@/features/user/api/login";
import { getMyUser } from "@/features/user/api/getMyUser";
import { tokenStore } from "@/lib/ApiClient";
import { register, RegisterRequestDto, RegisterResponseDto } from "@/features/user/api/register";
import { HTTPError } from "ky";

type AuthContextValue = {
  user: MeUserInfo | null
  login: UseMutateAsyncFunction<LoginResponseDto, BaseError, LoginRequestDto>
  register: UseMutateAsyncFunction<RegisterResponseDto, BaseError, RegisterRequestDto>
  logout: () => void;
  isLoggingIn: boolean;
  isRegistering: boolean;
  refetchUser: (
    options?: RefetchOptions | undefined
  ) => Promise<QueryObserverResult<MeUserInfo | null, BaseError>>;
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
    isSuccess,
    refetch
  } = useQuery<MeUserInfo | null, BaseError>({
    queryKey: userKey,
    queryFn: async () => {
      if (tokenStore.Token) {
        try {
          return await getMyUser();
        } catch (e) {
        }
      }
      return null;
    },
  });

  const setUser = React.useCallback(
    (data: MeUserInfo) => queryClient.setQueryData(userKey, data),
    [queryClient]
  );

  const loginMutation = useMutation<LoginResponseDto, BaseError, LoginRequestDto>({
    mutationFn: login,
    onSuccess: response => {
      tokenStore.UpsertFromDto(response);
      queryClient.invalidateQueries({ queryKey: userKey });
    },
  });

  const registerMutation = useMutation<RegisterResponseDto, BaseError, RegisterRequestDto>({
    mutationFn: register,
    onSuccess: response => {
      tokenStore.UpsertFromDto(response);
      queryClient.invalidateQueries({ queryKey: userKey });
    },
  });

  const logout = React.useCallback(() => {
    tokenStore.UpsertFromDto(undefined);
    queryClient.invalidateQueries({ queryKey: userKey });
  }, [queryClient]);


  const value = React.useMemo<AuthContextValue>(
    () => ({
      user: user ?? null,
      error,
      refetchUser: refetch,
      login: loginMutation.mutateAsync,
      register: registerMutation.mutateAsync,
      isLoggingIn: loginMutation.isPending,
      logout,
      isRegistering: registerMutation.isPending,
    }),
    [
      user,
      error,
      refetch,
      loginMutation.mutateAsync,
      loginMutation.isPending,
      registerMutation.mutateAsync,
      registerMutation.isPending,
    ]
  );

  if (isSuccess) {
    return (
      <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
    );
  }

  if (isLoading) {
    return (<div className="w-screen h-screen flex justify-center items-center">
      Загрузка профиля...
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
