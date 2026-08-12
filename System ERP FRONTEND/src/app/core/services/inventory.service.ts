import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/auth.models';
import { InventoryStockDTO } from '../models/inventory.models';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private http = inject(HttpClient);
  private apiUrl = environment.endpoint + '/Inventory';

  getStock(branchId?: string, warehouseId?: string): Observable<ApiResponse<InventoryStockDTO[]>> {
    let params = new HttpParams();
    if (branchId) params = params.set('branchId', branchId);
    if (warehouseId) params = params.set('warehouseId', warehouseId);

    return this.http.get<ApiResponse<InventoryStockDTO[]>>(`${this.apiUrl}/Stock`, { params });
  }

  adjustStock(idStock: string, newQuantity: number, reason?: string): Observable<ApiResponse<boolean>> {
    let params = new HttpParams().set('newQuantity', newQuantity.toString());
    if (reason) params = params.set('reason', reason);

    return this.http.post<ApiResponse<boolean>>(`${this.apiUrl}/AdjustStock/${idStock}`, {}, { params });
  }
}
