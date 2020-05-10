export interface UserData {
  userId: number;
  token: string;
  expireDate: Date;
  role: string;
  isExpired: boolean;
  email: string;
}
