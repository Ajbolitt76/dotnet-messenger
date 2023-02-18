import React from "react";
import {ModuleRoutes} from "@/types";
import {TeamPage} from "@/features/teams/routes/TeamPage";
import {CreateTeamPage} from '@/features/teams/routes/CreateTeamPage';

export const TeamModuleRoutes: ModuleRoutes = {
  anonymous: [],
  authenticated: [
    {
      path: "/team/new",
      element: <CreateTeamPage /> 
    },
    {
      path: "/team/:teamId",
      element: <TeamPage />
    },
  ],
  common: [],
};