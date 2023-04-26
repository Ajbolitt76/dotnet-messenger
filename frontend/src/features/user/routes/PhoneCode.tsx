import { AuthLayout } from "../components/AuthLayout";
import { PhoneCodeForm } from "../components/PhoneCodeForm";

export const PhoneCode: React.FC = () => (
    <AuthLayout title={"Вход или регистрация"}>
      <PhoneCodeForm />
    </AuthLayout>
  )
  