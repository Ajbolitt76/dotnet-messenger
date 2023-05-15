import React, { useEffect, useRef, useState } from "react"
import { useConversationMessages } from "../api/getConversationMessages"
import { useParams } from "react-router-dom";
import { Group, Input, WriteBar, WriteBarIcon } from "@vkontakte/vkui";
import { useAuth } from "@/lib/AuthProvider";
import clsx from "clsx";
import { ConversationInfoInstance, ConversationMessageInstance, MessageState } from "../stores/MessagesStore";
import { Icon16CheckDoubleOutline, Icon16CheckOutline, Icon16ClockOutline, Icon16ErrorCircleFill } from "@vkontakte/icons";
import { observer } from "mobx-react-lite";
import { useStore } from "@/stores/RootStore";
import { nanoid } from "nanoid";
import dayjs from "dayjs";
import styles from './MessagesView.module.pcss';
import { useActivatableScrollbar } from "@/lib/hooks/useActivatableScorllbar";

export const MessagesView: React.FC = observer(() => {
  const { chatId } = useParams<{ chatId: string }>();
  const { conversationsStore: { conversations } } = useStore();
  const messageRef = useRef<HTMLDivElement>(null);

  if (chatId == null)
    return (<></>)
  const conversation = conversations.get(chatId ?? '');

  const { isFetchingNextPage, isError, hasNextPage, fetchNextPage } = useConversationMessages(chatId!);
  const scrollRef = useActivatableScrollbar();

  return (
    <Group className="w-1/2 min-w-[300px] flex flex-col justify-end" mode="plain">
      <div ref={scrollRef} className="h-full max-h-full overflow-y-auto flex flex-col-reverse pb-2 pr-2">
        {
          conversation?.orderedMessages.map(y => (<MessageItemView message={y!} key={y!.messageId} />)).reverse()
        }
      </div>
      <TgWriteBar conversation={conversation!} />
    </Group>
  )
})

const TgWriteBar: React.FC<{
  conversation: ConversationInfoInstance
}> = ({ conversation }) => {
  const [text, setText] = useState('');
  const { user } = useAuth();

  const submit = () => {
    if (text.replaceAll('\n', '').length == 0)
      return;

    conversation.sendMessage(user!.id,
      nanoid(),
      {
        message: text.trim(),
      });

    console.log(text);
    setText('')
  }


  return (
    <WriteBar
      className="rounded-lg"
      after={
        <>
          <WriteBarIcon onClick={() => submit()} mode="send" />
        </>
      }
      placeholder="Сообщение"
      value={text}
      onChange={x => setText(x.target.value)}
      onKeyDown={x => {
        if (x.key == "Enter" && !x.shiftKey) {
          x.preventDefault();
          submit();
        }
      }}
    />
  )
}

// TODO: separate message views for diffrent chat types
const MessageItemView: React.FC<{
  message: ConversationMessageInstance
}> = observer(({ message }) => {
  const { user } = useAuth();
  const ownMessage = user?.id == message.sentBy;

  return (
    <div className={clsx(
      styles["message-item"],
      "flex flex-col bg-gray-800 w-fit px-2 py-1 rounded-lg mb-1 max-w-[60%]",
      ownMessage && "self-end",
      message.isSystem && "self-center")}>
      <p className={clsx("whitespace-pre-line break-all max-w-full", message.isSystem && "italic")}>
        {message.content}
      </p>
      <p className="text-sm text text-steel-gray-200 self-end">
        {dayjs(message.sentAt).format('HH:mm')}
        {message.editedAt && '(ред)'}
        {ownMessage && <DeliveryIndicator state={message.messageState} />}
      </p>
    </div>
  )
});


const DeliveryIndicator: React.FC<{
  state: MessageState
}> = ({ state }) => {
  const classNames = "inline-block ml-2"
  switch (state) {
    case "pending":
      return (<Icon16ClockOutline className={classNames} />)
    case "delivered":
      return (<Icon16CheckOutline className={classNames} />)
    case "read":
      return (<Icon16CheckDoubleOutline className={classNames} />)
    case "failed":
      return (<Icon16ErrorCircleFill className={classNames} />)
  }
}