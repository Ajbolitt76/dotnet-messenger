import React from "react";
import {TabPanel, TabView} from "primereact/tabview";
import {Avatar, Button, Card, CellButton} from "@vkontakte/vkui";
import {useMutation, useQuery, useQueryClient} from "react-query";
import {apiClient} from "@/lib/ApiClient";
import {TeamMemberDto} from "@/features/teams/types";
import {Splitter} from "primereact/splitter";
import {SystemFileDto} from "@/lib/ApiTypes";
import {useStore} from "@/stores/RootStore";
import {Simulate} from "react-dom/test-utils";
import error = Simulate.error;
import {useNavigate} from "react-router-dom";

export const MyInvites: React.FC = () => {
  const {notificationStore} = useStore();
  const queryProvider = useQueryClient();
  const {data, isLoading} = useQuery("myInvites", () => {
    return apiClient.get("teams/my-invites").json<{
      teamId: number;
      name: string;
      photo?: SystemFileDto;
    }[]>();
  })

  const accept = async (teamId: number) => {
    try {
      await apiClient.get(`team/apply`, {
        searchParams: {
          teamId
        }
      }).json();
      notificationStore.success("Вы успешно присоединились к команде", "Успех");
      queryProvider.invalidateQueries("myInvites");
      queryProvider.invalidateQueries("myTeams");

    } catch (e) {
      notificationStore.error("Не удалось принять приглашение", "Ошибка");
    }
  }

  const decline = async (teamId: number) => {
    try {
      await apiClient.get(`team/leave`, {
        searchParams: {
          teamId
        }
      }).json();
      notificationStore.success("Вы успешно отклонили приглашение", "Успех");
      queryProvider.invalidateQueries("myInvites");

    } catch (e) {
      notificationStore.error("Не удалось отклонить приглашение", "Ошибка");
    }
  }

  return (
    <div className="flex flex-col ">
      {isLoading && <div>Загрузка...</div>}
      {data && data.map(teamMember => (
        <div key={teamMember.teamId} className="flex gap-3 justify-between items-center">
          <Avatar className="" src={teamMember.photo?.path} initials={teamMember.name[0]}/>
          <div className="mr-auto">
            {teamMember.name}
          </div>
          <Button onClick={() => accept(teamMember.teamId)} appearance="positive">Принять</Button>
          <Button onClick={() => decline(teamMember.teamId)} appearance="negative">Отклонить</Button>
          <Splitter/>
        </div>))
      }
    </div>
  )
}

export const MyTeams: React.FC = () => {
  const {notificationStore} = useStore();
  const navigate = useNavigate();

  const queryProvider = useQueryClient();
  const {data, isLoading} = useQuery("myTeams", () => {
    return apiClient.get("teams/my").json<{
      teamId: number;
      name: string;
      isAdmin: boolean;
      photo?: SystemFileDto;
    }[]>();
  })

  const leave = async (teamId: number) => {
    try {
      await apiClient.get(`team/leave`, {
        searchParams: {
          teamId
        }
      }).json();
      notificationStore.success("Вы успешно покинули команду", "Успех");
      queryProvider.invalidateQueries("myTeams");

    } catch (e) {
      notificationStore.error("Не удалось покинуть команду", "Ошибка");
    }
  }

  return (
    <div className="flex flex-col gap-3">
      {isLoading && <div>Загрузка...</div>}
      {data && data.map(teamMember => (

        <div className="flex items-center">
          <Avatar className="mr-3" src={teamMember.photo?.path} initials={teamMember.name[0]}/>
          <CellButton onClick={() => navigate(`/team/${teamMember.teamId}`)}>
            <div className="mr-auto">
              {teamMember.name}
            </div>
          </CellButton>
          {!teamMember.isAdmin && <Button onClick={() => leave(teamMember.teamId)}>Покинуть</Button>}
          <Splitter/>
        </div>))
      }
      <Button onClick={() => navigate("/team/new")} size="l" className="mt-2">Создать команду</Button>
    </div>
  )
}


export const MyWork: React.FC = () => {
  return (
    <div className="flex w-full justify-between">
      <Card mode="shadow" className="p-5 w-6/12">
        <h2 className="text-3xl font-bold">Ваши команды</h2>
        <TabView>
          <TabPanel header="Мои команды">
            <MyTeams/>
          </TabPanel>
          <TabPanel header="Приглашения">
            <MyInvites/>
          </TabPanel>
        </TabView>
      </Card>
      <Card mode="shadow" className="p-5 w-5/12">
        <h2 className="text-3xl font-bold">Проекты</h2>

      </Card>
    </div>
  )
}
