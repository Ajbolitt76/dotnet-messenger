export type SkillDto = {
  id: number;
  name: string;
}

export type StatusDto = {
  id: number;
  name: string;
  color: string;
}

export type TagDto = {
  id: number;
  name: string;
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
