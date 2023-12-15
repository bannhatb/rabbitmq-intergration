import { Injectable } from '@angular/core';
import { Config } from './constant';
import { HttpClient } from '@angular/common/http';
import { BusinessService } from './business.service';

const GET_LIST_EXAM = Config.API_URL_CONSUMER + '/api/Exam/get-list-exam';
const GET_EXAM_DETAIL = (id: any) =>
  Config.API_URL_CONSUMER + '/api/Exam/get-exam-detail/' + id;

@Injectable({
  providedIn: 'root',
})
export class ConsumerService {
  constructor(
    private httpClient: HttpClient,
    private businessService: BusinessService
  ) {}

  getListExam() {
    return this.httpClient.get(
      GET_LIST_EXAM,
      this.businessService.getRequestOptions()
    );
  }

  getExamDetailById(id: any) {
    return this.httpClient.get(
      GET_EXAM_DETAIL(id),
      this.businessService.getRequestOptions()
    );
  }
}
