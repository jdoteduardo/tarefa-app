import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { AuthService } from '../../services/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [LoginComponent, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show formInvalido when form is invalid', () => {
    component.loginForm.setValue({ USUARIO: '', SENHA: '' });
    component.onSubmit();
    expect(component.formInvalido).toBeTrue();
    expect(component.loginError).toBeFalse();
  });

  it('should call AuthService.login and navigate on success', fakeAsync(() => {
    component.loginForm.setValue({ USUARIO: 'admin', SENHA: 'mv' });
    authServiceSpy.login.and.returnValue(of({ access_token: 'token123' }));

    component.onSubmit();
    tick();

    expect(authServiceSpy.login).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/tarefas']);
    expect(component.loginError).toBeFalse();
    expect(component.formInvalido).toBeFalse();
  }));

  it('should set loginError on login failure', fakeAsync(() => {
    component.loginForm.setValue({ USUARIO: 'admin', SENHA: 'wrong' });
    authServiceSpy.login.and.returnValue(throwError(() => new Error('Login error')));

    component.onSubmit();
    tick();

    expect(component.loginError).toBeTrue();
    expect(routerSpy.navigate).not.toHaveBeenCalled();
  }));
});