import React from "react";
import { MainLayout } from "@/components/Layout/MainLayout";
import { Head } from "@/components/Head";
import { ContentLayout } from "@/components/Layout/ContentLayout";
import { MyWork } from "@/features/user/components/MyWork";

export const MyWorkPage: React.FC = () => (
  <MainLayout>
    <Head title="Моя работа"/>
    <ContentLayout>
      <MyWork />
    </ContentLayout>
  </MainLayout>
)
