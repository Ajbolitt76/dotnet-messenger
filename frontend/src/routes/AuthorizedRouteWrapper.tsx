import React from "react";
import { useAuth } from "@/lib/AuthProvider";
import { Navigate, Outlet, RouteObject } from "react-router-dom";

export const AuthorizedRouteWrapper: React.FC = () => {
  const auth = useAuth();

  return auth.user != null ? (<Outlet/>) : (<Navigate to="/login"/>);
}
