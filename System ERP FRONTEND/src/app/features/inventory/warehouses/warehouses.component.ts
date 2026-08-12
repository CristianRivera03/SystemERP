import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WarehouseService } from '../../../core/services/warehouse.service';
import { BranchService } from '../../../core/services/branch.service';
import { WarehouseDTO, WarehouseCategoryDTO, BranchDTO, LocationDTO } from '../../../core/models/inventory.models';

@Component({
  selector: 'app-warehouses',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './warehouses.component.html',
  styleUrls: ['./warehouses.component.scss']
})
export class WarehousesComponent implements OnInit {
  private warehouseService = inject(WarehouseService);
  private branchService = inject(BranchService);

  warehouses = signal<WarehouseDTO[]>([]);
  categories = signal<WarehouseCategoryDTO[]>([]);
  branches = signal<BranchDTO[]>([]);
  filteredWarehouses = signal<WarehouseDTO[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = '';

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Modal Warehouse
  showModal = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  currentWarehouseId = signal<string | null>(null);

  formName = '';
  formDescription = '';
  formBranchId = '';
  formCategoryId = 1;

  // Modal Locations
  showLocationModal = signal<boolean>(false);
  selectedWarehouse = signal<WarehouseDTO | null>(null);
  locations = signal<LocationDTO[]>([]);
  locAisle = '';
  locRack = '';
  locLevel = '';
  locPosition = '';
  locCapacity = 500;
  locNotes = '';

  ngOnInit(): void {
    this.loadInitialData();
  }

  loadInitialData(): void {
    this.isLoading.set(true);
    this.warehouseService.getCategories().subscribe(res => {
      if (res.status && res.value) this.categories.set(res.value);
    });

    this.branchService.getBranches().subscribe(res => {
      if (res.status && res.value) {
        this.branches.set(res.value);
        if (res.value.length > 0) this.formBranchId = res.value[0].idBranch;
      }
    });

    this.loadWarehouses();
  }

  loadWarehouses(): void {
    this.warehouseService.getWarehouses().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.warehouses.set(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredWarehouses.set(this.warehouses());
    } else {
      this.filteredWarehouses.set(
        this.warehouses().filter(w => 
          w.name.toLowerCase().includes(term) ||
          (w.branchName && w.branchName.toLowerCase().includes(term)) ||
          (w.categoryName && w.categoryName.toLowerCase().includes(term))
        )
      );
    }
  }

  openCreateModal(): void {
    this.isEditMode.set(false);
    this.currentWarehouseId.set(null);
    this.formName = '';
    this.formDescription = '';
    if (this.branches().length > 0) this.formBranchId = this.branches()[0].idBranch;
    this.formCategoryId = 1;
    this.showModal.set(true);
  }

  openEditModal(w: WarehouseDTO): void {
    this.isEditMode.set(true);
    this.currentWarehouseId.set(w.idWarehouse);
    this.formName = w.name;
    this.formDescription = w.description || '';
    this.formBranchId = w.idBranch;
    this.formCategoryId = w.idWarehouseCategory;
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
  }

  saveWarehouse(): void {
    if (!this.formName.trim() || !this.formBranchId) return;

    const payload: Partial<WarehouseDTO> = {
      name: this.formName.trim(),
      description: this.formDescription,
      idBranch: this.formBranchId,
      idWarehouseCategory: Number(this.formCategoryId)
    };

    if (this.isEditMode() && this.currentWarehouseId()) {
      this.warehouseService.updateWarehouse(this.currentWarehouseId()!, payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.loadWarehouses();
            this.closeModal();
          }
        }
      });
    } else {
      this.warehouseService.createWarehouse(payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.loadWarehouses();
            this.closeModal();
          }
        }
      });
    }
  }

  toggleStatus(w: WarehouseDTO): void {
    this.warehouseService.toggleStatus(w.idWarehouse).subscribe({
      next: (res) => {
        if (res.status) {
          this.loadWarehouses();
        }
      }
    });
  }

  // Location Management
  openLocationsModal(w: WarehouseDTO): void {
    this.selectedWarehouse.set(w);
    this.locAisle = '';
    this.locRack = '';
    this.locLevel = '';
    this.locPosition = '';
    this.locCapacity = 500;
    this.loadLocations(w.idWarehouse);
    this.showLocationModal.set(true);
  }

  loadLocations(warehouseId: string): void {
    this.warehouseService.getLocations(warehouseId).subscribe(res => {
      if (res.status && res.value) {
        this.locations.set(res.value);
      }
    });
  }

  closeLocationModal(): void {
    this.showLocationModal.set(false);
  }

  addLocation(): void {
    if (!this.selectedWarehouse()) return;

    const payload: Partial<LocationDTO> = {
      idWarehouse: this.selectedWarehouse()!.idWarehouse,
      aisle: this.locAisle.toUpperCase().trim(),
      rack: this.locRack.trim(),
      level: this.locLevel.trim(),
      position: this.locPosition.trim(),
      capacity: this.locCapacity,
      notes: this.locNotes.trim()
    };

    this.warehouseService.createLocation(payload).subscribe(res => {
      if (res.status) {
        this.loadLocations(this.selectedWarehouse()!.idWarehouse);
        this.locAisle = '';
        this.locRack = '';
        this.locLevel = '';
        this.locPosition = '';
        this.locNotes = '';
      }
    });
  }
}
