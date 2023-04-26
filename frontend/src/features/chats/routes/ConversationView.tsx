import React from "react"
import { MessagesView } from "../components/MessagesView"

export const ConversationView: React.FC = () => {
  return (
    <div className="w-full flex justify-center">
        <MessagesView />
    </div>
  )
}