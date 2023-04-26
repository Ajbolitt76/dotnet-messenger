import { apiClient } from "@/lib/ApiClient";

export type InitiatePhoneAuthRequestDto = {
  phone: string;
}

export type InitiatePhoneAuthResponseDto = {
  nextTry: string;
  codeSent: boolean;
  isLogin: boolean;
}

export function initiatePhoneAuth(request: InitiatePhoneAuthRequestDto) {
  return apiClient.post('auth/initiate-phone-auth', { json: request }).json<InitiatePhoneAuthResponseDto>();
}
