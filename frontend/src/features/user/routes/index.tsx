import React from "react";
import { RouteObject } from "react-router-dom";
import { Login } from "./Login";
import { Register } from "./Register";
import { UserProfilePage } from "@/features/user/routes/UserProfilePage";
import { ModuleRoutes } from "@/types";
import { ProfileEditorPage } from "@/features/user/routes/ProfileEditorPage";
import { MyWorkPage } from "@/features/user/routes/MyWorkPage";
import {OtherUserPage} from "@/features/user/routes/OtherUserPage";

export const UserModuleRoutes: ModuleRoutes = {
  anonymous: [
    {
      path: "/login",
      element: <Login/>,
    },
    {
      path: "/register",
      element: <Register/>,
    }
  ],
  authenticated: [
    {
      path: "/me",
      element: <UserProfilePage />
    },
    {
      path: "/edit-me",
      element: <ProfileEditorPage />
    },
    {
      path: "/my-work",
      element: <MyWorkPage />
    }
  ],
  common: [
    {
      path: "/user/:id",
      element: <OtherUserPage />
    }
  ]
}
