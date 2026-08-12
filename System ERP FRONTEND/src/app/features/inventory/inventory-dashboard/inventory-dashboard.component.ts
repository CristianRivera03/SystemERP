import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { InventoryService } from '../../../core/services/inventory.service';
import { BranchService } from '../../../core/services/branch.service';
import { WarehouseService } from '../../../core/services/warehouse.service';
import { InventoryStockDTO, BranchDTO, WarehouseDTO } from '../../../core/models/inventory.models';

@Component({
  selector: 'app-inventory-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory-dashboard.component.html',
  styleUrls: ['./inventory-dashboard.component.scss']
})
export class InventoryDashboardComponent implements OnInit {
  private inventoryService = inject(InventoryService);
  private branchService = inject(BranchService);
  private warehouseService = inject(WarehouseService);

  stocks = signal<InventoryStockDTO[]>([]);
  filteredStocks = signal<InventoryStockDTO[]>([]);
  branches = signal<BranchDTO[]>([]);
  warehouses = signal<WarehouseDTO[]>([]);
  isLoading = signal<boolean>(false);

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Filters
  searchTerm = '';
  selectedBranchId = '';
  selectedWarehouseId = '';

  // Stats
  totalProducts = signal<number>(0);
  totalStockUnits = signal<number>(0);
  lowStockCount = signal<number>(0);

  // Modal Adjust Stock
  showAdjustModal = signal<boolean>(false);
  selectedStock = signal<InventoryStockDTO | null>(null);
  newQuantity = 0;
  adjustReason = '';

  ngOnInit(): void {
    this.loadFilters();
    this.loadStock();
  }

  loadFilters(): void {
    this.branchService.getBranches().subscribe(res => {
      if (res.status && res.value) this.branches.set(res.value);
    });

    this.warehouseService.getWarehouses().subscribe(res => {
      if (res.status && res.value) this.warehouses.set(res.value);
    });
  }

  loadStock(): void {
    this.isLoading.set(true);
    this.inventoryService.getStock(this.selectedBranchId || undefined, this.selectedWarehouseId || undefined).subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.stocks.set(res.value);
          this.calculateStats(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  calculateStats(list: InventoryStockDTO[]): void {
    this.totalProducts.set(new Set(list.map(s => s.idProduct)).size);
    this.totalStockUnits.set(list.reduce((acc, curr) => acc + (curr.quantity || 0), 0));
    this.lowStockCount.set(list.filter(s => s.quantity <= 10).length);
  }

  applyFilter(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredStocks.set(this.stocks());
    } else {
      this.filteredStocks.set(
        this.stocks().filter(s => 
          (s.productName && s.productName.toLowerCase().includes(term)) ||
          (s.productCode && s.productCode.toLowerCase().includes(term)) ||
          (s.locationCode && s.locationCode.toLowerCase().includes(term)) ||
          (s.warehouseName && s.warehouseName.toLowerCase().includes(term))
        )
      );
    }
  }

  onFilterChange(): void {
    this.loadStock();
  }

  openAdjustModal(stock: InventoryStockDTO): void {
    this.selectedStock.set(stock);
    this.newQuantity = stock.quantity;
    this.adjustReason = '';
    this.showAdjustModal.set(true);
  }

  closeAdjustModal(): void {
    this.showAdjustModal.set(false);
  }

  saveStockAdjustment(): void {
    if (!this.selectedStock()) return;

    this.inventoryService.adjustStock(this.selectedStock()!.idStock, this.newQuantity, this.adjustReason).subscribe({
      next: (res) => {
        if (res.status) {
          this.loadStock();
          this.closeAdjustModal();
        }
      }
    });
  }
}
