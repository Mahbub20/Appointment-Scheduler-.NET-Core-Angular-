import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  baseUrl : string = environment.apiUrl;

  constructor(private http : HttpClient, private router: Router, private toastrService: ToastrService) { }

  login(userObj: any){
    return this.http.post<any>(`${this.baseUrl}/User/authenticate`,userObj);
  }

  signUp(userObj: any){
    return this.http.post<any>(`${this.baseUrl}/User/register`,userObj);
  }

  storeToken(token: string){
    localStorage.setItem('token',token);
  }

  signOut(){
    localStorage.clear();
    this.toastrService.success("Logout successfully!");
    this.router.navigate(['login']);
  }

  isLoggedIn(): boolean{
    return !!localStorage.getItem('token');
  }
}
