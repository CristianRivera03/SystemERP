import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-purchases',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="ibm-container">
      <div class="ibm-header">
        <div>
          <h1 class="ibm-title">Módulo de Compras</h1>
          <p class="ibm-subtitle">Recepción de mercancías, facturas de proveedores y cuentas por pagar</p>
        </div>
      </div>

      <div class="under-construction-card">
        <div class="icon-wrapper">
          <i class="bx bx-shopping-bag"></i>
        </div>
        <h3>Módulo en Construcción</h3>
        <p class="description">
          Este módulo permitirá registrar compras nacionales e internacionales directas al inventario.
        </p>
        <span class="ibm-badge ibm-badge-info">Próximamente</span>
      </div>
    </div>
  `,
  styles: [`
    .ibm-container { padding: 1.5rem; color: #f4f4f4; }
    .ibm-header { margin-bottom: 2rem; }
    .ibm-title { font-size: 1.5rem; font-weight: 600; color: #fff; margin: 0; }
    .ibm-subtitle { font-size: 0.875rem; color: #a8a8a8; margin-top: 0.25rem; }
    .under-construction-card {
      background: #262626; border: 1px solid #393939; border-top: 4px solid #0f62fe;
      border-radius: 12px; padding: 3rem 2rem; text-align: center; max-width: 600px; margin: 2rem auto;
    }
    .icon-wrapper { font-size: 3.5rem; color: #0f62fe; margin-bottom: 1rem; }
    h3 { color: #fff; margin-bottom: 0.75rem; font-size: 1.25rem; }
    .description { color: #c6c6c6; margin-bottom: 1.25rem; line-height: 1.5; font-size: 0.9rem; }
    .ibm-badge-info { background: rgba(15,98,254,0.2); color: #78a9ff; border: 1px solid #0f62fe; padding: 0.35rem 0.8rem; border-radius: 12px; font-size: 0.8rem; }
  `]
})
export class PurchasesComponent {}
