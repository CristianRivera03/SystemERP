import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';

export interface ProductDTO {
  idProduct?: string;
  idCategory: number;
  categoryName?: string;
  idSubCategory?: number;
  subCategoryName?: string;
  idProductType: number;
  productTypeDescription?: string;
  idUnitMeasure: number;
  unitMeasureDescription?: string;
  purchaseUnitId?: number;
  purchaseUnitName?: string;
  saleUnitId?: number;
  saleUnitName?: string;
  name: string;
  sku?: string;
  originalCode?: string;
  internalCode?: string;
  barcode?: string;
  size?: string;
  dimensions?: string;
  presentation?: string;
  description?: string;
  imageUrl?: string;
  isTaxable?: boolean;
  minStock?: number;
  isActive?: boolean;
  createdAt?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Product`;

  public getProducts(search?: string, categoryId?: number, subCategoryId?: number): Observable<Response<ProductDTO[]>> {
    let queryParams: string[] = [];
    if (search) queryParams.push(`search=${encodeURIComponent(search)}`);
    if (categoryId) queryParams.push(`categoryId=${categoryId}`);
    if (subCategoryId) queryParams.push(`subCategoryId=${subCategoryId}`);

    const queryString = queryParams.length > 0 ? `?${queryParams.join('&')}` : '';
    return this.http.get<Response<ProductDTO[]>>(`${this.apiUrl}/List${queryString}`);
  }

  public getProductById(id: string): Observable<Response<ProductDTO>> {
    return this.http.get<Response<ProductDTO>>(`${this.apiUrl}/${id}`);
  }

  public createProduct(dto: ProductDTO): Observable<Response<ProductDTO>> {
    return this.http.post<Response<ProductDTO>>(`${this.apiUrl}/Create`, dto);
  }

  public updateProduct(id: string, dto: ProductDTO): Observable<Response<ProductDTO>> {
    return this.http.put<Response<ProductDTO>>(`${this.apiUrl}/Update/${id}`, dto);
  }

  public toggleStatus(id: string): Observable<Response<boolean>> {
    return this.http.patch<Response<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }
}
