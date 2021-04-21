import {
    HttpEvent,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse,
    HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ResponseMessageService } from './response-message.service';
import { Injector,Injectable } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { LoaderService } from './loader.service';

@Injectable()

export class ErrorIntercept implements HttpInterceptor {
    private requests: HttpRequest<any>[] = [];
    apiUrl: string = environment.apiURL;
    tenant: string='opensisv2';
    constructor(private injector: Injector,private loginService:LoginService,private router:Router) { 
    }

    intercept(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(request)
            .pipe(
                retry(1),
                catchError((error: HttpErrorResponse) => {
                    const notifier = this.injector.get(ResponseMessageService);
 
                    let errorMessage = '';
                    if (error.error instanceof ErrorEvent) {
                        // client-side error
                        errorMessage = `Error: ${error.error.message}`;
                        notifier.showError(errorMessage);
                    } else {
                        // server-side error
                        errorMessage = `Error Status: ${error.status}\nMessage: ${error.message}`;
                        notifier.showError(errorMessage);
                    }
                    sessionStorage.setItem("httpError",errorMessage);
                    return throwError(errorMessage);
                })
            )
    }
      
}