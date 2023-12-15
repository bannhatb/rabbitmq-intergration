import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConsumerService } from 'src/app/services/consumer.service';
import { ProducerService } from 'src/app/services/producer.service';

@Component({
  selector: 'app-exam-detail',
  templateUrl: './exam-detail.component.html',
  styleUrls: ['./exam-detail.component.scss'],
})
export class ExamDetailComponent implements OnInit {
  examId: string = this.activatedRoute.snapshot.paramMap.get('id') || '-1';

  examDetailData: any;
  rankUserData: any;

  constructor(
    private activatedRoute: ActivatedRoute,
    private consumerService: ConsumerService,
    private producerService: ProducerService
  ) {}

  ngOnInit(): void {
    this.getExamDetail();
  }

  getExamDetail() {
    this.consumerService.getExamDetailById(this.examId).subscribe({
      next: (res: any) => {
        this.examDetailData = res;
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {},
    });
    this.producerService.getRankUserByExamId(this.examId).subscribe({
      next: (res: any) => {
        this.rankUserData = res;
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {},
    });
  }
}
