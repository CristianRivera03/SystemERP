import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SupplierService, SupplierDTO, SupplierContactDTO } from '../../core/services/supplier.service';
import { CatalogService } from '../../core/services/catalog.service';
import { CatalogDTO } from '../../core/models/catalog.models';

@Component({
  selector: 'app-suppliers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.scss']
})
export class SuppliersComponent implements OnInit {
  private supplierService = inject(SupplierService);
  private catalogService = inject(CatalogService);

  suppliers = signal<SupplierDTO[]>([]);
  filteredSuppliers = signal<SupplierDTO[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = '';

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Modal State
  showModal = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  currentSupplierId = signal<string | null>(null);

  // Form Fields
  formName = '';
  formTaxId = '';
  formCode = '';
  formWebsite = '';
  formEmail = '';
  formPhone = '';
  formAddressComplement = '';

  // Location Cascaded Selects
  departments = signal<CatalogDTO<string>[]>([]);
  municipalities = signal<CatalogDTO<string>[]>([]);
  districts = signal<CatalogDTO<string>[]>([]);

  selectedDepartmentId = '';
  selectedMunicipalityId = '';
  selectedDistrictId = '';

  // Contacts
  contacts = signal<SupplierContactDTO[]>([]);
  newContactName = '';
  newContactPhone = '';
  newContactEmail = '';

  ngOnInit(): void {
    this.loadSuppliers();
    this.loadDepartments();
  }

  loadSuppliers(): void {
    this.isLoading.set(true);
    this.supplierService.getSuppliers().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.suppliers.set(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.msg || 'Error al cargar proveedores');
        this.isLoading.set(false);
      }
    });
  }

  loadDepartments(): void {
    this.catalogService.getDepartments().subscribe(res => {
      if (res.status && res.value) this.departments.set(res.value);
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
        if (res.status && res.value) this.municipalities.set(res.value);
      });
    }
  }

  onMunicipalityChange(muniId: string): void {
    this.selectedMunicipalityId = muniId;
    this.selectedDistrictId = '';
    this.districts.set([]);

    if (muniId) {
      this.catalogService.getDistricts(muniId).subscribe(res => {
        if (res.status && res.value) this.districts.set(res.value);
      });
    }
  }

  applyFilter(): void {
    let list = this.suppliers();
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase().trim();
      list = list.filter(s =>
        s.name.toLowerCase().includes(term) ||
        s.taxId.toLowerCase().includes(term) ||
        (s.code && s.code.toLowerCase().includes(term)) ||
        (s.email && s.email.toLowerCase().includes(term))
      );
    }
    this.filteredSuppliers.set(list);
  }

  openCreateModal(): void {
    this.isEditMode.set(false);
    this.currentSupplierId.set(null);
    this.resetForm();
    this.showModal.set(true);
  }

  openEditModal(supplier: SupplierDTO): void {
    this.isEditMode.set(true);
    this.currentSupplierId.set(supplier.idSupplier || null);
    this.formName = supplier.name;
    this.formTaxId = supplier.taxId;
    this.formCode = supplier.code || '';
    this.formWebsite = supplier.website || '';
    this.formEmail = supplier.email || '';
    this.formPhone = supplier.phone || '';
    this.formAddressComplement = supplier.addressComplement || '';
    this.selectedDistrictId = supplier.districtId;
    this.contacts.set(supplier.contacts || []);
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.resetForm();
  }

  resetForm(): void {
    this.formName = '';
    this.formTaxId = '';
    this.formCode = '';
    this.formWebsite = '';
    this.formEmail = '';
    this.formPhone = '';
    this.formAddressComplement = '';
    this.selectedDepartmentId = '';
    this.selectedMunicipalityId = '';
    this.selectedDistrictId = '';
    this.contacts.set([]);
    this.newContactName = '';
    this.newContactPhone = '';
    this.newContactEmail = '';
  }

  addContactToForm(): void {
    if (!this.newContactName.trim()) return;
    const contact: SupplierContactDTO = {
      fullName: this.newContactName.trim(),
      phone: this.newContactPhone.trim() || undefined,
      email: this.newContactEmail.trim() || undefined
    };
    this.contacts.update(list => [...list, contact]);
    this.newContactName = '';
    this.newContactPhone = '';
    this.newContactEmail = '';
  }

  removeContactFromForm(index: number): void {
    this.contacts.update(list => list.filter((_, i) => i !== index));
  }

  saveSupplier(): void {
    if (!this.formName.trim() || !this.formTaxId.trim() || !this.selectedDistrictId) {
      this.errorMessage.set('Por favor ingrese la Razón Social, NIT y ubicación geográfica');
      return;
    }

    const payload: SupplierDTO = {
      idSupplier: this.currentSupplierId() || undefined,
      name: this.formName.trim(),
      taxId: this.formTaxId.trim(),
      code: this.formCode.trim() || undefined,
      website: this.formWebsite.trim() || undefined,
      email: this.formEmail.trim() || undefined,
      phone: this.formPhone.trim() || undefined,
      districtId: this.selectedDistrictId,
      addressComplement: this.formAddressComplement.trim() || undefined,
      contacts: this.contacts()
    };

    if (this.isEditMode() && this.currentSupplierId()) {
      this.supplierService.updateSupplier(this.currentSupplierId()!, payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Proveedor actualizado exitosamente');
            this.closeModal();
            this.loadSuppliers();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al actualizar proveedor')
      });
    } else {
      this.supplierService.createSupplier(payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Proveedor creado exitosamente');
            this.closeModal();
            this.loadSuppliers();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al crear proveedor')
      });
    }
  }

  toggleStatus(supplier: SupplierDTO): void {
    if (!supplier.idSupplier) return;
    this.supplierService.toggleStatus(supplier.idSupplier).subscribe({
      next: (res) => {
        if (res.status) {
          this.loadSuppliers();
        }
      }
    });
  }
}
