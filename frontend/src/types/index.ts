import { UseQueryOptions } from "@tanstack/react-query";
import { RouteObject } from "react-router-dom";

export type TargetType = 'document' | 'window' | React.Ref<HTMLElement> | undefined;

export type ModuleRoutes = {
  anonymous: RouteObject[];
  authenticated: RouteObject[];
  common: RouteObject[];
}

export type TgQueryConfig<TQ, TD = TQ, TE = Error> = Omit<
  UseQueryOptions<TQ, TE, TD>,
  'queryKey' | 'queryFn' | 'initialData'
>;

export type FCProps<C> = C extends React.FC<infer P> ? P : never;
