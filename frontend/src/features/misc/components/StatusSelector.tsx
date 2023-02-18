import React, { useMemo } from "react";
import { CustomSelect, CustomSelectOption } from "@vkontakte/vkui";
import { KnownEntity, useStatusesForEntity } from "@/features/misc/api/getStatusesForEntity";
import { StatusDisplay } from "@/components/UI/StatusDisplay";

export type StatusSelectProps = {
  type: KnownEntity;
  value: number;
  onChange: (statusId: number) => void;
}


export const StatusSelector: React.FC<StatusSelectProps> = ({ type, value, onChange }) => {
  const { data: statuses, isLoading } = useStatusesForEntity({ type });

  const options = useMemo(() => {
    return statuses?.map((status) => ({
      value: status.id,
      label: status.name,
      status: status,
    })) ?? [];
  }, [statuses]);

  return (
    <CustomSelect
      options={options}
      value={value}
      onChange={(e) => onChange(Number(e.target.value))}
      placeholder="Выберите статус"
      fetching={isLoading}
      renderOption={({ option, ...restProps }) => (
        <CustomSelectOption {...restProps} >
          <StatusDisplay status={option.status} />
        </CustomSelectOption>
      )}
    />
  );
}
