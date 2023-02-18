import React from "react";
import { useAuth } from "@/lib/AuthProvider";
import { MainLayout } from "@/components/Layout/MainLayout";
import { Head } from "@/components/Head";
import { ProfileEditor } from "@/features/user/components/ProfileEditor";
import { ContentLayout } from "@/components/Layout/ContentLayout";

export const ProfileEditorPage: React.FC = () => {
  const auth = useAuth();
  const userToRender = auth.user;

  return (
    <MainLayout>
      <Head title="Редактор профиля"/>
      <ContentLayout>
        {userToRender && (<ProfileEditor initialUser={userToRender}/>)}
      </ContentLayout>
    </MainLayout>
  )
}
