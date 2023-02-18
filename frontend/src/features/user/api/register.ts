import { apiClient } from "@/lib/ApiClient";

export type RegisterRequestDto = {
  email: string;
  username: string;
  name: string;
  surname: string;
  password: string;
}

export type RegisterResponseDto = {
  token: string;
}

export function register(request: RegisterRequestDto) {
  return apiClient.post('register', { json: request }).json<RegisterResponseDto>();
}
