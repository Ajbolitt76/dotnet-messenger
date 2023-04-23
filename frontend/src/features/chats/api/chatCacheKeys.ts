export class ChatCacheKeys {
  public static conversationList = ["conv-list"]

  public static conversationMessages(id: string){
    return ["conv", id, "messages"]
  }
}