import React from "react";
import { useAuth } from "@/lib/AuthProvider";
import { Navigate, Outlet, RouteObject, useLocation } from "react-router-dom";

export const AuthorizedRouteWrapper: React.FC = () => {
  const auth = useAuth();
  const location = useLocation();
  return auth.user != null ? (<Outlet />) : (<Navigate to="/login" state={{ returnPath: location.pathname }} />);
}
