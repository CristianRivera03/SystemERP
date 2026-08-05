import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';
import { UserService } from '../../../core/services/user.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { UserDTO } from '../../../core/models/user.models';
import { CatalogDTO } from '../../../core/models/catalog.models';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  private readonly userService = inject(UserService);
  private readonly catalogService = inject(CatalogService);
  private readonly fb = inject(FormBuilder);

  public users: UserDTO[] = [];
  public filteredUsers: UserDTO[] = [];
  public roles: CatalogDTO[] = [];
  public countries: CatalogDTO[] = [];

  public searchTerm = '';
  public isLoading = false;
  public errorMessage: string | null = null;
  public successMessage: string | null = null;

  // Selected User for Modals
  public selectedUser: UserDTO | null = null;

  // Modal States
  public isRegisterModalOpen = false;
  public isEditNameModalOpen = false;
  public isEditInfoModalOpen = false;
  public isEditRoleModalOpen = false;
  public isSubmitting = false;
  public showRegisterPassword = false;

  public toggleShowRegisterPassword(): void {
    this.showRegisterPassword = !this.showRegisterPassword;
  }

  public allowOnlyNumbers(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input) {
      const sanitized = input.value.replace(/[^0-9+]/g, '');
      if (input.value !== sanitized) {
        input.value = sanitized;
      }
    }
  }

  // Forms
  public registerForm!: FormGroup;
  public editNameForm!: FormGroup;
  public editInfoForm!: FormGroup;
  public editRoleForm!: FormGroup;

  ngOnInit(): void {
    this.initForms();
    this.loadCatalogs();
    this.loadUsers();
  }

  private initForms(): void {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      idRole: ['', [Validators.required]],
      idCountry: ['', [Validators.required]],
      phone: [''],
      documentId: ['']
    });

    this.editNameForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]]
    });

    this.editInfoForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      phone: [''],
      documentId: [''],
      idCountry: ['', [Validators.required]]
    });

    this.editRoleForm = this.fb.group({
      idRole: ['', [Validators.required]]
    });
  }

  public loadCatalogs(): void {
    this.catalogService.getRoles().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.roles = res.value;
        }
      }
    });

    this.catalogService.getCountries().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.countries = res.value;
        }
      }
    });
  }

  public loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.userService.getUsers().subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.status && res.value) {
          this.users = res.value;
          this.filterUsers();
        } else {
          this.errorMessage = res.msg || 'No se pudieron cargar los usuarios.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.msg || 'Error de conexión al cargar usuarios.';
      }
    });
  }

  public filterUsers(): void {
    if (!this.searchTerm.trim()) {
      this.filteredUsers = [...this.users];
      return;
    }

    const term = this.searchTerm.toLowerCase();
    this.filteredUsers = this.users.filter(u =>
      u.firstName.toLowerCase().includes(term) ||
      u.lastName.toLowerCase().includes(term) ||
      u.email.toLowerCase().includes(term) ||
      (u.roleName && u.roleName.toLowerCase().includes(term)) ||
      (u.countryName && u.countryName.toLowerCase().includes(term)) ||
      (u.documentId && u.documentId.toLowerCase().includes(term))
    );
  }

  // --- REGISTRAR USUARIO ---
  public openRegisterModal(): void {
    this.registerForm.reset({
      firstName: '',
      lastName: '',
      email: '',
      password: '',
      idRole: '',
      idCountry: '',
      phone: '',
      documentId: ''
    });
    this.errorMessage = null;
    this.successMessage = null;
    this.isRegisterModalOpen = true;
  }

  public closeRegisterModal(): void {
    this.isRegisterModalOpen = false;
  }

  public submitRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    const val = this.registerForm.value;
    const dto = {
      ...val,
      idRole: Number(val.idRole),
      idCountry: Number(val.idCountry)
    };

    this.userService.registerUser(dto).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.status) {
          this.showSuccess('Usuario registrado con éxito.');
          this.closeRegisterModal();
          this.loadUsers();
        } else {
          this.errorMessage = res.msg || 'No se pudo registrar el usuario.';
        }
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.msg || 'Error al registrar usuario.';
      }
    });
  }

  // --- MODIFICAR NOMBRE ---
  public openEditNameModal(user: UserDTO): void {
    this.selectedUser = user;
    this.editNameForm.patchValue({
      firstName: user.firstName,
      lastName: user.lastName
    });
    this.errorMessage = null;
    this.isEditNameModalOpen = true;
  }

  public closeEditNameModal(): void {
    this.isEditNameModalOpen = false;
    this.selectedUser = null;
  }

  public submitEditName(): void {
    if (!this.selectedUser || this.editNameForm.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = null;

    this.userService.updateName(this.selectedUser.idUser, this.editNameForm.value).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.status) {
          this.showSuccess('Nombre del usuario actualizado correctamente.');
          this.closeEditNameModal();
          this.loadUsers();
        } else {
          this.errorMessage = res.msg || 'No se pudo actualizar el nombre.';
        }
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.msg || 'Error al actualizar el nombre.';
      }
    });
  }

  // --- MODIFICAR INFORMACION ---
  public openEditInfoModal(user: UserDTO): void {
    this.selectedUser = user;
    this.editInfoForm.patchValue({
      email: user.email,
      phone: user.phone || '',
      documentId: user.documentId || '',
      idCountry: user.idCountry
    });
    this.errorMessage = null;
    this.isEditInfoModalOpen = true;
  }

  public closeEditInfoModal(): void {
    this.isEditInfoModalOpen = false;
    this.selectedUser = null;
  }

  public submitEditInfo(): void {
    if (!this.selectedUser || this.editInfoForm.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = null;

    const val = this.editInfoForm.value;
    const dto = {
      ...val,
      idCountry: Number(val.idCountry)
    };

    this.userService.updateInfo(this.selectedUser.idUser, dto).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.status) {
          this.showSuccess('Información del usuario actualizada correctamente.');
          this.closeEditInfoModal();
          this.loadUsers();
        } else {
          this.errorMessage = res.msg || 'No se pudo actualizar la información.';
        }
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.msg || 'Error al actualizar información.';
      }
    });
  }

  // --- MODIFICAR ROLES ---
  public openEditRoleModal(user: UserDTO): void {
    this.selectedUser = user;
    this.editRoleForm.patchValue({
      idRole: user.idRole
    });
    this.errorMessage = null;
    this.isEditRoleModalOpen = true;
  }

  public closeEditRoleModal(): void {
    this.isEditRoleModalOpen = false;
    this.selectedUser = null;
  }

  public submitEditRole(): void {
    if (!this.selectedUser || this.editRoleForm.invalid) return;

    this.isSubmitting = true;
    this.errorMessage = null;

    const dto = {
      idRole: Number(this.editRoleForm.value.idRole)
    };

    this.userService.updateRole(this.selectedUser.idUser, dto).subscribe({
      next: (res) => {
        this.isSubmitting = false;
        if (res.status) {
          this.showSuccess('Rol del usuario actualizado correctamente.');
          this.closeEditRoleModal();
          this.loadUsers();
        } else {
          this.errorMessage = res.msg || 'No se pudo actualizar el rol.';
        }
      },
      error: (err) => {
        this.isSubmitting = false;
        this.errorMessage = err.error?.msg || 'Error al actualizar el rol.';
      }
    });
  }

  // --- DESACTIVAR / ACTIVAR USUARIO ---
  public toggleStatus(user: UserDTO): void {
    const action = (user.isActive ?? true) ? 'desactivar' : 'activar';
    if (!confirm(`¿Está seguro de que desea ${action} al usuario ${user.firstName} ${user.lastName}?`)) {
      return;
    }

    this.userService.toggleStatus(user.idUser).subscribe({
      next: (res) => {
        if (res.status) {
          this.showSuccess(`Usuario ${action === 'desactivar' ? 'desactivado' : 'activado'} exitosamente.`);
          this.loadUsers();
        } else {
          this.errorMessage = res.msg || `No se pudo ${action} el usuario.`;
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.msg || `Error al ${action} usuario.`;
      }
    });
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => {
      this.successMessage = null;
    }, 4000);
  }
}
