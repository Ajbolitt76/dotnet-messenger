import { Splitter } from "@/components/UI/Splitter";
import { useAuth } from "@/lib/AuthProvider";
import { Avatar, Cell, CellButton, Group, Spinner } from "@vkontakte/vkui";
import React, { ReactElement } from "react";
import { useConversationList } from "../api/getConversations";
import { Icon24PenOutline } from "@vkontakte/icons";
import { useModal } from "@/components/Modal";
import { CreatePersonalChatModal } from "./Modals/CreatePersonalChatModal";
import { NavLink } from "react-router-dom";
import * as dayjs from 'dayjs'
import styles from './MessengerSidebar.module.pcss';
import { ConversationInfoInstance } from "../stores/MessagesStore";
import { observer } from "mobx-react-lite";
import { useStore } from "@/stores/RootStore";

export const MessengerSidebar = observer(() => {
  return (
    <div className="flex flex-col w-[300px] h-full bg-gray-800">
      <UserProfileCard />
      <Splitter className="bg-gray-600" />
      <div className="flex-1">
        <ConversationList />
      </div>
    </div>
  );
});

const UserProfileCard = () => {
  const { user, logout } = useAuth();
  return (
    <div className="p-4 flex-none flex flex-col gap-3">
      <Avatar size={52} initials={user?.name[0]}>
        <Avatar.BadgeWithPreset preset="online" />
      </Avatar>
      <p className="text-lg leading-5 text-white">
        {user?.name}
        <br />
        @{user?.userName}
        <br />
        {user?.id}
      </p>
    </div>
  )
}

const ConversationListContainer = observer(({ children }: { children: React.ReactNode }) => {
  return (<Group mode="plain">{children}</Group>)
});

const ConversationList = observer(() => {
  const { isLoading } = useConversationList();
  const { conversationsStore: {sortedConversations}} = useStore();
  const showModal = useModal<void>(CreatePersonalChatModal, false);

  return (
    <ConversationListContainer>
      <CellButton
        onClick={() => showModal()}
        before={
          <Icon24PenOutline />
        }
      >
        Создать чат
      </CellButton>
      {sortedConversations?.map(x => (<ConversationListItem conversation={x} key={x.id} />))}
      {isLoading && <Spinner></Spinner>}
    </ConversationListContainer>
  )
})

function formatLmDate(sentAt: Date): string {
  const date = dayjs(sentAt);
  const diff = dayjs().diff(date, 'days')
  if (diff < 1) {
    return date.format('HH:mm')
  } else if (diff < 7) {
    return date.format('ddd')
  }
  return date.format('DD.MM.YYYY')
}

const ConversationListItem: React.FC<{
  conversation: ConversationInfoInstance
}> = observer(({ conversation }) => {

  return (
    <NavLink
      to={`/chats/${conversation.id}`}
      className={({ isActive, isPending }) => isActive || isPending ? styles['selected-message'] : ''}
    >
      <Cell
        className={styles['conversation-preview']}
        before={
          <Avatar size={42} initials={conversation.title[0]} gradientColor="blue">
            {/* <Avatar.BadgeWithPreset preset="online" /> */}
          </Avatar>
        }
        indicator={
          <p className="mr-auto">
            {conversation.lastMessage && formatLmDate(conversation.lastMessage.sentAt)}
          </p>
        }
      >
        <div className="flex flex-col justify-stretch max-w-full">
          <p className="font-bold">
            {conversation.title}
          </p>
          <p className="text-steel-gray-200 overflow-hidden text-ellipsis">
            {conversation.lastMessage?.text}
          </p>
        </div>
      </Cell>
    </NavLink>
  )
})