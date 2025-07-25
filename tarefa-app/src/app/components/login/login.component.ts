import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';
import { IUsuario } from '../../interfaces/IUsuario';
import { catchError } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) {
    this.loginForm = this.fb.group({
      USUARIO: ['', [Validators.required, Validators.minLength(3)]],
      SENHA: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  ngOnInit() { }

  onSubmit() {
    if (this.validarForm()) {
      const usuario: IUsuario = {
        login: this.loginForm.get('USUARIO')?.value,
        senha: this.loginForm.get('SENHA')?.value
      };

      this.authService.login(usuario)
        .pipe(
          catchError(error => {
            throw error;
          })
        ).subscribe(data => this.salvarToken(data.access_token))
    }
  }

  salvarToken(token: string) {
    window.localStorage.setItem('token', token);
    this.router.navigate(["/tarefas"]);
  }

  validarForm(): boolean {
    let formValid = true;
    if (this.loginForm.valid)
      formValid = false;

    return formValid;
  }
}
