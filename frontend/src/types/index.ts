import { RouteObject } from "react-router-dom";

export type TargetType = 'document' | 'window' | React.Ref<HTMLElement> | undefined;

export type ModuleRoutes = {
  anonymous: RouteObject[];
  authenticated: RouteObject[];
  common: RouteObject[];
}
export type FCProps<C> = C extends React.FC<infer P> ? P : never;
