import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import { CatalogDTO } from '../models/catalog.models';

export interface SubCategoryDTO {
  idSubCategory: number;
  idCategory: number;
  categoryName?: string;
  name: string;
  description?: string;
  isActive?: boolean;
}

export interface UnitMeasureDTO {
  idUnitMeasure: number;
  description: string;
  name?: string;
  type?: string;
  isActive?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class CatalogService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Catalog`;

  // --- GETTERS ---
  public getRoles(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/Roles`);
  }

  public getCountries(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/Countries`);
  }

  public getCategories(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/Categories`);
  }

  public getSubCategories(categoryId?: number): Observable<Response<SubCategoryDTO[]>> {
    const url = categoryId ? `${this.apiUrl}/SubCategories?categoryId=${categoryId}` : `${this.apiUrl}/SubCategories`;
    return this.http.get<Response<SubCategoryDTO[]>>(url);
  }

  public getProductTypes(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/ProductTypes`);
  }

  public getUnitMeasures(): Observable<Response<UnitMeasureDTO[]>> {
    return this.http.get<Response<UnitMeasureDTO[]>>(`${this.apiUrl}/UnitMeasures`);
  }

  public getPresentations(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/Presentations`);
  }

  public getDepartments(): Observable<Response<CatalogDTO<string>[]>> {
    return this.http.get<Response<CatalogDTO<string>[]>>(`${this.apiUrl}/Departments`);
  }

  public getMunicipalities(departmentId: string): Observable<Response<CatalogDTO<string>[]>> {
    return this.http.get<Response<CatalogDTO<string>[]>>(`${this.apiUrl}/Municipalities/${departmentId}`);
  }

  public getDistricts(municipalityId: string): Observable<Response<CatalogDTO<string>[]>> {
    return this.http.get<Response<CatalogDTO<string>[]>>(`${this.apiUrl}/Districts/${municipalityId}`);
  }

  // --- CRUD OPERACIONES ---

  // Category
  public createCategory(dto: CatalogDTO): Observable<Response<CatalogDTO>> {
    return this.http.post<Response<CatalogDTO>>(`${this.apiUrl}/Category`, dto);
  }

  public deleteCategory(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/Category/${id}`);
  }

  // SubCategory
  public createSubCategory(dto: SubCategoryDTO): Observable<Response<SubCategoryDTO>> {
    return this.http.post<Response<SubCategoryDTO>>(`${this.apiUrl}/SubCategory`, dto);
  }

  public deleteSubCategory(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/SubCategory/${id}`);
  }

  // ProductType
  public createProductType(dto: CatalogDTO): Observable<Response<CatalogDTO>> {
    return this.http.post<Response<CatalogDTO>>(`${this.apiUrl}/ProductType`, dto);
  }

  public deleteProductType(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/ProductType/${id}`);
  }

  // UnitMeasure
  public createUnitMeasure(dto: UnitMeasureDTO): Observable<Response<UnitMeasureDTO>> {
    return this.http.post<Response<UnitMeasureDTO>>(`${this.apiUrl}/UnitMeasure`, dto);
  }

  public deleteUnitMeasure(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/UnitMeasure/${id}`);
  }

  // Presentation
  public createPresentation(dto: CatalogDTO): Observable<Response<CatalogDTO>> {
    return this.http.post<Response<CatalogDTO>>(`${this.apiUrl}/Presentation`, dto);
  }

  public deletePresentation(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/Presentation/${id}`);
  }
}
