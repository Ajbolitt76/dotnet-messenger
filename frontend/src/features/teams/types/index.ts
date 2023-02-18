import { SystemFileDto } from "@/lib/ApiTypes";
import { UserListDto } from "@/features/user/types";

export type TeamMemberDto = {
  isAdmin: boolean;
  isCreator: boolean;
  displayRoleName: string;
  user: UserListDto | null;
  team?: TeamListDto;
}


export type TeamListDto = {
  id: number;
  name: string;
  photo: SystemFileDto | null;
}


export type TeamByIdDto = TeamListDto &{
  teamMembers: TeamMemberDto[] | null;
}
