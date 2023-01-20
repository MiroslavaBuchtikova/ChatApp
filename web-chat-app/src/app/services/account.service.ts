import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { LoginModel } from '../models/login';
import { AuthResult } from '../models/authResult';
import { RegisterModel } from '../models/register';

@Injectable({ providedIn: 'root' })
export class AccountService {
    loginModel!: LoginModel;
    registerModel!: RegisterModel;

    constructor(
        private http: HttpClient
    ) {
    }

    login(email: string, password: string) {
        this.loginModel = {
            email: email,
            password: password
        }
        return this.http.post<AuthResult>(environment.userUrl + '/login', this.loginModel)
            .pipe(map(user => {
                localStorage.setItem('jwt', user.token);
                return user;
            }));
    }


    register(username: string, email: string, password: string) {
        this.registerModel = {
            username: username,
            email: email,
            password: password
        };
        return this.http.post<AuthResult>(environment.userUrl + '/register', this.registerModel)
        .pipe(map(user => {
            localStorage.setItem('jwt', user.token);
            return user;
        }));
    }
}