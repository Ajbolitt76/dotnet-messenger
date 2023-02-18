import { StatusDto, SystemFileDto, TagDto } from "@/lib/ApiTypes";
import { TeamByIdDto, TeamListDto } from "@/features/teams/types";

export type ProjectMilestoneDto = {
  name: string;
  description: string;
  achievedAt: Date | null;
  eta: string | null;
}

export type ProjectLinkDto = {
  name: string;
  link: string;
}

export type ProjectListDto = {
  id: number;
  photo: SystemFileDto;
  name: string;
  shortName: string;
  description: string;
  state: StatusDto;
  team: TeamListDto;
  tags: TagDto[];
}

export type ProjectDto = {
  id: number;
  photo: SystemFileDto;
  name: string;
  shortName: string;
  description: string;
  state: StatusDto;
  team: TeamByIdDto;
  carouselPhotos: SystemFileDto[];
  tags: TagDto[];
  milestones: ProjectMilestoneDto[];
}
