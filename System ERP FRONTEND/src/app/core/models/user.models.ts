export interface UserDTO {
  idUser: string;
  idRole: number;
  roleName?: string;
  idCountry: number;
  countryName?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  documentId?: string;
  isActive?: boolean;
  createdAt?: string;
}

export interface UpdateUserNameDTO {
  firstName: string;
  lastName: string;
}

export interface UpdateUserInfoDTO {
  email: string;
  phone?: string;
  documentId?: string;
  idCountry: number;
}

export interface UpdateUserRoleDTO {
  idRole: number;
}
