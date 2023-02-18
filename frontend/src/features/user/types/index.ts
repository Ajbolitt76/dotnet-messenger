import { SkillDto, StatusDto, SystemFileDto } from "@/lib/ApiTypes";

export type UserContactDto = {
  name: string;
  value: string;
}

export type UserSkillDto = {
  value: number;
  skill: SkillDto;
  textValue: string;
  textInCircle: string;
}

export type OtherUserProfileDto = {
  id: number;
  name: string;
  surname: string;
  lastActivity: string;
  about: string;
  instituteName?: string;
  instituteEducationProgram?: string;
  instituteCourse?: string;
  contacts?: UserContactDto[];
  skills?: UserSkillDto[];
  photo?: SystemFileDto;
  status: StatusDto;
}

export interface UserProfileDto extends OtherUserProfileDto {
  role: number;
  email: string;
}

export type UserListDto = {
  id: number;
  name: string;
  surname: string;
  lastActivity: Date;
  about?: string;
  instituteName?: string;
  instituteEducationProgram?: string;
  instituteCourse?: string;
  photo?: SystemFileDto;
}
