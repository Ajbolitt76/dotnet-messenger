import React, { useEffect, useMemo } from "react";
import { useAuth } from "@/lib/AuthProvider";
import { LoginMode, LoginRequestDto } from "@/features/user/api/login";
import { Button, FormItem, FormLayout, Input, Link, Separator, Spacing, Spinner } from "@vkontakte/vkui";
import { addValidator } from "@/stores/Extensions/Validation";
import { Instance, types } from "mobx-state-tree";
import { isEmail, notEmpty } from "@/lib/validators";
import { observer } from "mobx-react-lite";
import { validationFormItemProps } from "@/lib/validators";
import { useNavigate } from "react-router-dom";
import { useStore } from "@/stores/RootStore";
import { NotificationTypes } from "@/stores/NotificationStore";

const loginFormWithValidation = addValidator(types.model("LoginForm", {
  userIdentifier: types.string,
  password: types.string,
}).actions(
  (self) => ({
    setUserIdentifier(email: string) {
      self.userIdentifier = email;
    },
    setPassword(password: string) {
      self.password = password;
    },
  }),
), {
  password: [
    (value) => notEmpty(value),
  ],
  userIdentifier: [
    (value) => notEmpty(value),
  ]
}, true);

type LoginForm = Instance<typeof loginFormWithValidation>;

export const LoginForm = observer(() => {
  const formState = useMemo<LoginForm>(() => loginFormWithValidation.create({ userIdentifier: "", password: "" }), []);
  const authProvider = useAuth();
  const navigate = useNavigate();
  const { notificationStore, authStore: {phoneTicket}} = useStore();

  const onLogin = async () => {
    formState.validator.validate();
    if (formState.validator.isValid) {
      try {
        await authProvider.login({
          loginMode: LoginMode.Username,
          username: formState.userIdentifier,
          password: formState.password,
        });
        navigate("/");
      }catch (e){
        notificationStore.addNotification({
          type: NotificationTypes.ERROR,
          message: "Неверный логин или пароль",
          title: "Ошибка",
        });
      }
    }
  }

  useEffect(() => {
    async function loginByTicket(){
      if(!phoneTicket.hasActiveLoginTicket)
        return;

      try {
        await authProvider.login({
          loginMode: LoginMode.Phone,
          phoneTicket: phoneTicket.ticket!
        });
        navigate("/");
      }catch (e){
        notificationStore.addNotification({
          type: NotificationTypes.ERROR,
          message: "Произошла ошибка. Обновите страницу, и попробуйте снова",
          title: "Ошибка",
        });
      }
    }

    loginByTicket();
  }, [phoneTicket.ticket])

  return phoneTicket.hasActiveLoginTicket 
  ? (<Spinner />) 
  : (
    <FormLayout className="w-full" onSubmit={(e) =>{
      e.preventDefault();
      onLogin();
    }}>
      <FormItem top="Идентификатор" {...validationFormItemProps(formState.validator.errors.userIdentifier)}>
        <Input
          type="text"
          onInput={(e) => formState.setUserIdentifier(e.currentTarget.value)}
          value={formState.userIdentifier}
        />
      </FormItem>
      <FormItem top="Пароль" {...validationFormItemProps(formState.validator.errors.password)}>
        <Input
          type="password"
          onInput={(e) => formState.setPassword(e.currentTarget.value)}
          value={formState.password}
        />
      </FormItem>
      <FormItem className="flex justify-center">
        <Button type="submit" loading={authProvider.isLoggingIn} size="l">
          Войти
        </Button>
      </FormItem>
    </FormLayout>
  )
});

LoginForm.displayName = "LoginForm";
