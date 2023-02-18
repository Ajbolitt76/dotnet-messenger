import { RouterProvider } from "react-router-dom";
import { router } from "./router";
import React from "react";
export const AppRouter: React.FC = () => {
  return (
    <RouterProvider router={router} />
  )
};
