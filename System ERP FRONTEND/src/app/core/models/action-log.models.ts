export interface ActionLogDTO {
  idLog: string;
  idUser?: string;
  userName?: string;
  action: string;
  affectedTable: string;
  recordId: string;
  details?: string;
  sourceIp?: string;
  actionDate?: string;
}
