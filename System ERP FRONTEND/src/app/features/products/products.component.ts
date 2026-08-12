import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService, ProductDTO } from '../../core/services/product.service';
import { CatalogService, SubCategoryDTO, UnitMeasureDTO } from '../../core/services/catalog.service';
import { CatalogDTO } from '../../core/models/catalog.models';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  private productService = inject(ProductService);
  private catalogService = inject(CatalogService);

  products = signal<ProductDTO[]>([]);
  filteredProducts = signal<ProductDTO[]>([]);
  isLoading = signal<boolean>(false);
  searchTerm = '';
  filterCategory = 0;

  // Catalogs
  categories = signal<CatalogDTO[]>([]);
  subCategories = signal<SubCategoryDTO[]>([]);
  productTypes = signal<CatalogDTO[]>([]);
  unitMeasures = signal<UnitMeasureDTO[]>([]);

  // Notifications
  successMessage = signal<string | null>(null);
  errorMessage = signal<string | null>(null);

  // Modal State
  showModal = signal<boolean>(false);
  isEditMode = signal<boolean>(false);
  currentProductId = signal<string | null>(null);

  // Form Fields
  formName = '';
  formSku = '';
  formOriginalCode = '';
  formInternalCode = '';
  formBarcode = '';
  formCategoryId = 0;
  formSubCategoryId: number | null = null;
  formProductTypeId = 0;
  formUnitMeasureId = 0;
  formPurchaseUnitId: number | null = null;
  formSaleUnitId: number | null = null;
  formSize = '';
  formDimensions = '';
  formPresentation = '';
  formMinStock: number | null = 5;
  formIsTaxable = true;
  formDescription = '';

  ngOnInit(): void {
    this.loadProducts();
    this.loadCatalogs();
  }

  loadProducts(): void {
    this.isLoading.set(true);
    this.productService.getProducts().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.products.set(res.value);
          this.applyFilter();
        }
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.msg || 'Error al cargar productos');
        this.isLoading.set(false);
      }
    });
  }

  loadCatalogs(): void {
    this.catalogService.getCategories().subscribe(res => {
      if (res.status && res.value) this.categories.set(res.value);
    });
    this.catalogService.getProductTypes().subscribe(res => {
      if (res.status && res.value) this.productTypes.set(res.value);
    });
    this.catalogService.getUnitMeasures().subscribe(res => {
      if (res.status && res.value) this.unitMeasures.set(res.value);
    });
  }

  onCategoryChange(catId: number): void {
    this.formCategoryId = catId;
    this.formSubCategoryId = null;
    this.subCategories.set([]);
    if (catId > 0) {
      this.catalogService.getSubCategories(catId).subscribe(res => {
        if (res.status && res.value) this.subCategories.set(res.value);
      });
    }
  }

  applyFilter(): void {
    let list = this.products();
    if (this.filterCategory > 0) {
      list = list.filter(p => p.idCategory === Number(this.filterCategory));
    }
    if (this.searchTerm.trim()) {
      const term = this.searchTerm.toLowerCase().trim();
      list = list.filter(p =>
        p.name.toLowerCase().includes(term) ||
        (p.sku && p.sku.toLowerCase().includes(term)) ||
        (p.internalCode && p.internalCode.toLowerCase().includes(term)) ||
        (p.categoryName && p.categoryName.toLowerCase().includes(term))
      );
    }
    this.filteredProducts.set(list);
  }

  openCreateModal(): void {
    this.isEditMode.set(false);
    this.currentProductId.set(null);
    this.resetForm();
    this.showModal.set(true);
  }

  openEditModal(product: ProductDTO): void {
    this.isEditMode.set(true);
    this.currentProductId.set(product.idProduct || null);
    this.formName = product.name;
    this.formSku = product.sku || '';
    this.formOriginalCode = product.originalCode || '';
    this.formInternalCode = product.internalCode || '';
    this.formBarcode = product.barcode || '';
    this.formCategoryId = product.idCategory;
    this.onCategoryChange(product.idCategory);
    this.formSubCategoryId = product.idSubCategory || null;
    this.formProductTypeId = product.idProductType;
    this.formUnitMeasureId = product.idUnitMeasure;
    this.formPurchaseUnitId = product.purchaseUnitId || null;
    this.formSaleUnitId = product.saleUnitId || null;
    this.formSize = product.size || '';
    this.formDimensions = product.dimensions || '';
    this.formPresentation = product.presentation || '';
    this.formMinStock = product.minStock || 5;
    this.formIsTaxable = product.isTaxable ?? true;
    this.formDescription = product.description || '';
    this.showModal.set(true);
  }

  closeModal(): void {
    this.showModal.set(false);
    this.resetForm();
  }

  resetForm(): void {
    this.formName = '';
    this.formSku = '';
    this.formOriginalCode = '';
    this.formInternalCode = '';
    this.formBarcode = '';
    this.formCategoryId = 0;
    this.formSubCategoryId = null;
    this.formProductTypeId = 0;
    this.formUnitMeasureId = 0;
    this.formPurchaseUnitId = null;
    this.formSaleUnitId = null;
    this.formSize = '';
    this.formDimensions = '';
    this.formPresentation = '';
    this.formMinStock = 5;
    this.formIsTaxable = true;
    this.formDescription = '';
    this.subCategories.set([]);
  }

  saveProduct(): void {
    if (!this.formName.trim() || !this.formCategoryId || !this.formProductTypeId || !this.formUnitMeasureId) {
      this.errorMessage.set('Por favor complete los campos obligatorios (*)');
      return;
    }

    const payload: ProductDTO = {
      idProduct: this.currentProductId() || undefined,
      name: this.formName.trim(),
      sku: this.formSku.trim() || undefined,
      originalCode: this.formOriginalCode.trim() || undefined,
      internalCode: this.formInternalCode.trim() || undefined,
      barcode: this.formBarcode.trim() || undefined,
      idCategory: Number(this.formCategoryId),
      idSubCategory: this.formSubCategoryId ? Number(this.formSubCategoryId) : undefined,
      idProductType: Number(this.formProductTypeId),
      idUnitMeasure: Number(this.formUnitMeasureId),
      purchaseUnitId: this.formPurchaseUnitId ? Number(this.formPurchaseUnitId) : undefined,
      saleUnitId: this.formSaleUnitId ? Number(this.formSaleUnitId) : undefined,
      size: this.formSize.trim() || undefined,
      dimensions: this.formDimensions.trim() || undefined,
      presentation: this.formPresentation.trim() || undefined,
      minStock: this.formMinStock ? Number(this.formMinStock) : 0,
      isTaxable: this.formIsTaxable,
      description: this.formDescription.trim() || undefined
    };

    if (this.isEditMode() && this.currentProductId()) {
      this.productService.updateProduct(this.currentProductId()!, payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Producto actualizado exitosamente');
            this.closeModal();
            this.loadProducts();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al actualizar producto')
      });
    } else {
      this.productService.createProduct(payload).subscribe({
        next: (res) => {
          if (res.status) {
            this.successMessage.set('Producto creado exitosamente');
            this.closeModal();
            this.loadProducts();
            setTimeout(() => this.successMessage.set(null), 3000);
          }
        },
        error: (err) => this.errorMessage.set(err?.error?.msg || 'Error al crear producto')
      });
    }
  }

  toggleStatus(product: ProductDTO): void {
    if (!product.idProduct) return;
    this.productService.toggleStatus(product.idProduct).subscribe({
      next: (res) => {
        if (res.status) {
          this.loadProducts();
        }
      }
    });
  }
}
