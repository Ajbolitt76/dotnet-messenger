import { apiClient } from "@/lib/ApiClient";

export type LoginRequestDto = {
  userIdentifier: string;
  password: string;
}

export type LoginResponseDto = {
  token: string;
}

export function login(request: LoginRequestDto) {
  return apiClient.post('login', { json: request }).json<LoginResponseDto>();
}
