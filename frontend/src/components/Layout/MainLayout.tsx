import React from "react";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { Avatar, Button } from "@vkontakte/vkui";
import { useAuth } from "@/lib/AuthProvider";
import { Icon20LockOutline } from "@vkontakte/icons";
import clsx from "clsx";

type MainLayoutProps = {
  children: React.ReactNode;
};

const Logo: React.FC = () => (
  <Link className="flex items-center text-white pl-3" to="/">
    <img className="h-8 w-auto" src="/src/assets/logo.png" alt="Лого "/>
  </Link>
);

type SideNavigationItem = {
  name: string;
  to: string;
};

const NavItems: React.FC = () => {
  const { user } = useAuth();
  const navs: SideNavigationItem[] = [
    {
      name: "Мои проекты",
      to: "/projects",
    },
    {
      name: "Главная",
      to: "/",
    }
  ];

  if(user){
    navs.push({
      name: "Моя работа",
      to: "/my-work",
    });
  }

  return (
    <>
      {navs.map((nav) => (
        <NavLink
          end
          key={nav.to}
          to={nav.to}
          className={({isActive}) => clsx(
            "flex h-full items-center font-semibold text-lg",
            isActive ? "font-bold text-blue-800" : "font-semibold text-gray-500",
          )}
        >
          {nav.name}
        </NavLink>
      ))}
    </>
  )
}

const UserDisplay: React.FC = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  return user
    ? (<div className="flex items-center">
      <div
        onClick={() => navigate("/me")}
        className="flex items-center gap-2 rounded-3xl p-2 transition hover:cursor-pointer hover:bg-steel-gray-40"
      >
        <Avatar size={42} initials={user.name[0]+user.surname[0]}>
          <Avatar.BadgeWithPreset preset="online"/>
        </Avatar>
        <p className="font-semibold text-lg text-gray-700">
          {user.name} {user.surname[0]}.
        </p>
      </div>
      <Button onClick={() => logout()}>Выйти</Button>
    </div>)
    : (
      <Button onClick={() => navigate("/login")} before={<Icon20LockOutline/>} size="l">
        Войти
      </Button>)
}

export const MainLayout = ({ children }: MainLayoutProps) => {
  return (
    <div className="h-screen flex overflow-hidden bg-gray-100">
      <div className="flex flex-col w-0 flex-1 overflow-hidden">
        <div className="relative z-10 flex-shrink-0 flex h-16 bg-white shadow">
          <Logo/>
          <div className="flex-1 px-4 flex justify-end">
            <div className="ml-4 flex items-center gap-5">
              <NavItems/>
              <UserDisplay/>
            </div>
          </div>
        </div>
        <main className="flex-1 relative overflow-y-auto focus:outline-none">{children}</main>
      </div>
    </div>
  )
}
