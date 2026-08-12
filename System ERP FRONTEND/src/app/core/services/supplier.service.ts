import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';

export interface SupplierContactDTO {
  idSupplierContact?: number;
  idSupplier?: string;
  fullName: string;
  phone?: string;
  email?: string;
  isActive?: boolean;
}

export interface SupplierDTO {
  idSupplier?: string;
  name: string;
  taxId: string;
  code?: string;
  website?: string;
  email?: string;
  phone?: string;
  districtId: string;
  districtName?: string;
  municipalityName?: string;
  departmentName?: string;
  addressComplement?: string;
  isActive?: boolean;
  createdAt?: string;
  contacts?: SupplierContactDTO[];
}

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Supplier`;

  public getSuppliers(search?: string): Observable<Response<SupplierDTO[]>> {
    const url = search ? `${this.apiUrl}/List?search=${encodeURIComponent(search)}` : `${this.apiUrl}/List`;
    return this.http.get<Response<SupplierDTO[]>>(url);
  }

  public getSupplierById(id: string): Observable<Response<SupplierDTO>> {
    return this.http.get<Response<SupplierDTO>>(`${this.apiUrl}/${id}`);
  }

  public createSupplier(dto: SupplierDTO): Observable<Response<SupplierDTO>> {
    return this.http.post<Response<SupplierDTO>>(`${this.apiUrl}/Create`, dto);
  }

  public updateSupplier(id: string, dto: SupplierDTO): Observable<Response<SupplierDTO>> {
    return this.http.put<Response<SupplierDTO>>(`${this.apiUrl}/Update/${id}`, dto);
  }

  public toggleStatus(id: string): Observable<Response<boolean>> {
    return this.http.patch<Response<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }

  public addContact(dto: SupplierContactDTO): Observable<Response<SupplierContactDTO>> {
    return this.http.post<Response<SupplierContactDTO>>(`${this.apiUrl}/Contact`, dto);
  }

  public deleteContact(contactId: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/Contact/${contactId}`);
  }
}
