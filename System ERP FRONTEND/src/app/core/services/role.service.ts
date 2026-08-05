import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import {
  ModuleDTO,
  RoleWithModulesDTO,
  UpdateRolePermissionsDTO,
  CreateRoleRequest
} from '../models/role-permissions.models';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/Role`;

  public getRolesWithModules(): Observable<Response<RoleWithModulesDTO[]>> {
    return this.http.get<Response<RoleWithModulesDTO[]>>(this.apiUrl);
  }

  public getAllModules(): Observable<Response<ModuleDTO[]>> {
    return this.http.get<Response<ModuleDTO[]>>(`${this.apiUrl}/Modules`);
  }

  public createRole(request: CreateRoleRequest): Observable<Response<RoleWithModulesDTO>> {
    return this.http.post<Response<RoleWithModulesDTO>>(this.apiUrl, request);
  }

  public updateRolePermissions(dto: UpdateRolePermissionsDTO): Observable<Response<boolean>> {
    return this.http.put<Response<boolean>>(`${this.apiUrl}/Permissions`, dto);
  }

  public deleteRole(idRole: number): Observable<Response<boolean>> {
    return this.http.delete<Response<boolean>>(`${this.apiUrl}/${idRole}`);
  }
}
