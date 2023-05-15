import { useCallback, useEffect, useRef } from "react";

const hideDelay = 1000;
const scrollbarWidth = 20;

export function useActivatableScrollbar() {
  const sbRef = useRef<HTMLDivElement>(null);
  const scrollHideTimer = useRef<number>(0);
  const hoverHideTimer = useRef<number>(0);
  const isScrolling = useRef(false);
  const isHovering = useRef(false);

  const scrollHandler = useCallback(() => {
    const el = sbRef.current;
    if(!el)
      return;  
    
    if(!isScrolling.current){
      el.classList.add("c-scrolling");
    }
    clearTimeout(scrollHideTimer.current);

    isScrolling.current = true;

    scrollHideTimer.current = setTimeout(() => {
      el.classList.remove("c-scrolling");
      isScrolling.current = false;
    }, hideDelay) as unknown as number;
  
  }, []);

  const mouseHandler = useCallback((e: MouseEvent) => {
    const el = sbRef.current;
    if(!el)
      return;

    if((el.offsetWidth - e.offsetX > scrollbarWidth && isHovering.current) || e.type == "mouseleave"){
      isHovering.current = false;
      hoverHideTimer.current = setTimeout(() => {
        el.classList.remove("c-scroll-hover");
        isHovering.current = false;
      }, hideDelay) as unknown as number;
    } else if(el.offsetWidth - e.offsetX < scrollbarWidth) {
      if(!isHovering.current){
        el.classList.add("c-scroll-hover");
      }
  
      clearTimeout(hoverHideTimer.current);
      isHovering.current = true;
    }
  }, []);

  useEffect(() => {
    const el = sbRef.current;
    el?.classList.add("c-scrollbar")
    el?.addEventListener("scroll", scrollHandler);
    el?.addEventListener("mousemove", mouseHandler);
    el?.addEventListener("mouseleave", mouseHandler);
    
    return () => {
      el?.removeEventListener("scroll", scrollHandler);
      el?.removeEventListener("mousemove", mouseHandler);
      el?.removeEventListener("mouseleave", mouseHandler);
    }
  }, [scrollHandler]);

  return sbRef;
}