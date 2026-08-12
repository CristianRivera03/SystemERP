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
        path: 'dashboard',
        loadComponent: () => import('./features/catalogs/catalogs-dashboard/catalogs-dashboard.component').then((m) => m.CatalogsDashboardComponent)
      },
      {
        path: 'gestion-usuarios',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
      },
      {
        path: 'security/users',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
      },
      {
        path: 'perfiles-empleados',
        loadComponent: () => import('./features/security/user-management/user-management.component').then((m) => m.UserManagementComponent)
      },
      {
        path: 'roles-permisos',
        loadComponent: () => import('./features/security/role-management/role-management.component').then((m) => m.RoleManagementComponent)
      },
      {
        path: 'security/roles',
        loadComponent: () => import('./features/security/role-management/role-management.component').then((m) => m.RoleManagementComponent)
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
        path: 'productos',
        loadComponent: () => import('./features/products/products.component').then((m) => m.ProductsComponent)
      },
      {
        path: 'proveedores',
        loadComponent: () => import('./features/suppliers/suppliers.component').then((m) => m.SuppliersComponent)
      },
      {
        path: 'clientes',
        loadComponent: () => import('./features/customers/customers.component').then((m) => m.CustomersComponent)
      },

      // Dedicated Under-Construction Module Components
      {
        path: 'ventas',
        loadComponent: () => import('./features/sales/sales.component').then((m) => m.SalesComponent)
      },
      {
        path: 'cotizaciones-ventas',
        loadComponent: () => import('./features/sales/sales-quotes.component').then((m) => m.SalesQuotesComponent)
      },
      {
        path: 'compras',
        loadComponent: () => import('./features/purchases/purchases.component').then((m) => m.PurchasesComponent)
      },
      {
        path: 'cotizaciones-compra',
        loadComponent: () => import('./features/purchases/purchase-quotes.component').then((m) => m.PurchaseQuotesComponent)
      },
      {
        path: 'ordenes-compra',
        loadComponent: () => import('./features/purchases/purchase-orders.component').then((m) => m.PurchaseOrdersComponent)
      },
      {
        path: 'retaceo',
        loadComponent: () => import('./features/purchases/landed-cost.component').then((m) => m.LandedCostComponent)
      },
      {
        path: 'asignacion-precios',
        loadComponent: () => import('./features/inventory/price-assignment.component').then((m) => m.PriceAssignmentComponent)
      },
      {
        path: 'traslados',
        loadComponent: () => import('./features/inventory/stock-transfers.component').then((m) => m.StockTransfersComponent)
      },
      {
        path: 'kardex',
        loadComponent: () => import('./features/inventory/kardex.component').then((m) => m.KardexComponent)
      },
      {
        path: 'devoluciones',
        loadComponent: () => import('./features/returns/returns.component').then((m) => m.ReturnsComponent)
      },
      {
        path: 'flota-conductores',
        loadComponent: () => import('./features/logistics/fleet.component').then((m) => m.FleetComponent)
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
