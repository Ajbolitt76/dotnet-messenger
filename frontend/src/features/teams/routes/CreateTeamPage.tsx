import React, {FormEvent, useCallback, useState} from "react";
import {useAuth} from "@/lib/AuthProvider";
import {MainLayout} from "@/components/Layout/MainLayout";
import {Head} from "@/components/Head";
import {ContentLayout} from "@/components/Layout/ContentLayout";
import {ProfileEditor} from "@/features/user/components/ProfileEditor";
import {Button, FormItem, FormLayout, Input} from "@vkontakte/vkui";
import {useNavigate} from "react-router-dom";
import {useCreateTeam} from "@/features/teams/api/createTeam";

export const CreateTeamPage: React.FC = () => {
  const [teamName, setTeamName] = useState("");
  const navigate = useNavigate();
  const onCreated = useCallback((id: number) => {
    navigate(`/team/${id}`);
  }, [navigate]);

  const {mutate, isLoading} = useCreateTeam({onCreated});
  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    mutate({name: teamName});
  }

  return (
    <MainLayout>
      <Head title="Редактор профиля"/>
      <ContentLayout>
        <div className="elevated-8 rounded-2xl py-4 px-2">
          <h2 className="text-3xl font-bold ml-4">Создание команды</h2>
          <FormLayout onSubmit={(e) => onSubmit(e)}>
            <FormItem top="Название команды">
              <Input type="text" value={teamName} onChange={(e) => setTeamName(e.target.value)}/>
            </FormItem>
            <Button
              type="submit" 
              loading={isLoading}
              disabled={isLoading}
              className="ml-4" size="l" mode="primary">Создать</Button>
          </FormLayout>
        </div>
      </ContentLayout>
    </MainLayout>
  )
}