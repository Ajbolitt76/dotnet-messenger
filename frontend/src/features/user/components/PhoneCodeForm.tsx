import React, { useState } from "react";
import { observer } from "mobx-react-lite";
import { Button, FormItem, FormLayout, Input } from "@vkontakte/vkui";
import { useStore } from "@/stores/RootStore";
import { AuthStoreInstance } from "@/features/user/stores";
import { initiatePhoneAuth } from "../api/inititatePhoneAuth";
import { waitWithNotification } from "@/lib/utils";
import { verifyPhoneCode } from "../api/verifyPhoneCode";
import { useLocation, useNavigate } from "react-router-dom";

enum Steps {
  Phone,
  Code
}

const StepPhone = observer<{
  authStore: AuthStoreInstance;
  toNextStep: () => void
}>(({ authStore, toNextStep }) => {
  const [phone, setPhone] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  async function tryAuth() {
    setIsLoading(true);
    var result = await waitWithNotification(initiatePhoneAuth({ phone }));
    if (result.success) {
      authStore.phoneTicket.updateFromInitiate(result.data);
      authStore.phoneTicket.setPhone(phone);
      toNextStep();
    }
    setIsLoading(false);
  }

  return (
    <>
      <FormItem top="Номер телефона">
        <Input
          type="text"
          onInput={(e) => setPhone(e.currentTarget.value)}
          value={phone}
        />
      </FormItem>
      <FormItem className="flex justify-center">
        <Button type="submit" onClick={() => tryAuth()} loading={isLoading} size="l">
          Получить код
        </Button>
      </FormItem>
    </>
  )
});

const StepCode = observer<{
  authStore: AuthStoreInstance;
  toNextStep: () => void;
  reset: () => void;
}>(({ authStore, toNextStep, reset }) => {
  const [code, setCode] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const { isLogin, phone } = authStore.phoneTicket;

  if (isLogin == null || phone == null) {
    reset();
    return (<></>)
  }

  async function tryVerify() {
    if (isLogin == null || phone == null) {
      reset();
      return
    }

    setIsLoading(true);

    var result = await waitWithNotification(verifyPhoneCode({
      code,
      phone,
      scope: isLogin ? "loginTicket" : "registerTicket"
    }));

    if (result.success) {
      authStore.phoneTicket.setTicket(result.data.ticket)
      toNextStep();
    }
    setIsLoading(false);
  }

  return (
    <>
      <FormItem top="Код из смс">
        <Input
          type="text"
          onInput={(e) => setCode(e.currentTarget.value)}
          value={code}
        />
      </FormItem>
      <FormItem className="flex justify-center">
        <Button type="submit" onClick={() => tryVerify()}  loading={isLoading} size="l">
          Подтвердить
        </Button>
      </FormItem>
    </>
  )
});


export const PhoneCodeForm = observer(() => {
  const { authStore } = useStore()
  const [currentStep, setCurrentStep] = useState<Steps>(Steps.Phone);
  const navigate = useNavigate();
  const location = useLocation();

  function reset() {
    authStore.phoneTicket.reset();
  }

  function afterConfirmed() {
    const { isLogin, ticket } = authStore.phoneTicket;

    if (ticket == null || isLogin == null) {
      return reset();
    }

    if (isLogin)
      navigate("/login", {state: location.state});
    else
      navigate("/register", {state: location.state});
  }

  function getRenderSection() {
    switch (currentStep) {
      case Steps.Phone:
        return (<StepPhone authStore={authStore} toNextStep={() => setCurrentStep(Steps.Code)} />)
      case Steps.Code:
        return (<StepCode authStore={authStore} toNextStep={() => afterConfirmed()} reset={() => reset()} />)
    }
  }

  return (
    <FormLayout className="w-full" onSubmit={(e) => {
      e.preventDefault();
    }}>
      {getRenderSection()}
    </FormLayout>
  )
});