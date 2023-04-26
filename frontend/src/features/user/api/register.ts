import { apiClient } from "@/lib/ApiClient";

export type RegisterRequestDto = {
  name: string;
  phoneTicket: string;
  username: string;
  password: string;
}

export type RegisterResponseDto = {
  token: string;
  refreshToken: string;
}

export function register(request: RegisterRequestDto) {
  return apiClient.post('auth/registration', { json: request }).json<RegisterResponseDto>();
}
