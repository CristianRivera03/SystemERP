export interface LoginDTO {
  email: string;
  password: string;
}

export interface RegisterDTO {
  idRole: number;
  idCountry: number;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone?: string;
  documentId?: string;
}

export interface SessionDTO {
  idUser: string;
  firstName: string;
  lastName: string;
  email: string;
  idRole: number;
  roleName?: string;
  idCountry: number;
  countryName?: string;
  token: string;
}
