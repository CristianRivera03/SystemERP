import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  badge?: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  public navItems: NavItem[] = [
    {
      label: 'Catálogos del Sistema',
      icon: 'bx-folder-open',
      route: '/catalogs'
    },
    {
      label: 'Roles y Permisos',
      icon: 'bx-shield-quarter',
      route: '/security/roles'
    },
    {
      label: 'Gestión de Usuarios',
      icon: 'bx-user-pin',
      route: '/security/users'
    },
    {
      label: 'Inventario (Próximo)',
      icon: 'bx-package',
      route: '/inventory',
      badge: 'Pronto'
    },
    {
      label: 'Ventas y Facturación',
      icon: 'bx-cart',
      route: '/sales',
      badge: 'Pronto'
    }
  ];
}
