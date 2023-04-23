import React from "react"
import "./Splitter.css"
import clsx from "clsx"

export interface SplitterProps {
  className?: string
};

export const Splitter = ({className}: SplitterProps) => {
  return (<div className={clsx("splitter", className)}/>)
}