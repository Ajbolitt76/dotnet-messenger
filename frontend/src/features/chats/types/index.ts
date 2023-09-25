export enum ConversationTypes {
  PersonalChat = "Personal",
  Channel = "Channel",
  GroupChat = "Group",
}

export enum RealtimeUpdateTypes {
  NewMessage = "NEW_MESSAGE",
  DeletedMessage = "DELETED_MESSAGE",
}

export type UpdateMessage = NewMessageRealtimeUpdate;

interface IRealtimeUpdate {
  $type: RealtimeUpdateTypes;
}

export interface NewMessageRealtimeUpdate extends IRealtimeUpdate{
  $type: RealtimeUpdateTypes.NewMessage;
  conversationId: string,
  data: {
    messageId: string,
    sentBy: string,
    content: string,
    sentAt: string,
    editedAt: string,
    positon: number
  }
}

export interface DeletedMessageRealtimeUpdate extends IRealtimeUpdate{
  $type: RealtimeUpdateTypes.NewMessage;
  conversationId: string;
  messageId: string;
}