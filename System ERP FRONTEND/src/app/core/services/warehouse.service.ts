import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/auth.models';
import { WarehouseCategoryDTO, WarehouseDTO, LocationDTO } from '../models/inventory.models';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class WarehouseService {
  private http = inject(HttpClient);
  private apiUrl = environment.endpoint + '/Warehouse';

  getCategories(): Observable<ApiResponse<WarehouseCategoryDTO[]>> {
    return this.http.get<ApiResponse<WarehouseCategoryDTO[]>>(`${this.apiUrl}/Categories`);
  }

  getWarehouses(): Observable<ApiResponse<WarehouseDTO[]>> {
    return this.http.get<ApiResponse<WarehouseDTO[]>>(`${this.apiUrl}/List`);
  }

  createWarehouse(dto: Partial<WarehouseDTO>): Observable<ApiResponse<WarehouseDTO>> {
    return this.http.post<ApiResponse<WarehouseDTO>>(`${this.apiUrl}/Create`, dto);
  }

  updateWarehouse(id: string, dto: Partial<WarehouseDTO>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.apiUrl}/Update/${id}`, dto);
  }

  toggleStatus(id: string): Observable<ApiResponse<boolean>> {
    return this.http.patch<ApiResponse<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }

  getLocations(warehouseId: string): Observable<ApiResponse<LocationDTO[]>> {
    return this.http.get<ApiResponse<LocationDTO[]>>(`${this.apiUrl}/${warehouseId}/Locations`);
  }

  createLocation(dto: Partial<LocationDTO>): Observable<ApiResponse<LocationDTO>> {
    return this.http.post<ApiResponse<LocationDTO>>(`${this.apiUrl}/CreateLocation`, dto);
  }
}
