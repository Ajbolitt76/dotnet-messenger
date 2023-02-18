import React from "react";

export const usePrevious = <T>(newValue: T) => {
  const ref = React.useRef<T | undefined>(undefined);

  React.useEffect(() => {
    ref.current = newValue;
  });

  return ref.current;
};
