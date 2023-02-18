import React, { CSSProperties } from "react";
import { StatusDto } from "@/lib/ApiTypes";
import clsx from "clsx";
import "./StatusDisplay.pcss";

const knownBgClasses: Record<string, string> = {
  'bg-gradient-green': 'bg-gradient-green',
  'bg-gradient-yellow': 'bg-gradient-yellow',
  'bg-gradient-red': 'bg-gradient-red',
}

interface StatusDisplayProps {
  status: StatusDto;
}

export const StatusDisplay: React.FC<StatusDisplayProps> = ({ status }) => {
  const background = knownBgClasses[status.color];
  const styles: CSSProperties = status.color.startsWith('#') ? { backgroundColor : status.color } : {};

  return (
    <span className="a-status">
        <span className={clsx(['a-status__marker', background])} style={styles} ></span>
        {status.name}
    </span>
  )
}
