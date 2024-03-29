export interface ApiError {
  title: string;
  status: number;
  placeholder: Record<string, string>
  traceId: string;
}

export type SystemFileDto = {
  id: number;
  path: string;
}

export type BaseError = {
  statusCode: number;
  errorCode?: string;
  message?: string;
}

export const KnownUserContacts = {
  email: 'Email',
  phone: 'Телефон',
  telegram: 'Telegram',
}

export type UserContactType = keyof typeof KnownUserContacts;
