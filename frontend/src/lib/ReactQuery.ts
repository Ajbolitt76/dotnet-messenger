import { HTTPError } from 'ky';
import { QueryClient, UseQueryOptions, UseMutationOptions, DefaultOptions } from 'react-query';
import { AsyncReturnType } from 'type-fest';

const queryConfig: DefaultOptions = {
  queries: {
    useErrorBoundary: true,
    refetchOnWindowFocus: false,
    retry: false,
  },
};

export const queryClient = new QueryClient({ defaultOptions: queryConfig });

export type ExtractFnReturnType<FnType extends (...args: any) => any> = AsyncReturnType<FnType>;

export type QueryConfig<T> = Omit<
  UseQueryOptions<T>,
  'queryKey' | 'queryFn'
>;

export type MutationConfig<MutationFnType extends (...args: any) => any> = UseMutationOptions<
  ExtractFnReturnType<MutationFnType>,
  HTTPError,
  Parameters<MutationFnType>[0]
>;
