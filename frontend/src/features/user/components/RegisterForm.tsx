import { addValidator } from "@/stores/Extensions/Validation";
import { isEmail, validationFormItemProps } from "@/lib/validators";
import { Instance, types } from "mobx-state-tree";
import { useAuth } from "@/lib/AuthProvider";
import { useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import React, { useMemo } from "react";
import { Button, FormItem, FormLayout, Input, Link, Separator } from "@vkontakte/vkui";
import { NotificationTypes } from "@/stores/NotificationStore";
import { useStore } from "@/stores/RootStore";

const registerFormWithValidation = addValidator(types.model("RegisterForm", {
  email: types.string,
  username: types.string,
  password: types.string,
  name: types.string,
  surname: types.string,
}).actions(
  (self) => ({
    setEmail(email: string) {
      self.email = email;
    },
    setPassword(password: string) {
      self.password = password;
    },
    setUsername(username: string) {
      self.username = username;
    },
    setName(name: string) {
      self.name = name;
    },
    setSurname(surname: string) {
      self.surname = surname;
    }
  }),
), {
  password: [
    (value) => value.length > 5 && /^(?=(.*[a-z]){3,})(?=(.*[A-Z]){1,})(?=(.*[0-9]){2,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{3,}$/.test(value) ? null : "Слабый пароль",
  ],
  email: [(value) => isEmail(value)],
  username: [(value) => value.length > 0 ? null : "Имя пользователя не может быть пустым"],
  name: [(value) => value.length > 0 ? null : "Имя не может быть пустым"],
  surname: [(value) => value.length > 0 ? null : "Фамилия не может быть пустой"],
}, true);

type RegisterForm = Instance<typeof registerFormWithValidation>;

export const RegisterForm = observer(() => {
  const formState = useMemo<RegisterForm>(() => registerFormWithValidation.create({
    password: "",
    email: "",
    surname: "",
    name: "",
    username: "",
  }), []);

  const authProvider = useAuth();
  const navigate = useNavigate();
  const { notificationStore } = useStore();

  const onLogin = () => {
    navigate("../login");
  }

  const onRegister = async () => {
    formState.validator.validate();
    if (formState.validator.isValid) {
      try {
        await authProvider.register({
          email: formState.email,
          name: formState.name,
          password: formState.password,
          surname: formState.surname,
          username: formState.username,
        })
        navigate("/");
      } catch (e) {
        console.log(e);
        notificationStore.addNotification({
          type: NotificationTypes.ERROR,
          message: "Произошла во время регистрации",
          title: "Ошибка",
        });
      }
    }
  }

  return (
    <FormLayout className="w-full">
      <FormItem top="Email" {...validationFormItemProps(formState.validator.errors.email)}>
        <Input
          type="email"
          onInput={(e) => formState.setEmail(e.currentTarget.value)}
          value={formState.email}
        />
      </FormItem>
      <FormItem top="Логин" {...validationFormItemProps(formState.validator.errors.username)}>
        <Input
          type="text"
          onInput={(e) => formState.setUsername(e.currentTarget.value)}
          value={formState.username}
        />
      </FormItem>
      <FormItem top="Пароль" {...validationFormItemProps(formState.validator.errors.password)}>
        <Input
          type="password"
          onInput={(e) => formState.setPassword(e.currentTarget.value)}
          value={formState.password}
        />
      </FormItem>
      <FormItem top="Имя" {...validationFormItemProps(formState.validator.errors.name)}>
        <Input
          type="text"
          onInput={(e) => formState.setName(e.currentTarget.value)}
          value={formState.name}
        />
      </FormItem>
      <FormItem top="Фамилия" {...validationFormItemProps(formState.validator.errors.surname)}>
        <Input
          type="text"
          onInput={(e) => formState.setSurname(e.currentTarget.value)}
          value={formState.surname}
        />
      </FormItem>
      <FormItem className="flex justify-center">
        <Button loading={authProvider.isRegistering}  size="l" onClick={() => onRegister()}>
          Зарегистрироваться
        </Button>
      </FormItem>
      <Separator />
      <FormItem className="flex justify-center">
        <Link disabled={authProvider.isRegistering} onClick={() => onLogin()}>
          Уже зарегистрированы? Войти
        </Link>
      </FormItem>
    </FormLayout>
  )
});

RegisterForm.displayName = "LoginForm";
