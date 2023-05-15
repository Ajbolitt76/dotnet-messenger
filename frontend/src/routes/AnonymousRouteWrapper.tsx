import React from "react";
import { Navigate, Outlet, RouteObject, useLocation } from "react-router-dom";
import { useAuth } from "@/lib/AuthProvider";

export const AnonymousRouteWrapper: React.FC = () => {
  const auth = useAuth();
  const location = useLocation();

  return auth.user == null ? (<Outlet/>) : (<Navigate to={location.state.returnPath} />);
}
