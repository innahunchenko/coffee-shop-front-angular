import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.authService.isAuthenticated().pipe(
      switchMap(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['']);
          return of(false); 
        }

        const requiredRoles: string[] = route.data['roles'] || [];
        if (requiredRoles.length === 0) {
          return of(true); 
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
      })
    );
  }
}
