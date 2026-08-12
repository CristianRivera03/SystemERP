import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CatalogService } from '../../../core/services/catalog.service';
import { CatalogDTO } from '../../../core/models/catalog.models';

@Component({
  selector: 'app-catalogs-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalogs-dashboard.component.html',
  styleUrls: ['./catalogs-dashboard.component.scss']
})
export class CatalogsDashboardComponent implements OnInit {
  private catalogService = inject(CatalogService);

  public selectedCatalog: string = 'Categories';
  public items: CatalogDTO<any>[] = [];
  public filteredItems: CatalogDTO<any>[] = [];
  public searchTerm: string = '';
  public isLoading: boolean = false;
  public errorMessage: string | null = null;
  public successMessage: string | null = null;

  // Modals / Actions
  public isModalOpen: boolean = false;
  public newItemName: string = '';
  public isSaving: boolean = false;

  // Location Selector Helper State (For Departments, Municipalities, Districts)
  public departments: CatalogDTO<string>[] = [];
  public municipalities: CatalogDTO<string>[] = [];
  public districts: CatalogDTO<string>[] = [];
  public selectedDeptId: string = '';
  public selectedMuniId: string = '';

  public catalogTabs = [
    { key: 'Categories', label: 'Categorías', icon: 'folder' },
    { key: 'ProductTypes', label: 'Tipos de Producto', icon: 'tag' },
    { key: 'UnitMeasures', label: 'Unidades de Medida', icon: 'bar-chart-2' },
    { key: 'Presentations', label: 'Presentaciones', icon: 'package' },
    { key: 'Roles', label: 'Roles de Sistema', icon: 'shield' },
    { key: 'Countries', label: 'Países', icon: 'globe' },
    { key: 'Departments', label: 'Ubicaciones (Deptos/Mun)', icon: 'map-pin' }
  ];

  ngOnInit(): void {
    this.loadCatalog(this.selectedCatalog);
  }

  public isCurrentCatalogEditable(): boolean {
    return ['Categories', 'ProductTypes', 'UnitMeasures', 'Presentations'].includes(this.selectedCatalog);
  }

  public selectTab(tabKey: string): void {
    this.selectedCatalog = tabKey;
    this.searchTerm = '';
    this.errorMessage = null;
    this.successMessage = null;
    this.loadCatalog(tabKey);
  }

  public loadCatalog(key: string): void {
    this.isLoading = true;
    switch (key) {
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
          next: (res) => {
            const mapped: CatalogDTO<number>[] = (res.value || []).map(u => ({
              id: u.idUnitMeasure,
              name: `${u.name || u.description}${u.type ? ' (' + u.type + ')' : ''}`
            }));
            this.handleSuccess(mapped);
          },
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

    if (this.selectedCatalog === 'UnitMeasures') {
      this.catalogService.createUnitMeasure({ idUnitMeasure: 0, description: dto.name, name: dto.name }).subscribe({
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
      return;
    }

    let request$;
    switch (this.selectedCatalog) {
      case 'Categories':
        request$ = this.catalogService.createCategory(dto);
        break;
      case 'ProductTypes':
        request$ = this.catalogService.createProductType(dto);
        break;
      case 'Presentations':
        request$ = this.catalogService.createPresentation(dto);
        break;
      default:
        this.isSaving = false;
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

  public deleteItem(item: CatalogDTO<any>): void {
    if (!confirm(`¿Está seguro de eliminar "${item.name}"?`)) return;

    let request$;
    const id = Number(item.id);

    switch (this.selectedCatalog) {
      case 'Categories':
        request$ = this.catalogService.deleteCategory(id);
        break;
      case 'ProductTypes':
        request$ = this.catalogService.deleteProductType(id);
        break;
      case 'UnitMeasures':
        request$ = this.catalogService.deleteUnitMeasure(id);
        break;
      case 'Presentations':
        request$ = this.catalogService.deletePresentation(id);
        break;
      default:
        alert('Este catálogo es de solo lectura.');
        return;
    }

    request$.subscribe({
      next: (res) => {
        if (res.status) {
          this.successMessage = `Elemento eliminado correctamente.`;
          this.loadCatalog(this.selectedCatalog);
        } else {
          this.errorMessage = res.msg || 'No se pudo eliminar el elemento.';
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.msg || 'Error al intentar eliminar el elemento.';
      }
    });
  }
}
