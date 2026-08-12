export interface CompanyDTO {
  idCompany: string;
  businessName: string;
  tradeName?: string;
  taxId: string;
  nrc?: string;
  commercialLine1: string;
  commercialLine2?: string;
  commercialLine3?: string;
  email?: string;
  phone?: string;
  districtId: string;
  districtName?: string;
  addressComplement?: string;
  logoUrl?: string;
  isActive?: boolean;
  createdAt?: string;
}

export interface BranchDTO {
  idBranch: string;
  idCompany: string;
  companyName?: string;
  name: string;
  districtId: string;
  districtName?: string;
  municipalityId?: string;
  municipalityName?: string;
  departmentId?: string;
  departmentName?: string;
  addressComplement?: string;
  phone?: string;
  email?: string;
  isActive?: boolean;
  createdAt?: string;
}

export interface WarehouseCategoryDTO {
  idWarehouseCategory: number;
  name: string;
  description?: string;
}

export interface WarehouseDTO {
  idWarehouse: string;
  idBranch: string;
  branchName?: string;
  idWarehouseCategory: number;
  categoryName?: string;
  name: string;
  description?: string;
  isActive?: boolean;
}

export interface LocationDTO {
  idLocation: string;
  idWarehouse: string;
  warehouseName?: string;
  aisle?: string;
  rack?: string;
  level?: string;
  position?: string;
  code?: string;
  capacity?: number;
  notes?: string;
  isActive?: boolean;
}

export interface InventoryStockDTO {
  idStock: string;
  idProduct: string;
  productName?: string;
  productCode?: string;
  idLocation: string;
  locationCode?: string;
  warehouseName?: string;
  quantity: number;
  lastUpdated?: string;
}
