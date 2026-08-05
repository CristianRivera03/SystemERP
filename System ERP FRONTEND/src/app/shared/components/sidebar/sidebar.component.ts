import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ModuleDTO } from '../../../core/models/auth.models';

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
  private readonly authService = inject(AuthService);

  // Dynamic Navigation Items computed from Logged-in User's Assigned Modules
  public navItems = computed<NavItem[]>(() => {
    const user = this.authService.currentUser();
    const modules = user?.modules;

    if (!modules) {
      return [];
    }

    return modules
      .filter(m => m.isActive !== false)
      .map(m => this.mapModuleToNavItem(m));
  });

  private mapModuleToNavItem(module: ModuleDTO): NavItem {
    // Resolve route from DB frontend_path
    let route = module.frontendPath || '/catalogs';
    if (route === '/gestion-usuarios') route = '/security/users';
    if (route === '/roles-permisos') route = '/security/roles';
    if (route === '/bitacora') route = '/bitacora';
    if (route === '/dashboard') route = '/catalogs';

    let rawIcon = (module.icon || 'folder').trim().toLowerCase();

    // Map Feather / Lucide / Generic DB icon names to valid Boxicons classes
    const iconMap: Record<string, string> = {
      'home': 'bx-home-alt',
      'users': 'bx-group',
      'user': 'bx-user-pin',
      'user-check': 'bx-user-check',
      'truck': 'bxs-truck',
      'business': 'bx-briefcase',
      'proveedores': 'bx-briefcase',
      'box': 'bx-package',
      'shopping-cart': 'bx-cart',
      'shopping-bag': 'bx-shopping-bag',
      'clipboard': 'bx-receipt',
      'list': 'bx-list-ul',
      'file-text': 'bx-file-find',
      'file': 'bx-file',
      'layers': 'bx-layer',
      'tag': 'bx-purchase-tag-alt',
      'database': 'bx-data',
      'map-pin': 'bx-map-pin',
      'move': 'bx-transfer',
      'file-plus': 'bx-file-blank',
      'corner-down-left': 'bx-undo',
      'navigation': 'bx-compass',
      'shield': 'bx-shield-quarter',
      'kardex': 'bx-spreadsheet',
      'almacenes': 'bx-store',
      'sucursales': 'bx-buildings',
      'traslados': 'bx-transfer-alt',
      'devoluciones': 'bx-undo',
      'bitacora': 'bx-history',
      'retaceo': 'bx-git-repo-forked',
      'flota-conductores': 'bx-car'
    };

    let iconClass = '';
    if (rawIcon.startsWith('bx ') || rawIcon.startsWith('bxs ') || rawIcon.startsWith('fa ')) {
      iconClass = rawIcon;
    } else if (rawIcon.startsWith('bx-') || rawIcon.startsWith('bxs-') || rawIcon.startsWith('bxl-')) {
      iconClass = `bx ${rawIcon}`;
    } else if (iconMap[rawIcon]) {
      const mapped = iconMap[rawIcon];
      iconClass = mapped.startsWith('bx') ? `bx ${mapped}` : `bx bx-${mapped}`;
    } else {
      iconClass = `bx bx-${rawIcon}`;
    }

    return {
      label: module.name,
      icon: iconClass,
      route: route
    };
  }
}
