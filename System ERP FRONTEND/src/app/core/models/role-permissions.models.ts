export interface ModuleDTO {
  idModule: number;
  name: string;
  frontendPath: string;
  icon?: string;
  isActive?: boolean;
}

export interface RoleWithModulesDTO {
  idRole: number;
  roleName: string;
  modules: ModuleDTO[];
}

export interface UpdateRolePermissionsDTO {
  idRole: number;
  moduleIds: number[];
}

export interface CreateRoleRequest {
  roleName: string;
  moduleIds?: number[];
}
