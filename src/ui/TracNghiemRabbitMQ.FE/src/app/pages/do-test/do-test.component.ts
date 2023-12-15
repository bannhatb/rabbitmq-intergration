import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConsumerService } from 'src/app/services/consumer.service';
import { ProducerService } from 'src/app/services/producer.service';

@Component({
  selector: 'app-do-test',
  templateUrl: './do-test.component.html',
  styleUrls: ['./do-test.component.scss'],
})
export class DoTestComponent implements OnInit {
  examId: string = this.activatedRoute.snapshot.paramMap.get('id') || '-1';
  examDetailData: any;
  resultUserChooses: { questionId: number; choose: number }[] = [];
  form: FormGroup;
  userIdCurrent: any;
  selectedAnswerId: number | null = null;
  userData: {
    userId: number;
    examId: number;
    resultUserChooses: { questionId: number; choose: number }[];
  } = {
    userId: 0,
    examId: 0,
    resultUserChooses: [],
  };
  testTime: number = 30;

  constructor(
    private activatedRoute: ActivatedRoute,
    private consumerService: ConsumerService,
    private producerService: ProducerService,
    private router: Router,
    private fb: FormBuilder,
    private activeRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.getUserId();
    this.getExamDetail();
  }

  getExamDetail() {
    this.consumerService.getExamDetailById(this.examId).subscribe({
      next: (res: any) => {
        this.examDetailData = res;
        this.testTime = res.time;
        console.log(this.testTime);
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {},
    });
  }
  onItemChange(eventData: any, questionId: number) {
    const value = eventData.target.value;
    this.selectedAnswer(questionId, value);
  }
  submitData() {
    this.userData = {
      userId: this.userIdCurrent,
      examId: this.examDetailData.id,
      resultUserChooses: this.resultUserChooses,
    };
    const dataSubmit = this.userData;
    console.log(dataSubmit);
    this.producerService.sendTestUserChoose(dataSubmit).subscribe({
      next: (res: any) => {
        console.log(res);
        alert('Nộp bài thành công');
        this.router.navigateByUrl('/');
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {},
    });
  }
  getUserId() {
    this.producerService.getUserIdCurrent().subscribe({
      next: (res: any) => {
        this.userIdCurrent = res;
        console.log('UserCurrent: ' + this.userIdCurrent);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {},
    });
  }

  selectedAnswer(questionId: number, answerId: number) {
    const item = this.resultUserChooses.filter(
      (x) => x.questionId === questionId
    );
    if (!item || item.length === 0) {
      this.resultUserChooses.push({ questionId: questionId, choose: answerId });
    } else {
      item[0].choose = answerId;
    }
    console.log(this.resultUserChooses);
  }
}
