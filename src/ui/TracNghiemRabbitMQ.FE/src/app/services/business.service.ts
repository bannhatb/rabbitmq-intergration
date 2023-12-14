import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from './constant';

const TOKEN_KEY = 'token'
const LOGIN_URL = Config.API_URL_PRODUCER + '/api/Auth/login';

@Injectable({
  providedIn: 'root'
})
export class BusinessService {

  constructor(
    private httpClient: HttpClient
  ) { }

  login(data:any){
    return this.httpClient.post(LOGIN_URL, JSON.stringify(data), this.getRequestOptions())
  }

  public logout(): void{
    localStorage.removeItem(TOKEN_KEY);
  }

  setToken(token:any){
    localStorage.setItem(TOKEN_KEY, token)
  }

  getToken(){
    return localStorage.getItem(TOKEN_KEY)
  }

  getRequestOptions(){
    const token = this.getToken()
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + token
      })
    }
    return httpOptions
  }

}
