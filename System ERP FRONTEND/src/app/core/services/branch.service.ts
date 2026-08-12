import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/auth.models';
import { BranchDTO } from '../models/inventory.models';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class BranchService {
  private http = inject(HttpClient);
  private apiUrl = environment.endpoint + '/Branch';

  getBranches(): Observable<ApiResponse<BranchDTO[]>> {
    return this.http.get<ApiResponse<BranchDTO[]>>(`${this.apiUrl}/List`);
  }

  createBranch(dto: Partial<BranchDTO>): Observable<ApiResponse<BranchDTO>> {
    return this.http.post<ApiResponse<BranchDTO>>(`${this.apiUrl}/Create`, dto);
  }

  updateBranch(id: string, dto: Partial<BranchDTO>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.apiUrl}/Update/${id}`, dto);
  }

  toggleStatus(id: string): Observable<ApiResponse<boolean>> {
    return this.http.patch<ApiResponse<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }
}
