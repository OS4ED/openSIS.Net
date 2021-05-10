import { Injectable } from '@angular/core';
import {
  HttpResponse,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoaderService } from './loader.service';
import { environment } from '../../environments/environment';
import { DefaultValuesService } from '../common/default-values.service';

@Injectable()
export class LoaderInterceptor implements HttpInterceptor {
  private requests: HttpRequest<any>[] = [];
  apiUrl: string = environment.apiURL;
  tenant: string;
  constructor(
    private loaderService: LoaderService,
    private defaultValuesService: DefaultValuesService,
    ) { 
    this.tenant = this.defaultValuesService.getDefaultTenant();
  }
  removeRequest(req: HttpRequest<any>) {
    const i = this.requests.indexOf(req);
    if (i >= 0) {
      this.requests.splice(i, 1);
    }
    this.loaderService.isLoading.next(this.requests.length > 0);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (req.url !== this.apiUrl + this.tenant + "/User/checkUserLoginEmail" && req.url !== this.apiUrl + this.tenant + "/Student/checkStudentInternalId"  && req.url !== this.apiUrl + this.tenant + "/Staff/checkStaffInternalId" 
    && req.url !== this.apiUrl + this.tenant + "/School/checkSchoolInternalId" && req.url !== this.apiUrl + this.tenant +"/Grade/checkStandardRefNo") {
      this.requests.push(req);
      // console.log("No of requests--->" + this.requests.length);
      this.loaderService.isLoading.next(true);
     return this.returnRequest(req,next);
    }
    else {
      this.loaderService.isLoading.next(false);
      return this.returnRequest(req,next);
    }

  }

  returnRequest(req,next){
    return Observable.create(observer => {
      const subscription = next.handle(req)
        .subscribe(
          event => {
            if (event instanceof HttpResponse) {
              this.removeRequest(req);
              observer.next(event);
            }
          },
          err => {
            this.removeRequest(req);
            observer.error(err);
          },
          () => {
            this.removeRequest(req);
            observer.complete();
          });
      // remove request from queue when cancelled
      return () => {
        this.removeRequest(req);
        subscription.unsubscribe();
      };
    });
  }
}
