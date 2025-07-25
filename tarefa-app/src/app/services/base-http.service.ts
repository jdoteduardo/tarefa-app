import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpService {

  private readonly url = 'http://localhost:5000/api/v1';

  constructor(private httpClient: HttpClient) { }

  getClient<T>(url: string, options?: Object): Observable<T> {
    options = Object.assign(options || {}, { headers: this.getHeaders() });
    return this.httpClient.get<T>(`${this.url}${url}`, options);
  }

  postClient<T>(url: string, body: any, options?: Object): Observable<T> {
    options = Object.assign(options || {}, { headers: this.getHeaders() });
    return this.httpClient.post<T>(`${this.url}${url}`, body, options);
  }

  putClient<T>(url: string, body: any, options?: Object): Observable<T> {
    options = Object.assign(options || {}, { headers: this.getHeaders() });
    return this.httpClient.put<T>(`${this.url}${url}`, body, options);
  }

  deleteClient<T>(url: string, options?: Object): Observable<T> {
    options = Object.assign(options || {}, { headers: this.getHeaders() });
    return this.httpClient.delete<T>(`${this.url}${url}`, options);
  }

  getHeaders() {

    if (window.localStorage.getItem('token')) {
      const token = window.localStorage.getItem('token');
      return new HttpHeaders({ Authorization: "Bearer " + token });
    }
    return null;
  }
}
