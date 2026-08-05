import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import { CatalogDTO } from '../models/catalog.models';

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

  public getProductTypes(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/ProductTypes`);
  }

  public getUnitMeasures(): Observable<Response<CatalogDTO[]>> {
    return this.http.get<Response<CatalogDTO[]>>(`${this.apiUrl}/UnitMeasures`);
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

  // ProductType
  public createProductType(dto: CatalogDTO): Observable<Response<CatalogDTO>> {
    return this.http.post<Response<CatalogDTO>>(`${this.apiUrl}/ProductType`, dto);
  }

  public deleteProductType(id: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/ProductType/${id}`);
  }

  // UnitMeasure
  public createUnitMeasure(dto: CatalogDTO): Observable<Response<CatalogDTO>> {
    return this.http.post<Response<CatalogDTO>>(`${this.apiUrl}/UnitMeasure`, dto);
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
