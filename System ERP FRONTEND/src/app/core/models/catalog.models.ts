export interface CatalogDTO<TId = number> {
  id: TId;
  name: string;
}

export type CatalogType =
  | 'Categories'
  | 'ProductTypes'
  | 'UnitMeasures'
  | 'Presentations'
  | 'Roles'
  | 'Countries'
  | 'Departments'
  | 'Municipalities'
  | 'Districts';
