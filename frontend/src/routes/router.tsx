import React from "react";
import { createBrowserRouter, Outlet, RouteObject } from "react-router-dom";
import { MainLayout } from "@/components/Layout/MainLayout";
import { Landing } from "@/features/landing/routes/Landing";
import { ModuleRoutes } from "@/types";
import { UserModuleRoutes } from "@/features/user/routes";
import { AuthorizedRouteWrapper } from "@/routes/AuthorizedRouteWrapper";
import { AnonymousRouteWrapper } from "@/routes/AnonymousRouteWrapper";
import { ErrorFallback } from "@/components/ErrorFallback";
import {TeamModuleRoutes} from "@/features/teams/routes";

export const authorizedRoutes: RouteObject = {
  path: "/",
  element: <AuthorizedRouteWrapper/>,
  children: []
}

export const anonymousRoutes: RouteObject = {
  path: "/",
  element: <AnonymousRouteWrapper/>,
  children: []
}

export const commonRoutes: RouteObject[] = [
  {
    path: "/",
    element: <Landing/>,
  },
];

function registerModuleRoutes(routes: ModuleRoutes) {
  anonymousRoutes.children!.push(...routes.anonymous);
  authorizedRoutes.children!.push(...routes.authenticated);
  commonRoutes.push(...routes.common);
}

registerModuleRoutes(UserModuleRoutes);
registerModuleRoutes(TeamModuleRoutes);

export const router = createBrowserRouter([
  {
    path: "/",
    errorElement: <ErrorFallback />,
    children: [
      ...commonRoutes,
      anonymousRoutes,
      authorizedRoutes,
    ]
  }]);
