import { Injectable } from '@angular/core';
import { Config } from './constant';
import { HttpClient } from '@angular/common/http';
import { BusinessService } from './business.service';

const REGISTER = Config.API_URL_PRODUCER + '/api/Auth/register';
const LOGIN = Config.API_URL_PRODUCER + '/api/Auth/login';
const GET_RANK_USER = (examId: any) => Config.API_URL_PRODUCER + '/api/TestUser/get-rank-user-by/' + examId;
const SEND_TEST_USER_CHOOSE = Config.API_URL_PRODUCER + '/api/TestUser/send-test-user-choose';
const GET_USERID_CURRENT = Config.API_URL_PRODUCER + '/api/Auth/get-id-username-current-user';

@Injectable({
  providedIn: 'root'
})
export class ProducerService {

  constructor(
    private httpClient: HttpClient,
    private businessService: BusinessService,
  ) { }

  getRankUserByExamId(examId: any) {
    return this.httpClient.get(GET_RANK_USER(examId), this.businessService.getRequestOptions())
  }

  sendTestUserChoose(data: any) {
    return this.httpClient.post(SEND_TEST_USER_CHOOSE, JSON.stringify(data), this.businessService.getRequestOptions())
  }

  getUserIdCurrent() {
    return this.httpClient.get(GET_USERID_CURRENT, this.businessService.getRequestOptions())
  }

}
