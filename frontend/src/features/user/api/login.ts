import { apiClient } from "@/lib/ApiClient";

export enum LoginMode {
  Phone = "phone",
  Username = "username"
}

type BaseRequest = {
  loginMode: LoginMode,
}

export type UsernameLoginRequestDto = BaseRequest & {
  loginMode: LoginMode.Phone
  phoneTicket: string;
}

export type PasswordLoginRequestDto = BaseRequest & {
  loginMode: LoginMode.Username
  username: string;
  password: string;
}

export type LoginRequestDto = UsernameLoginRequestDto | PasswordLoginRequestDto;

export type LoginResponseDto = {
  token: string;
  refreshToken: string;
}

export function login(request: LoginRequestDto) {
  return apiClient.post('auth/login', { json: request }).json<LoginResponseDto>();
}
