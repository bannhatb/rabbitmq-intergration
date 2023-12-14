import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { BusinessService } from '../services/business.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate, CanActivateChild {

  constructor(
    private router: Router,
    private businessService: BusinessService,
  ){

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const token = localStorage.getItem('token')
      //check token
      if(token && token !== "" && token !== undefined && token !== null){
        return true
      }
    // Redirect to login page if you aren't login
    this.router.navigate(['/login']);
    return false
  }
  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return true;
  }

  public isAuthenticated(){
  var token = this.businessService.getToken();
    if(token !== "" && token != undefined && token != null){
      return true;
    }
    else{
      return false;
    }
}

}
