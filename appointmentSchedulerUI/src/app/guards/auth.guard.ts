import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService : AuthenticationService, private router: Router, private toastrService: ToastrService){}
  canActivate(): boolean {
    if(this.authService.isLoggedIn()){
      return true;
    }
    else{
      this.toastrService.error("Please login first!");
      this.router.navigate(['login']);
      return false;
    }
  }
  
}
