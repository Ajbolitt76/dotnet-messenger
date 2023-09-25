import { unstable_Popover as Popover } from '@vkontakte/vkui';
import React from 'react';

export type ContextTargetContent = React.ComponentType<{
  getRootRef?: React.Ref<HTMLDivElement>;
  onClick: React.MouseEventHandler<unknown> | undefined;
  onContextMenu?: React.MouseEventHandler<unknown> | undefined;
}>

export type WithContextMenuProps = {
  target: ContextTargetContent;
  content: (setShown: (x: boolean) => void) => React.ReactNode;
  onClick: () => void;
  onContextMenu?: () => boolean;
  targetRef?: React.RefCallback<HTMLDivElement>;
}

export const WithContextMenu: React.FC<WithContextMenuProps> = (props) => {
  const Target = props.target;

  const targetRef = React.useRef<HTMLDivElement>(null);
  const [shown, setShown] = React.useState(false);
  const [offset, setOffset] = React.useState(0);

  if(targetRef.current){
    props.targetRef?.(targetRef.current);
  }
  return (
    <Popover
      content={props.content?.((x) => setShown(x))}
      offsetSkidding={offset}
      onShownChange={setShown}
      shown={shown}
    >
      <Target 
        getRootRef={targetRef}
        onClick={() => {
          setShown(false);
          props.onClick?.();
        }}
        onContextMenu={(e) => {
          e.preventDefault();
          const baseOffset = targetRef.current?.getBoundingClientRect();
          
          setOffset(e.clientX - (baseOffset?.x ?? 0));

          if(props.onContextMenu?.()) {
            setShown((x) => !x);
          }
        }}
      />
    </Popover>
  )
}