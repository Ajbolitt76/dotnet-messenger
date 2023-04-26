import { IAnyComplexType, IAnyStateTreeNode, IStateTreeNode, Instance, getIdentifier, getRoot, getType, isStateTreeNode } from "mobx-state-tree";
import { RootModelType } from "./RootStore";

export function getInstanceId<T>(instance: T) {
  if (isStateTreeNode(instance)) 
    return getIdentifier(instance);
}

export function IsInstanceOf<T extends IAnyComplexType>(obj: IAnyStateTreeNode, type: T): obj is Instance<T> {
  return getType(obj) == type;
}

export function getTgRoot(node: IStateTreeNode) {
  return getRoot<RootModelType>(node);
} 
