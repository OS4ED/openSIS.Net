import { LocationStrategy } from '@angular/common';
import { Injectable } from '@angular/core';
import { Router, CanActivate} from '@angular/router';

import { LoginService } from '../services/login.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private loginService: LoginService,
        private locationStrategy: LocationStrategy
    ) { }

    canActivate(): boolean {
        if (!this.loginService.isAuthenticated()) {
          this.router.navigate(['/']);
          return false;
        }
        return true;
      }
}