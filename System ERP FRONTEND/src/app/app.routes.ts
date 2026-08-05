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
        path: 'security/users',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
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
