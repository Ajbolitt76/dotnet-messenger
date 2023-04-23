import { ModalProps } from "@/components/Modal";
import { TgModal } from "@/components/Modal/TgModal";
import { Button, FormItem, Input } from "@vkontakte/vkui";
import React, { useState } from "react";
import { useCreatePersonalChat } from "../../api/personalChat/createPersonalChat";
import { useStore } from "@/stores/RootStore";

export const CreatePersonalChatModal: React.FC<ModalProps & {}> =
  ({ reject, resolve }) => {
    const { isPending, isSuccess, mutateAsync } = useCreatePersonalChat();
    const { notificationStore } = useStore();
    const [ id, setId ] = useState('');

    async function createChat() {
      try{
        await mutateAsync({receiverId: id});
        resolve();
      }catch(e){
        
      }
    }

    return (
      <TgModal
        title="Создать приватный чат"
        onClose={(x) => reject(x)}
        footer={
          <>
            <Button
              onClick={() => createChat()}
              loading={isPending}
              appearance="positive"
            >
              Создать
            </Button>
          </>
        }>
        <div>
          <FormItem top="Guid">
            <Input
              value={id}
              onChange={(e) => setId(e.target.value)}
              type="text"
            />
          </FormItem>
        </div>
      </TgModal>
    )
  }