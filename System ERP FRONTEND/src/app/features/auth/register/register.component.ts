import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { CatalogService } from '../../../core/services/catalog.service';
import { CatalogDTO } from '../../../core/models/catalog.models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly catalogService = inject(CatalogService);
  private readonly router = inject(Router);

  public registerForm: FormGroup = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    idRole: ['', Validators.required],
    idCountry: ['', Validators.required],
    phone: [''],
    documentId: ['']
  });

  public roles: CatalogDTO[] = [];
  public countries: CatalogDTO[] = [];
  public isLoading = false;
  public errorMessage: string | null = null;
  public showPassword = false;

  public toggleShowPassword(): void {
    this.showPassword = !this.showPassword;
  }

  public allowOnlyNumbers(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input) {
      const sanitized = input.value.replace(/[^0-9+]/g, '');
      if (input.value !== sanitized) {
        input.value = sanitized;
      }
    }
  }

  ngOnInit(): void {
    this.loadCatalogs();
  }

  private loadCatalogs(): void {
    this.catalogService.getRoles().subscribe({
      next: (res) => { if (res.status && res.value) this.roles = res.value; }
    });

    this.catalogService.getCountries().subscribe({
      next: (res) => { if (res.status && res.value) this.countries = res.value; }
    });
  }

  public onSubmit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const payload = {
      ...this.registerForm.value,
      idRole: Number(this.registerForm.value.idRole),
      idCountry: Number(this.registerForm.value.idCountry)
    };

    this.authService.register(payload).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.status && res.value) {
          this.router.navigate(['/catalogs']);
        } else {
          this.errorMessage = res.msg || 'No se pudo completar el registro.';
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.msg || 'Error de conexión al registrar usuario.';
      }
    });
  }
}
