import { Instance, SnapshotIn, flow, getRoot, getType, types } from "mobx-state-tree";
import { ConversationTypes } from "../types";
import { GetChatListResponse, getConversations } from '../api/getConversations';
import { GetMessagesRequestParams, GetMessagesResponse, getConversationMessages } from '../api/getConversationMessages';
import { SendMessageRequest, SendMessageResponse, sendMessageToChat } from "../api/sendMessage";
import { IsInstanceOf, getTgRoot } from "@/stores/storeUtils";
import { nanoid } from "nanoid";
import { RootModelType } from "@/stores/RootStore";
import { CreatePersonalConversationResponse, createPersonalChat } from "../api/personalChat/createPersonalChat";
import { localizeSystemMessage } from "../localizeSystemMessage";

export type MessageState = "failed" | "pending" | "delivered" | "read"

const ActualConversationMessage = types.model('ActualConversationMessage', {
  messageId: types.identifier,
  sentBy: types.string,
  content: types.string,
  sentAt: types.Date,
  editedAt: types.maybeNull(types.Date),
}).views(self => ({
  get messageState(): MessageState {
    return "delivered"
  },
  get isSystem(): boolean{
    return self.sentBy == '00000000-0000-0000-0000-000000000000'
  }
}))
.actions(self => ({
  afterCreate() {
    if(self.isSystem){
      self.content = localizeSystemMessage(self.content);
    }
  }
}));

const PENDING_MESSAGE_PREFIX = 'pending'

const PendingConversationMessage = ActualConversationMessage
  .named('PendingConversationMessage')
  .props({
    isPending: types.optional(types.boolean, true),
    isFailed: types.optional(types.boolean, false)
  })
  .views(self => ({
    get messageState(): MessageState {
      if (self.isPending)
        return "pending"

      return "failed"
    }
  }));;

const ConversationMessage = types.union({
  dispatcher: (x) => {
    if (typeof x?.isPending == 'boolean'
      || typeof x?.messageId == 'string' && x.messageId.startsWith(PENDING_MESSAGE_PREFIX)) {
      return PendingConversationMessage;
    }
    return ActualConversationMessage;
  }
}, PendingConversationMessage, ActualConversationMessage);

export type ConversationMessageInstance = Instance<typeof ConversationMessage>;

export const MessageStore = types.model({
  id: types.identifier,
  messages: types.map(ConversationMessage),
  orderedMessages: types.array(types.safeReference(ConversationMessage))
}).actions(self => ({
  addLoadedMessages(data: GetMessagesResponse) {
    return data.messages.map(x => {
      const res = self.messages.put({
        ...x,
        sentAt: new Date(x.sentAt),
        editedAt: x.editedAt ? new Date(x.editedAt) : null,
      });

      self.orderedMessages.push(res);

      return res;
    })
  },
  addMessage(msg: SnapshotIn<typeof ConversationMessage>) {
    const res = self.messages.put(msg);
    self.orderedMessages.unshift(res);
    return res;
  },
}))
  .actions(self => ({
    setMessageSent(id: string, serverId: string) {
      const node = self.messages.get(id);
      if (node == null)
        throw new Error('Message was not found, cant change stratus');

      if (getType(node) != PendingConversationMessage)
        return;

      self.addMessage({
        messageId: serverId,
        content: node.content,
        sentBy: node.sentBy,
        sentAt: new Date()
      });
      self.messages.delete(id);
    },
    setMessageFailed(id: string) {
      const node = self.messages.get(id);
      if (node == null)
        throw new Error('Message was not found, cant change stratus');

      if (IsInstanceOf(node, PendingConversationMessage)) {
        node.isFailed = true;
        node.isPending = false;
      }
      return;
    }
  }));

type LastMessageInfo = {
  authorName: string;
  text: string;
  sentAt: Date;
};

const ConversationInfo = types.model({
  id: types.identifier,
  title: types.string,
  type: types.enumeration<ConversationTypes>("ConversationTypes", Object.values(ConversationTypes)),
  initialLastMessage: types.frozen<LastMessageInfo>(),
  messageStore: types.reference(MessageStore)
})
  .actions(self => ({
    processMessageBatch(data: GetMessagesResponse) {
      return self.messageStore.addLoadedMessages(data);
    },
  }))
  .actions(self => ({
    loadMessagesFromPointer: flow(
      function* loadMessagesFromPointer(fetchInfo: GetMessagesRequestParams) {
        const bookListRaw = yield getConversationMessages(self.id, fetchInfo);
        return self.processMessageBatch(bookListRaw);
      }
    ),
    sendMessage: flow(
      function* sendMessage(userId: string, idempotencyKey: string, messageInfo: Omit<SendMessageRequest, 'conversationId'>) {
        const msgId = `${PENDING_MESSAGE_PREFIX}_${idempotencyKey}`

        const tempMessage = (self.messageStore.messages.get(msgId)
          ?? self.messageStore.addMessage({
            messageId: `${PENDING_MESSAGE_PREFIX}_${idempotencyKey}`,
            content: messageInfo.message,
            sentBy: userId,
            sentAt: new Date(),
            isPending: true
          })) as Instance<typeof PendingConversationMessage>

        let responseRaw: SendMessageResponse;
        try {
          responseRaw = yield sendMessageToChat({
            conversationId: self.id,
            ...messageInfo
          });
        } catch (e: unknown) {
          self.messageStore.setMessageFailed(tempMessage.messageId)
          return
        }

        self.messageStore.setMessageSent(tempMessage.messageId, responseRaw.messageId)
        return responseRaw;
      }
    )
  }))
  .views(self => ({
    get orderedMessages() {
      return self.messageStore.orderedMessages.slice().reverse();
    },
    get lastMessage(): LastMessageInfo {
      const latest = self.messageStore?.orderedMessages[0];
      return latest
        ? {
          //TODO
          authorName: '',
          sentAt: latest.sentAt,
          text: latest.content
        }
        : self.initialLastMessage;
    }
  }));

export type ConversationInfoInstance = Instance<typeof ConversationInfo>;

export const ConversationsStore = types.model({
  conversations: types.map(ConversationInfo),
  messageStore: types.map(MessageStore)
})
  .actions(self => ({
    addConversation(data: Omit<SnapshotIn<typeof ConversationInfo>, 'messageStore'>) {
      const msgStore = self.messageStore.get(data.id)
        ?? self.messageStore.put({ id: data.id })
      return self.conversations.put({
        ...data,
        messageStore: msgStore.id
      })
    }
  }))
  .actions(self => ({
    processLoadList(data: GetChatListResponse) {
      self.conversations.clear();
      return data.items.map(x => self.addConversation({
        ...x,
        initialLastMessage: {
          ...x.lastMessage,
          sentAt: new Date(x.lastMessage.sentAt)
        }
      }))
    },
  }))
  .views(self => ({
    get sortedConversations() {
      const list = []
      for (let num of self.conversations.values()) {
        list.push(num);
      }
      return list.sort((y, x) => x.lastMessage.sentAt.getTime() - y.lastMessage.sentAt.getTime())
    }
  }))
  .actions(self => ({
    loadConversationList: flow(
      function* loadConversationList() {
        const rawConversations = yield getConversations();
        return self.processLoadList(rawConversations);
      }
    ),
  }))