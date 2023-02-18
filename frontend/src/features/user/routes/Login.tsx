import React from "react";
import { AuthLayout } from "@/features/user/components/AuthLayout";
import { LoginForm } from "@/features/user/components/LoginForm";

export const Login: React.FC = () => (
  <AuthLayout title={"Вход"}>
    <LoginForm />
  </AuthLayout>
)
