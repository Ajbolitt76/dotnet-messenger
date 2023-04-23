import { Login } from "./Login";
import { PhoneCode } from "./PhoneCode";
import { Register } from "./Register";
import { ModuleRoutes } from "@/types";

export const UserModuleRoutes: ModuleRoutes = {
  common: [],
  anonymous: [
    {
      path: "/login",
      element: <Login/>,
    },
    {
      path: "/register",
      element: <Register/>,
    },
    {
      path: "/phone-code",
      element: <PhoneCode />
    }
  ],
  authenticated: [
  ]
}
