import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { RolePermissionListViewModel } from '../models/roll-based-access.model';
import { CryptoService } from '../services/Crypto.service';
import { DefaultValuesService } from './default-values.service';

@Injectable({
  providedIn: 'root'
})
export class RolePermissionGuard implements CanActivate {
  constructor(private defaultValueService:DefaultValuesService,private router: Router){}

  canActivate(route: ActivatedRouteSnapshot,state: RouterStateSnapshot):boolean {
    let permissions:RolePermissionListViewModel = this.defaultValueService.getPermissionList() ;
    if(!permissions){
      this.router.navigate(['/']);
      return false;
    }
      let isValidRole=false;
      for(let [i,item] of permissions.permissionList.entries()){
        if(item.permissionGroup.path==state.url && item.permissionGroup.rolePermission[0].canView){
          isValidRole=true;
          break;
        }else{
          if(item.permissionGroup.permissionCategory.length>0){
            item.permissionGroup.permissionCategory.map((cat)=>{
              if(cat.path==state.url && cat.rolePermission[0].canView){
                isValidRole=true;
              }
            });
          }
        }
         if(isValidRole){
           break;
         }
      }

      if(!isValidRole){
        this.router.navigate(['/']);
        return isValidRole;
      }
      return isValidRole;
  }
  
}
