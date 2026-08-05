import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import { RegisterDTO } from '../models/auth.models';
import {
  UserDTO,
  UpdateUserNameDTO,
  UpdateUserInfoDTO,
  UpdateUserRoleDTO
} from '../models/user.models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/User`;

  public getUsers(): Observable<Response<UserDTO[]>> {
    return this.http.get<Response<UserDTO[]>>(`${this.apiUrl}/List`);
  }

  public getUserById(id: string): Observable<Response<UserDTO>> {
    return this.http.get<Response<UserDTO>>(`${this.apiUrl}/${id}`);
  }

  public registerUser(dto: RegisterDTO): Observable<Response<UserDTO>> {
    return this.http.post<Response<UserDTO>>(`${this.apiUrl}/Register`, dto);
  }

  public updateName(id: string, dto: UpdateUserNameDTO): Observable<Response<boolean>> {
    return this.http.put<Response<boolean>>(`${this.apiUrl}/UpdateName/${id}`, dto);
  }

  public updateInfo(id: string, dto: UpdateUserInfoDTO): Observable<Response<boolean>> {
    return this.http.put<Response<boolean>>(`${this.apiUrl}/UpdateInfo/${id}`, dto);
  }

  public updateRole(id: string, dto: UpdateUserRoleDTO): Observable<Response<boolean>> {
    return this.http.put<Response<boolean>>(`${this.apiUrl}/UpdateRole/${id}`, dto);
  }

  public toggleStatus(id: string): Observable<Response<boolean>> {
    return this.http.put<Response<boolean>>(`${this.apiUrl}/ToggleStatus/${id}`, {});
  }
}
