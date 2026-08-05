import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CatalogService } from '../../../core/services/catalog.service';
import { CatalogDTO, CatalogType } from '../../../core/models/catalog.models';

@Component({
  selector: 'app-catalogs-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalogs-dashboard.component.html',
  styleUrls: ['./catalogs-dashboard.component.scss']
})
export class CatalogsDashboardComponent implements OnInit {
  private readonly catalogService = inject(CatalogService);

  public selectedCatalog: CatalogType = 'Categories';
  public items: CatalogDTO<any>[] = [];
  public filteredItems: CatalogDTO<any>[] = [];
  public searchTerm = '';
  public isLoading = false;
  public errorMessage: string | null = null;
  public successMessage: string | null = null;

  // Cascading Location Catalog States
  public departments: CatalogDTO<string>[] = [];
  public municipalities: CatalogDTO<string>[] = [];
  public districts: CatalogDTO<string>[] = [];
  public selectedDeptId = '';
  public selectedMuniId = '';

  // Modal State for Creation
  public isModalOpen = false;
  public newItemName = '';
  public isSaving = false;

  public catalogTabs: { type: CatalogType; label: string; editable: boolean }[] = [
    { type: 'Categories', label: 'Categorías', editable: true },
    { type: 'ProductTypes', label: 'Tipos de Producto', editable: true },
    { type: 'UnitMeasures', label: 'Unidades de Medida', editable: true },
    { type: 'Presentations', label: 'Presentaciones', editable: true },
    { type: 'Departments', label: 'Ubicaciones (Deptos/Munis/Distritos)', editable: false },
    { type: 'Roles', label: 'Roles', editable: false },
    { type: 'Countries', label: 'Países', editable: false }
  ];

  ngOnInit(): void {
    this.loadCatalog(this.selectedCatalog);
  }

  public selectTab(type: CatalogType): void {
    this.selectedCatalog = type;
    this.searchTerm = '';
    this.errorMessage = null;
    this.successMessage = null;
    this.loadCatalog(type);
  }

  public loadCatalog(type: CatalogType): void {
    this.isLoading = true;
    this.items = [];
    this.filteredItems = [];

    switch (type) {
      case 'Categories':
        this.catalogService.getCategories().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'ProductTypes':
        this.catalogService.getProductTypes().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'UnitMeasures':
        this.catalogService.getUnitMeasures().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'Presentations':
        this.catalogService.getPresentations().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'Roles':
        this.catalogService.getRoles().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'Countries':
        this.catalogService.getCountries().subscribe({
          next: (res) => this.handleSuccess(res.value),
          error: (err) => this.handleError(err)
        });
        break;
      case 'Departments':
        this.loadDepartments();
        break;
    }
  }

  // Handle Location Cascading
  private loadDepartments(): void {
    this.catalogService.getDepartments().subscribe({
      next: (res) => {
        this.isLoading = false;
        this.departments = res.value || [];
        this.items = this.departments;
        this.filterItems();
      },
      error: (err) => this.handleError(err)
    });
  }

  public onDepartmentChange(): void {
    if (!this.selectedDeptId) {
      this.municipalities = [];
      this.districts = [];
      return;
    }
    this.isLoading = true;
    this.catalogService.getMunicipalities(this.selectedDeptId).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.municipalities = res.value || [];
        this.districts = [];
      },
      error: (err) => this.handleError(err)
    });
  }

  public onMunicipalityChange(): void {
    if (!this.selectedMuniId) {
      this.districts = [];
      return;
    }
    this.isLoading = true;
    this.catalogService.getDistricts(this.selectedMuniId).subscribe({
      next: (res) => {
        this.isLoading = false;
        this.districts = res.value || [];
      },
      error: (err) => this.handleError(err)
    });
  }

  private handleSuccess(data?: CatalogDTO<any>[]): void {
    this.isLoading = false;
    this.items = data || [];
    this.filterItems();
  }

  private handleError(err: any): void {
    this.isLoading = false;
    this.errorMessage = err.error?.msg || 'Ocurrió un error al consultar el catálogo.';
  }

  public filterItems(): void {
    if (!this.searchTerm.trim()) {
      this.filteredItems = [...this.items];
      return;
    }
    const term = this.searchTerm.toLowerCase();
    this.filteredItems = this.items.filter((item) =>
      item.name.toLowerCase().includes(term) || String(item.id).toLowerCase().includes(term)
    );
  }

  // Creation Modal Handlers
  public openCreateModal(): void {
    this.newItemName = '';
    this.isModalOpen = true;
  }

  public closeModal(): void {
    this.isModalOpen = false;
  }

  public saveNewItem(): void {
    if (!this.newItemName.trim()) return;

    this.isSaving = true;
    const dto: CatalogDTO = { id: 0, name: this.newItemName.trim() };

    let request$;
    switch (this.selectedCatalog) {
      case 'Categories':
        request$ = this.catalogService.createCategory(dto);
        break;
      case 'ProductTypes':
        request$ = this.catalogService.createProductType(dto);
        break;
      case 'UnitMeasures':
        request$ = this.catalogService.createUnitMeasure(dto);
        break;
      case 'Presentations':
        request$ = this.catalogService.createPresentation(dto);
        break;
      default:
        return;
    }

    request$.subscribe({
      next: (res) => {
        this.isSaving = false;
        this.closeModal();
        if (res.status) {
          this.successMessage = `Registro "${this.newItemName}" creado exitosamente.`;
          this.loadCatalog(this.selectedCatalog);
        } else {
          this.errorMessage = res.msg || 'No se pudo guardar el registro.';
        }
      },
      error: (err) => {
        this.isSaving = false;
        this.closeModal();
        this.errorMessage = err.error?.msg || 'Error al guardar el nuevo elemento.';
      }
    });
  }

  // Delete Action Handlers
  public deleteItem(item: CatalogDTO<any>): void {
    if (!confirm(`¿Está seguro de eliminar "${item.name}"?`)) return;

    this.isLoading = true;
    let delete$;

    switch (this.selectedCatalog) {
      case 'Categories':
        delete$ = this.catalogService.deleteCategory(item.id);
        break;
      case 'ProductTypes':
        delete$ = this.catalogService.deleteProductType(item.id);
        break;
      case 'UnitMeasures':
        delete$ = this.catalogService.deleteUnitMeasure(item.id);
        break;
      case 'Presentations':
        delete$ = this.catalogService.deletePresentation(item.id);
        break;
      default:
        return;
    }

    delete$.subscribe({
      next: (res) => {
        if (res.status) {
          this.successMessage = `Elemento "${item.name}" eliminado correctamente.`;
          this.loadCatalog(this.selectedCatalog);
        } else {
          this.isLoading = false;
          this.errorMessage = res.msg || 'No se pudo eliminar el registro.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.msg || 'Error al intentar eliminar el registro.';
      }
    });
  }

  public isCurrentCatalogEditable(): boolean {
    const tab = this.catalogTabs.find((t) => t.type === this.selectedCatalog);
    return tab ? tab.editable : false;
  }
}
