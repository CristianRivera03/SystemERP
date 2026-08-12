import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then((m) => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then((m) => m.RegisterComponent)
  },
  {
    path: '',
    loadComponent: () => import('./features/dashboard/dashboard-layout/dashboard-layout.component').then((m) => m.DashboardLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: 'catalogs',
        loadComponent: () => import('./features/catalogs/catalogs-dashboard/catalogs-dashboard.component').then((m) => m.CatalogsDashboardComponent)
      },
      {
        path: 'security/roles',
        loadComponent: () => import('./features/security/role-management/role-management.component').then((m) => m.RoleManagementComponent)
      },
      {
        path: 'roles-permisos',
        loadComponent: () => import('./features/security/role-management/role-management.component').then((m) => m.RoleManagementComponent)
      },
      {
        path: 'security/users',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
      },
      {
        path: 'gestion-usuarios',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
      },
      {
        path: 'bitacora',
        loadComponent: () => import('./features/security/bitacora/bitacora.component').then((m) => m.BitacoraComponent)
      },
      {
        path: 'security/audit-log',
        loadComponent: () => import('./features/security/bitacora/bitacora.component').then((m) => m.BitacoraComponent)
      },
      {
        path: 'sucursales',
        loadComponent: () => import('./features/inventory/branches/branches.component').then((m) => m.BranchesComponent)
      },
      {
        path: 'almacenes',
        loadComponent: () => import('./features/inventory/warehouses/warehouses.component').then((m) => m.WarehousesComponent)
      },
      {
        path: 'inventario',
        loadComponent: () => import('./features/inventory/inventory-dashboard/inventory-dashboard.component').then((m) => m.InventoryDashboardComponent)
      },
      {
        path: 'dashboard',
        loadComponent: () => import('./features/catalogs/catalogs-dashboard/catalogs-dashboard.component').then((m) => m.CatalogsDashboardComponent)
      },
      {
        path: '',
        redirectTo: 'catalogs',
        pathMatch: 'full'
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'catalogs'
  }
];
