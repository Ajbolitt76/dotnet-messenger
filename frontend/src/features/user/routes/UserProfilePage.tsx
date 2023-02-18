import { OtherUserProfileDto, UserProfileDto } from "@/features/user/types";
import React from "react";
import { MainLayout } from "@/components/Layout/MainLayout";
import { Head } from "@/components/Head";
import { useAuth } from "@/lib/AuthProvider";
import { Avatar } from "@vkontakte/vkui";
import { StatusDisplay } from "@/components/UI/StatusDisplay";
import './UserProfilePage.pcss';
import { UserContact } from "@/features/user/components/UserContact";
import { ProfileSkill } from "@/features/user/components/ProfileSkill";
import { Icon24PenOutline } from "@vkontakte/icons";
import { useNavigate } from "react-router-dom";
import { ContentLayout } from "@/components/Layout/ContentLayout";

export type UserProfileProps = {
  user: OtherUserProfileDto;
  isMe: boolean;
}

export const UserAvatar: React.FC<UserProfileProps> = ({ user }: UserProfileProps) => (
  <Avatar size={156} src={user.photo?.path} initials={user.name[0] + user.surname[0]}/>
)

export const UserInformation: React.FC<UserProfileProps> = ({ user, isMe }: UserProfileProps) => {
  const navigate = useNavigate();
  return (
    <div className="flex flex-col items-center gap-1">
      <UserAvatar user={user} isMe={isMe}/>
      <p className="text-gray-800 text-lg font-bold mt-4">
        {user.name} {user.surname}
        <Icon24PenOutline className="inline ml-2 cursor-pointer" onClick={() => navigate("/edit-me")}/>
      </p>
      {user.instituteName
        && <p className="text-gray-600 text-sm">
          {`${user.instituteEducationProgram ?? ""} ${user.instituteCourse ?? ""}, ${user.instituteName}`}
        </p>}
      <p
        className="text-gray-800 text-md font-semibold">Заходил: {new Date(user.lastActivity).toLocaleDateString("ru-ru")}</p>
      <StatusDisplay status={user.status}/>
    </div>
  );
};

export const UserProfile: React.FC<UserProfileProps> = ({ user, isMe }) => {
  return (
    <>
      <UserInformation user={user} isMe={isMe}/>
      <div className="flex flex-col gap-4">
        <div className="profile-section">
          <p className="profile-section__header">О себе</p>
          <div className="profile-section__splitter"/>
          <p className="profile-section__info">
            {user.about ?? "Пользователь не оставил никакой информации о себе"}
          </p>
        </div>
        <div className="profile-section">
          <p className="profile-section__header">Контакты</p>
          <div className="profile-section__splitter"/>
          <div className="profile-section__info">
            <div className="profile-contact">
              {user.contacts != null && user.contacts.length > 0
                ? user.contacts?.map((x) => <UserContact key={x.name} link={x} />)
                : <p>Пользователь не указал никаких контактов</p>}
            </div>
          </div>
        </div>
        <div className="profile-section">
          <p className="profile-section__header">Навыки</p>
          <div className="profile-section__splitter"/>
          <div className="profile-section__skills">
            {user.skills != null && user.skills.length > 0
              ? user.skills?.map((x) => <ProfileSkill key={x.skill.id} skill={x} />)
              : <p>Пользователь не указал никаких навыков</p>
            }
          </div>
        </div>
      </div>
    </>
  )
}

export type UserProfilePageProps = {
  user?: OtherUserProfileDto;
}


export const UserProfilePage: React.FC<UserProfilePageProps> = ({ user }) => {
  const auth = useAuth();
  const userToRender = user ?? auth.user;
  const isMe = (userToRender && userToRender.id === auth.user?.id) ?? false;

  return (
    <MainLayout>
      <Head title="Просомтр профиля"/>
      <ContentLayout>
        {userToRender && (<UserProfile user={userToRender} isMe={isMe}/>)}
      </ContentLayout>
    </MainLayout>
  )
}
