import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';

export interface CustomerDTO {
  idCustomer?: string;
  name: string;
  taxId: string;
  email?: string;
  phone?: string;
  districtId: string;
  districtName?: string;
  municipalityName?: string;
  departmentName?: string;
  addressComplement?: string;
  isActive?: boolean;
  createdAt?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Customer`;

  public getCustomers(search?: string): Observable<Response<CustomerDTO[]>> {
    const url = search ? `${this.apiUrl}/List?search=${encodeURIComponent(search)}` : `${this.apiUrl}/List`;
    return this.http.get<Response<CustomerDTO[]>>(url);
  }

  public getCustomerById(id: string): Observable<Response<CustomerDTO>> {
    return this.http.get<Response<CustomerDTO>>(`${this.apiUrl}/${id}`);
  }

  public createCustomer(dto: CustomerDTO): Observable<Response<CustomerDTO>> {
    return this.http.post<Response<CustomerDTO>>(`${this.apiUrl}/Create`, dto);
  }

  public updateCustomer(id: string, dto: CustomerDTO): Observable<Response<CustomerDTO>> {
    return this.http.put<Response<CustomerDTO>>(`${this.apiUrl}/Update/${id}`, dto);
  }

  public toggleStatus(id: string): Observable<Response<boolean>> {
    return this.http.patch<Response<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }
}
