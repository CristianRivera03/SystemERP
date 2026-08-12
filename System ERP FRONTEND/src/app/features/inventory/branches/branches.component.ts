import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BranchService } from '../../../core/services/branch.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { BranchDTO } from '../../../core/models/inventory.models';
import { CatalogDTO } from '../../../core/models/catalog.models';

@Component({
  selector: 'app-branches',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './branches.component.html',
  styleUrls: ['./branches.component.scss']
})
export class BranchesComponent implements OnInit {
  private branchService = inject(BranchService);
  private catalogService = inject(CatalogService);

  branches = signal<BranchDTO[]>([]);
  filteredBranches = signal<BranchDTO[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = '';

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Modal State
  showModal = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  currentBranchId = signal<string | null>(null);

  // Form Fields
  formName = '';
  formPhone = '';
  formEmail = '';
  formAddress = '';
  formCompanyId = 'a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11'; // Default Empresa Matriz

  // Cascaded Location Selects
  departments = signal<CatalogDTO<string>[]>([]);
  municipalities = signal<CatalogDTO<string>[]>([]);
  districts = signal<CatalogDTO<string>[]>([]);

  selectedDepartmentId = '';
  selectedMunicipalityId = '';
  selectedDistrictId = '';

  ngOnInit(): void {
    this.loadBranches();
    this.loadDepartments();
  }

  loadBranches(): void {
    this.isLoading.set(true);
    this.branchService.getBranches().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.branches.set(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Error al cargar la lista de sucursales.');
        this.isLoading.set(false);
      }
    });
  }

  loadDepartments(): void {
    this.catalogService.getDepartments().subscribe(res => {
      if (res.status && res.value) {
        this.departments.set(res.value);
      }
    });
  }

  onDepartmentChange(deptId: string): void {
    this.selectedDepartmentId = deptId;
    this.selectedMunicipalityId = '';
    this.selectedDistrictId = '';
    this.municipalities.set([]);
    this.districts.set([]);

    if (deptId) {
      this.catalogService.getMunicipalities(deptId).subscribe(res => {
        if (res.status && res.value) {
          this.municipalities.set(res.value);
        }
      });
    }
  }

  onMunicipalityChange(muniId: string): void {
    this.selectedMunicipalityId = muniId;
    this.selectedDistrictId = '';
    this.districts.set([]);

    if (muniId) {
      this.catalogService.getDistricts(muniId).subscribe(res => {
        if (res.status && res.value) {
          this.districts.set(res.value);
        }
      });
    }
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredBranches.set(this.branches());
    } else {
      this.filteredBranches.set(
        this.branches().filter(b => 
          b.name.toLowerCase().includes(term) ||
          (b.email && b.email.toLowerCase().includes(term)) ||
          (b.phone && b.phone.includes(term)) ||
          (b.departmentName && b.departmentName.toLowerCase().includes(term)) ||
          (b.municipalityName && b.municipalityName.toLowerCase().includes(term)) ||
          (b.districtName && b.districtName.toLowerCase().includes(term))
        )
      );
    }
  }

  openCreateModal(): void {
    this.isEditMode.set(false);
    this.currentBranchId.set(null);
    this.formName = '';
    this.formPhone = '';
    this.formEmail = '';
    this.formAddress = '';
    this.selectedDepartmentId = '';
    this.selectedMunicipalityId = '';
    this.selectedDistrictId = '';
    this.municipalities.set([]);
    this.districts.set([]);
    this.showModal.set(true);
  }

  openEditModal(branch: BranchDTO): void {
    this.isEditMode.set(true);
    this.currentBranchId.set(branch.idBranch);
    this.formName = branch.name;
    this.formPhone = branch.phone || '';
    this.formEmail = branch.email || '';
    this.formAddress = branch.addressComplement || '';

    // Set Location
    if (branch.departmentId) {
      this.selectedDepartmentId = branch.departmentId;
      this.catalogService.getMunicipalities(branch.departmentId).subscribe(resMuni => {
        if (resMuni.status && resMuni.value) {
          this.municipalities.set(resMuni.value);
          this.selectedMunicipalityId = branch.municipalityId || '';

          if (branch.municipalityId) {
            this.catalogService.getDistricts(branch.municipalityId).subscribe(resDist => {
              if (resDist.status && resDist.value) {
                this.districts.set(resDist.value);
                this.selectedDistrictId = branch.districtId || '';
              }
            });
          }
        }
      });
    }

    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
  }

  saveBranch(): void {
    if (!this.formName.trim() || !this.selectedDistrictId) {
      this.errorMessage.set('Completa el nombre y la ubicación (Distrito) de la sucursal.');
      return;
    }

    const payload: Partial<BranchDTO> = {
      idCompany: this.formCompanyId,
      name: this.formName.trim(),
      districtId: this.selectedDistrictId,
      addressComplement: this.formAddress,
      phone: this.formPhone,
      email: this.formEmail
    };

    if (this.isEditMode() && this.currentBranchId()) {
      this.branchService.updateBranch(this.currentBranchId()!, payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.showNotification('Sucursal actualizada correctamente.');
            this.loadBranches();
            this.closeModal();
          }
        },
        error: () => this.errorMessage.set('Error al actualizar la sucursal.')
      });
    } else {
      this.branchService.createBranch(payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.showNotification('Sucursal registrada correctamente.');
            this.loadBranches();
            this.closeModal();
          }
        },
        error: () => this.errorMessage.set('Error al crear la sucursal.')
      });
    }
  }

  toggleStatus(branch: BranchDTO): void {
    this.branchService.toggleStatus(branch.idBranch).subscribe({
      next: (res) => {
        if (res.status) {
          this.showNotification(`Sucursal ${branch.isActive ? 'desactivada' : 'activada'} correctamente.`);
          this.loadBranches();
        }
      }
    });
  }

  private showNotification(msg: string): void {
    this.successMessage.set(msg);
    setTimeout(() => this.successMessage.set(null), 4000);
  }
}
