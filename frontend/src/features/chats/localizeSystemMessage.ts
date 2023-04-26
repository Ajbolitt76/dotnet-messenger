export function localizeSystemMessage(message: string, params?: Record<string, string>){
  switch(message){
    case "PERSONAL_CHAT_CREATED":
      return "Приватный чат создан";
  }
  return message;
}