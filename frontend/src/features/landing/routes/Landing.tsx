import React from "react";
import { MainLayout } from "@/components/Layout/MainLayout";
import '../styles/landing.pcss';
import { Button } from "@vkontakte/vkui";
import { Icon16Add } from "@vkontakte/icons";
import { ProjectShowcase } from "@/features/landing/components/ProjectShowcase";

export const Landing: React.FC = () => {
  return (
    <MainLayout>
      <div className="landing-container">
        <h1>Платформа для решения ваших проблем</h1>
        <h2>Уже 20 реализованных проектов и их больше с каждым днем</h2>
        <div className="landing-container__buttons">
          <Button  size="l" before={<Icon16Add/>}>
            Подать заявку на проект
          </Button>
          <Button size="l" before={<Icon16Add/>}>
            Принять участие в проекте
          </Button>
        </div>
        <ProjectShowcase />
      </div>
    </MainLayout>
  );
}
