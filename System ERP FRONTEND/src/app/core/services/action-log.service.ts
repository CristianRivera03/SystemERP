import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../models/api-response.model';
import { ActionLogDTO } from '../models/action-log.models';

@Injectable({
  providedIn: 'root'
})
export class ActionLogService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.endpoint}/ActionLog`;

  public getLogs(): Observable<Response<ActionLogDTO[]>> {
    return this.http.get<Response<ActionLogDTO[]>>(`${this.apiUrl}/List`);
  }

  public getLogsByTable(tableName: string): Observable<Response<ActionLogDTO[]>> {
    return this.http.get<Response<ActionLogDTO[]>>(`${this.apiUrl}/Table/${tableName}`);
  }
}
