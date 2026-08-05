import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActionLogService } from '../../../core/services/action-log.service';
import { ActionLogDTO } from '../../../core/models/action-log.models';

@Component({
  selector: 'app-bitacora',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './bitacora.component.html',
  styleUrls: ['./bitacora.component.scss']
})
export class BitacoraComponent implements OnInit {
  private readonly actionLogService = inject(ActionLogService);

  public logs = signal<ActionLogDTO[]>([]);
  public loading = signal<boolean>(true);
  public searchTerm = signal<string>('');
  public selectedTableFilter = signal<string>('all');
  public selectedLog = signal<ActionLogDTO | null>(null);

  // Computed filtered logs
  public filteredLogs = computed(() => {
    const search = this.searchTerm().toLowerCase().trim();
    const tableFilter = this.selectedTableFilter().toLowerCase();
    
    return this.logs().filter(log => {
      const matchesSearch = !search || 
        (log.userName && log.userName.toLowerCase().includes(search)) ||
        (log.action && log.action.toLowerCase().includes(search)) ||
        (log.affectedTable && log.affectedTable.toLowerCase().includes(search)) ||
        (log.details && log.details.toLowerCase().includes(search)) ||
        (log.recordId && log.recordId.toLowerCase().includes(search));

      const matchesTable = tableFilter === 'all' || (log.affectedTable && log.affectedTable.toLowerCase() === tableFilter);

      return matchesSearch && matchesTable;
    });
  });

  // Unique list of tables for filter dropdown
  public availableTables = computed(() => {
    const tables = new Set<string>();
    this.logs().forEach(l => {
      if (l.affectedTable) tables.add(l.affectedTable);
    });
    return Array.from(tables);
  });

  ngOnInit(): void {
    this.loadLogs();
  }

  public loadLogs(): void {
    this.loading.set(true);
    this.actionLogService.getLogs().subscribe({
      next: (res) => {
        if (res.status && res.value) {
          this.logs.set(res.value);
        }
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  public openDetailsModal(log: ActionLogDTO): void {
    this.selectedLog.set(log);
  }

  public closeDetailsModal(): void {
    this.selectedLog.set(null);
  }

  public getBadgeClass(action: string): string {
    const act = (action || '').toUpperCase();
    if (act.includes('CREAR') || act.includes('REGISTRAR')) return 'cds-tag--green';
    if (act.includes('MODIFICAR') || act.includes('CAMBIAR')) return 'cds-tag--blue';
    if (act.includes('ELIMINAR') || act.includes('DESACTIVAR')) return 'cds-tag--red';
    if (act.includes('SESION') || act.includes('LOGIN')) return 'cds-tag--purple';
    return 'cds-tag--gray';
  }

  public getActionIcon(action: string): string {
    const act = (action || '').toUpperCase();
    if (act.includes('CREAR') || act.includes('REGISTRAR')) return 'bx-plus-circle';
    if (act.includes('MODIFICAR') || act.includes('CAMBIAR')) return 'bx-edit';
    if (act.includes('ELIMINAR')) return 'bx-trash';
    if (act.includes('SESION') || act.includes('LOGIN')) return 'bx-log-in-circle';
    if (act.includes('PERMISOS') || act.includes('ROL')) return 'bx-shield-quarter';
    return 'bx-history';
  }
}
