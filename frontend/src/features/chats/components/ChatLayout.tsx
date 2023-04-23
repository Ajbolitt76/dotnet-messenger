import React from "react";
import { Head } from "@/components/Head";
import { MessengerSidebar } from "./MessengerSidebar";
import { Outlet } from "react-router-dom";

type LayoutProps = {
  title: string;
};

export const ChatLayout = ({ title }: LayoutProps) => {
  return (
    <>
      <Head title={title}/>
      <div className="flex w-full h-full bg-gray-900">
        <MessengerSidebar />
        <Outlet />
      </div>
    </>
  );
};
