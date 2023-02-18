import React from "react";
import { Navigate, Outlet, RouteObject } from "react-router-dom";
import { useAuth } from "@/lib/AuthProvider";

export const AnonymousRouteWrapper: React.FC = () => {
  const auth = useAuth();

  return auth.user == null ? (<Outlet/>) : (<Navigate to="/me"/>);
}
