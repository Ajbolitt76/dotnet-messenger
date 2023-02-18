import React from "react";
import { useSkills } from "@/features/skills/api/getSkills";
import { useMemo } from "react";
import { CustomSelect } from "@vkontakte/vkui";

export type SkillSelectorProps = {
  value: number;
  onChange: (skillId: number) => void;
}

export const SkillSelector: React.FC<SkillSelectorProps> = ({ value, onChange }) => {
  const { data: skills, isLoading } = useSkills();

  const options = useMemo(() => {
    return skills?.map((skill) => ({
      value: skill.id,
      label: skill.name,
    })) ?? [];
  }, [skills]);

  return (
    <CustomSelect
      searchable={true}
      placeholder="Выберите навык"
      fetching={isLoading}
      options={options}
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
    />
  );
}
