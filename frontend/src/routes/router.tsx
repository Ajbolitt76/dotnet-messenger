import React from "react";
import { createBrowserRouter, RouteObject } from "react-router-dom";
import { ModuleRoutes } from "@/types";
import { UserModuleRoutes } from "@/features/user/routes";
import { AuthorizedRouteWrapper } from "@/routes/AuthorizedRouteWrapper";
import { AnonymousRouteWrapper } from "@/routes/AnonymousRouteWrapper";
import { ErrorFallback } from "@/components/ErrorFallback";
import { ChatModuleRoutes } from "@/features/chats/routes";
import { LandingPage } from "@/features/landing/routes";

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
    element: <LandingPage />,
  },
];

function registerModuleRoutes(routes: ModuleRoutes) {
  anonymousRoutes.children!.push(...routes.anonymous);
  authorizedRoutes.children!.push(...routes.authenticated);
  commonRoutes.push(...routes.common);
}

registerModuleRoutes(UserModuleRoutes);
registerModuleRoutes(ChatModuleRoutes);

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
