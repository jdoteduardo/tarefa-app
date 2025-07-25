import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IUsuario } from '../interfaces/IUsuario';
import { Observable } from 'rxjs';
import { IAuthResponse } from '../interfaces/IAuthResponse';
import { BaseHttpService } from './base-http.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly url = '/auth';

  constructor(private baseHttp: BaseHttpService) { }

  login(usuario: IUsuario) : Observable<IAuthResponse> {
    return this.baseHttp.postClient<IAuthResponse>(`${this.url}/login`, usuario);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout(): void {
    localStorage.removeItem('token');
  }
}
