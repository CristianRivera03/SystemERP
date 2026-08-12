import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-under-construction',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="ibm-container">
      <div class="under-construction-card">
        <div class="icon-wrapper">
          <i class="bx bx-wrench"></i>
        </div>
        <h2>Módulo en Desarrollo</h2>
        <p class="module-path">Ruta: <code>{{ currentPath }}</code></p>
        <p class="description">
          Este módulo está programado para la siguiente fase de desarrollo. La estructura de datos y servicios está preparada.
        </p>
        <button class="ibm-btn ibm-btn-primary" (click)="goHome()">
          Volver a Catálogos
        </button>
      </div>
    </div>
  `,
  styles: [`
    .ibm-container {
      padding: 3rem 1.5rem;
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 70vh;
    }
    .under-construction-card {
      background: #262626;
      border: 1px solid #393939;
      border-top: 4px solid #0f62fe;
      border-radius: 12px;
      padding: 3rem 2rem;
      text-align: center;
      max-width: 500px;
      box-shadow: 0 10px 30px rgba(0,0,0,0.5);
    }
    .icon-wrapper {
      font-size: 3.5rem;
      color: #0f62fe;
      margin-bottom: 1rem;
    }
    h2 {
      color: #ffffff;
      margin-bottom: 0.5rem;
    }
    .module-path {
      color: #a8a8a8;
      font-size: 0.9rem;
      margin-bottom: 1rem;
      code {
        background: #161616;
        color: #78a9ff;
        padding: 0.2rem 0.5rem;
        border-radius: 4px;
      }
    }
    .description {
      color: #c6c6c6;
      font-size: 0.95rem;
      margin-bottom: 1.5rem;
      line-height: 1.5;
    }
    .ibm-btn-primary {
      background: #0f62fe;
      color: #fff;
      border: none;
      padding: 0.6rem 1.5rem;
      border-radius: 4px;
      cursor: pointer;
      font-weight: 500;
      &:hover {
        background: #0353e9;
      }
    }
  `]
})
export class UnderConstructionComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  currentPath = '';

  ngOnInit(): void {
    this.currentPath = this.router.url;
  }

  goHome(): void {
    this.router.navigate(['/catalogs']);
  }
}
