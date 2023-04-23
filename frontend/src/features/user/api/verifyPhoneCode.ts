import { apiClient } from "@/lib/ApiClient";

export type InitiatePhoneAuthRequestDto = {
  phone: string;
  scope: "registerTicket" | "loginTicket"
  code: string;
}

export type InitiatePhoneAuthResponseDto = {
  ticket: string;
}

export function verifyPhoneCode(request: InitiatePhoneAuthRequestDto) {
  return apiClient.post('auth/verify-phone-ticket', { json: request }).json<InitiatePhoneAuthResponseDto>();
}
