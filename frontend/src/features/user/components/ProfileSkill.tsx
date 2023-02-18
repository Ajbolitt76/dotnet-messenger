import React from "react";
import { Knob } from "primereact/knob";
import { UserSkillDto } from "@/features/user/types";
import "./ProfileSkill.pcss";

interface ProfileSkillProps {
  skill: UserSkillDto;
}

export const ProfileSkill: React.FC<ProfileSkillProps> = ({ skill }) => {
  return (
    <div className="profile-skill">
      <Knob value={skill.value}
            min={0}
            max={100}
            readOnly={true}
            valueTemplate={skill.textInCircle || "{value}%"}/>
      <div>
        <p>{skill.textValue}</p>
        <p>{skill.skill.name}</p>
      </div>
    </div>);
}
