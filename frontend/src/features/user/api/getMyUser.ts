import { apiClient } from "@/lib/ApiClient";
import { UserProfileDto } from "@/features/user/types";

export function getMyUser(){
  return apiClient.get('me').json<UserProfileDto>();
}
