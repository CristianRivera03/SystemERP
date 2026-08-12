import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService, CustomerDTO } from '../../core/services/customer.service';
import { CatalogService } from '../../core/services/catalog.service';
import { CatalogDTO } from '../../core/models/catalog.models';

@Component({
  selector: 'app-customers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {
  private customerService = inject(CustomerService);
  private catalogService = inject(CatalogService);

  customers = signal<CustomerDTO[]>([]);
  filteredCustomers = signal<CustomerDTO[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = '';

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Modal State
  showModal = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  currentCustomerId = signal<string | null>(null);

  // Form Fields
  formName = '';
  formTaxId = '';
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

  ngOnInit(): void {
    this.loadCustomers();
    this.loadDepartments();
  }

  loadCustomers(): void {
    this.isLoading.set(true);
    this.customerService.getCustomers().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.customers.set(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.msg || 'Error al cargar clientes');
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
    let list = this.customers();
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase().trim();
      list = list.filter(c =>
        c.name.toLowerCase().includes(term) ||
        c.taxId.toLowerCase().includes(term) ||
        (c.email && c.email.toLowerCase().includes(term)) ||
        (c.phone && c.phone.toLowerCase().includes(term))
      );
    }
    this.filteredCustomers.set(list);
  }

  openCreateModal(): void {
    this.isEditMode.set(false);
    this.currentCustomerId.set(null);
    this.resetForm();
    this.showModal.set(true);
  }

  openEditModal(customer: CustomerDTO): void {
    this.isEditMode.set(true);
    this.currentCustomerId.set(customer.idCustomer || null);
    this.formName = customer.name;
    this.formTaxId = customer.taxId;
    this.formEmail = customer.email || '';
    this.formPhone = customer.phone || '';
    this.formAddressComplement = customer.addressComplement || '';
    this.selectedDistrictId = customer.districtId;
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.resetForm();
  }

  resetForm(): void {
    this.formName = '';
    this.formTaxId = '';
    this.formEmail = '';
    this.formPhone = '';
    this.formAddressComplement = '';
    this.selectedDepartmentId = '';
    this.selectedMunicipalityId = '';
    this.selectedDistrictId = '';
  }

  saveCustomer(): void {
    if (!this.formName.trim() || !this.formTaxId.trim() || !this.selectedDistrictId) {
      this.errorMessage.set('Por favor ingrese el Nombre/Razón Social, NIT/DUI y ubicación geográfica');
      return;
    }

    const payload: CustomerDTO = {
      idCustomer: this.currentCustomerId() || undefined,
      name: this.formName.trim(),
      taxId: this.formTaxId.trim(),
      email: this.formEmail.trim() || undefined,
      phone: this.formPhone.trim() || undefined,
      districtId: this.selectedDistrictId,
      addressComplement: this.formAddressComplement.trim() || undefined
    };

    if (this.isEditMode() && this.currentCustomerId()) {
      this.customerService.updateCustomer(this.currentCustomerId()!, payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Cliente actualizado exitosamente');
            this.closeModal();
            this.loadCustomers();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al actualizar cliente')
      });
    } else {
      this.customerService.createCustomer(payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Cliente creado exitosamente');
            this.closeModal();
            this.loadCustomers();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al crear cliente')
      });
    }
  }

  toggleStatus(customer: CustomerDTO): void {
    if (!customer.idCustomer) return;
    this.customerService.toggleStatus(customer.idCustomer).subscribe({
      next: (res) => {
        if (res.status) {
          this.loadCustomers();
        }
      }
    });
  }
}
