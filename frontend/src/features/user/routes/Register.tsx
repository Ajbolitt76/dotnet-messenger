import React from "react";
import { AuthLayout } from "@/features/user/components/AuthLayout";
import { RegisterForm } from "@/features/user/components/RegisterForm";

export const Register: React.FC = () => (
  <AuthLayout title={"Регистрация"}>
    <RegisterForm />
  </AuthLayout>
)
