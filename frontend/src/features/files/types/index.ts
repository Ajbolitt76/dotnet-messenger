import { FieldWithPossiblyUndefined } from "lodash";

export interface FileInfo {
    fileId: string;
    type: string;
}

export interface FileOwnershipSignedData {
    data: FileInfo;
    signature: string;
}