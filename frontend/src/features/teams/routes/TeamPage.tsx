import {MainLayout} from "@/components/Layout/MainLayout";
import {Head} from "@/components/Head";
import {ContentLayout} from "@/components/Layout/ContentLayout";
import React from "react";
import {TabPanel, TabView} from "primereact/tabview";
import {Avatar, Button, Card} from "@vkontakte/vkui";
import {TeamByIdDto, TeamMemberDto} from "@/features/teams/types";
import {useParams} from "react-router-dom";
import {useGetTeamById} from "@/features/teams/api/getTeamById";
import {useAuth} from "@/lib/AuthProvider";

export const TeamMemberCard: React.FC<{
  teamMember: TeamMemberDto;
}> = ({teamMember}) => {
  const user = teamMember.user!;
  const auth = useAuth();
  const isMe = auth.user?.id === user.id;
  return (
    <div className="elevated-4 rounded-xl p-4 flex">
      <Avatar size={64} src={user.photo?.path} initials={user.name[0] + user.surname[0]}/>
      <div className="flex flex-col gap-2 justify-center ml-4">
        <div className="text-xl font-bold">{user.name} {user.surname}</div>
        <div className="text-gray-500">{teamMember.displayRoleName}</div>
        <div className="flex gap-2">
          <Button appearance="negative">Выгнать</Button>
        </div>
      </div>
    </div>
  )
}

export const TeamMembers: React.FC<{
  team: TeamByIdDto,
  isAdmin: boolean
}> = ({team, isAdmin}) => {
  return (
    <div className="flex flex-wrap">
      {team.teamMembers?.map(teamMember => (
        <TeamMemberCard key={teamMember.user?.id} teamMember={teamMember}/>
      ))}
    </div>
  )
}

export const TeamPage: React.FC = () => {
  const teamId = Number(useParams<{
    teamId: string
  }>().teamId);
  const {user} = useAuth();
  const {data: team, isLoading} = useGetTeamById({teamId});
  const isAdmin = team?.teamMembers?.find(x => x.user?.id === user?.id)?.isAdmin ?? false;
  
  if (isNaN(teamId)) {
    return <div>Неправильные параметры</div>
  }

  return (
    <MainLayout>
      <Head title="Управление командой"/>
      <ContentLayout>
        {isLoading && <div>Загрузка...</div>}
        {team && (
          <Card mode="shadow" className="p-5 w-full">
            <h2 className="text-3xl font-bold">Управление командой</h2>
            <TabView>
              <TabPanel header="Участники">
                <TeamMembers team={team} isAdmin={isAdmin}/>
              </TabPanel>

              <TabPanel header="Заявки">123</TabPanel>
              <TabPanel header="Настройки">123</TabPanel>
            </TabView>
          </Card>
        )}
      </ContentLayout>
    </MainLayout>
  )
}