import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    if (!this.authService.isLoggedIn$) {
      this.router.navigate(['']);
      return false;
    }

    const requiredRoles: string[] = route.data['roles'] || [];
    if (requiredRoles.length === 0) {
      return true;
    }

    return this.authService.getUserRole().pipe(
      map(userRole => {
        const hasAccess = requiredRoles.includes(userRole);
        if (!hasAccess) {
          this.router.navigate(['']);
        }
        return hasAccess;
      })
    );
  }
}
