import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl : string = environment.apiUrl;

  constructor(private http : HttpClient) { }

  login(userObj: any){
    return this.http.post<any>(`${this.baseUrl}/User/authenticate`,userObj);
  }

  signUp(userObj: any){
    return this.http.post<any>(`${this.baseUrl}/User/register`,userObj);
  }
}
