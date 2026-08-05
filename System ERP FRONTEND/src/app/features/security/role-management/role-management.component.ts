import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoleService } from '../../../core/services/role.service';
import { ModuleDTO, RoleWithModulesDTO } from '../../../core/models/role-permissions.models';

@Component({
  selector: 'app-role-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './role-management.component.html',
  styleUrls: ['./role-management.component.scss']
})
export class RoleManagementComponent implements OnInit {
  private readonly roleService = inject(RoleService);

  public roles: RoleWithModulesDTO[] = [];
  public filteredRoles: RoleWithModulesDTO[] = [];
  public allModules: ModuleDTO[] = [];

  public searchTerm = '';
  public isLoading = false;
  public errorMessage: string | null = null;
  public successMessage: string | null = null;

  // New Role Modal State
  public isCreateModalOpen = false;
  public newRoleName = '';
  public selectedNewModuleIds: number[] = [];
  public isCreating = false;

  // Edit Permissions Panel State
  public isPermissionsModalOpen = false;
  public selectedRoleForPermissions: RoleWithModulesDTO | null = null;
  public activeModuleIds: Set<number> = new Set<number>();
  public isSavingPermissions = false;

  ngOnInit(): void {
    this.loadData();
  }

  public loadData(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.roleService.getAllModules().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.allModules = res.value;
        }
      }
    });

    this.roleService.getRolesWithModules().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.status && res.value) {
          this.roles = res.value;
          this.filterRoles();
        } else {
          this.errorMessage = res.msg || 'No se pudieron cargar los roles.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.msg || 'Error al conectar con el servicio de roles.';
      }
    });
  }

  public filterRoles(): void {
    if (!this.searchTerm.trim()) {
      this.filteredRoles = [...this.roles];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredRoles = this.roles.filter(
      (r) => r.roleName.toLowerCase().includes(term) || String(r.idRole).includes(term)
    );
  }

  // --- Create Role Modal Handlers ---
  public openCreateRoleModal(): void {
    this.newRoleName = '';
    this.selectedNewModuleIds = [];
    this.isCreateModalOpen = true;
  }

  public closeCreateRoleModal(): void {
    this.isCreateModalOpen = false;
  }

  public toggleNewModuleSelection(moduleId: number): void {
    const index = this.selectedNewModuleIds.indexOf(moduleId);
    if (index > -1) {
      this.selectedNewModuleIds.splice(index, 1);
    } else {
      this.selectedNewModuleIds.push(moduleId);
    }
  }

  public isNewModuleSelected(moduleId: number): boolean {
    return this.selectedNewModuleIds.includes(moduleId);
  }

  public submitCreateRole(): void {
    if (!this.newRoleName.trim()) return;

    this.isCreating = true;
    this.roleService.createRole({
      roleName: this.newRoleName.trim(),
      moduleIds: this.selectedNewModuleIds
    }).subscribe({
      next: (res) => {
        this.isCreating = false;
        this.closeCreateRoleModal();
        if (res.status) {
          this.successMessage = `Rol '${this.newRoleName}' creado exitosamente.`;
          this.loadData();
        } else {
          this.errorMessage = res.msg || 'No se pudo crear el rol.';
        }
      },
      error: (err) => {
        this.isCreating = false;
        this.closeCreateRoleModal();
        this.errorMessage = err.error?.msg || 'Error al guardar el nuevo rol.';
      }
    });
  }

  // --- Edit Permissions Panel Handlers ---
  public openPermissionsModal(role: RoleWithModulesDTO): void {
    this.selectedRoleForPermissions = role;
    this.activeModuleIds = new Set<number>(role.modules.map((m) => m.idModule));
    this.isPermissionsModalOpen = true;
  }

  public closePermissionsModal(): void {
    this.isPermissionsModalOpen = false;
    this.selectedRoleForPermissions = null;
    this.activeModuleIds.clear();
  }

  public togglePermission(moduleId: number): void {
    if (this.activeModuleIds.has(moduleId)) {
      this.activeModuleIds.delete(moduleId);
    } else {
      this.activeModuleIds.add(moduleId);
    }
  }

  public isModuleActiveForRole(moduleId: number): boolean {
    return this.activeModuleIds.has(moduleId);
  }

  public saveRolePermissions(): void {
    if (!this.selectedRoleForPermissions) return;

    this.isSavingPermissions = true;
    const dto = {
      idRole: this.selectedRoleForPermissions.idRole,
      moduleIds: Array.from(this.activeModuleIds)
    };

    this.roleService.updateRolePermissions(dto).subscribe({
      next: (res) => {
        this.isSavingPermissions = false;
        const roleName = this.selectedRoleForPermissions?.roleName;
        this.closePermissionsModal();
        if (res.status) {
          this.successMessage = `Permisos para el rol '${roleName}' actualizados correctamente.`;
          this.loadData();
        } else {
          this.errorMessage = res.msg || 'No se pudieron actualizar los permisos.';
        }
      },
      error: (err) => {
        this.isSavingPermissions = false;
        this.closePermissionsModal();
        this.errorMessage = err.error?.msg || 'Error al guardar los cambios de permisos.';
      }
    });
  }

  // --- Delete Role ---
  public deleteRole(role: RoleWithModulesDTO): void {
    if (!confirm(`¿Está seguro de eliminar el rol '${role.roleName}'?`)) return;

    this.roleService.deleteRole(role.idRole).subscribe({
      next: (res) => {
        if (res.status) {
          this.successMessage = `Rol '${role.roleName}' eliminado correctamente.`;
          this.loadData();
        } else {
          this.errorMessage = res.msg || 'No se pudo eliminar el rol.';
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.msg || 'Error al intentar eliminar el rol.';
      }
    });
  }
}
