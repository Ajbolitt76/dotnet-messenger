import React from "react";
import { MainLayout } from "@/components/Layout/MainLayout";
import { Head } from "@/components/Head";
import { Card } from "@vkontakte/vkui";

type LayoutProps = {
  children: React.ReactNode;
  title: string;
};

export const AuthLayout = ({ children, title }: LayoutProps) => {
  return (
    <>
      <Head title={title}/>
      <div className="flex justify-center items-center min-h-full w-full">
        <Card mode="shadow" className="w-full sm:w-96">
          <div className="flex flex-col items-center p-4">
            <img className="w-3/4 my-6" src="/src/assets/kfu_logo_2l.png" alt="Логотип кфу"/>
            {children}
          </div>
        </Card>
      </div>
    </>
  );
};
