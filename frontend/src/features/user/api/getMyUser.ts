import { apiClient } from "@/lib/ApiClient";
import { MeUserInfo } from "@/features/user/types";

export function getMyUser(){
  return apiClient.get('user/me').json<MeUserInfo>();
}
