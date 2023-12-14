import { Component, OnInit } from '@angular/core';
import { ConsumerService } from 'src/app/services/consumer.service';
import { ProducerService } from 'src/app/services/producer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  fullName: string = "Vo Van Ban"
  exams:Array<any> = []

  constructor(
    private consumerService: ConsumerService,
    private producerService: ProducerService
  ) { }

  ngOnInit(): void {
    this.loadHome();
  }

  loadHome() {
    this.consumerService.getListExam()
      .subscribe(
        (res) => this.handleGetExamSuccess(res),
        (err) => this.handleGetExamError(err)
    )
  }

  handleGetExamSuccess(res: any) {
    this.exams = res
    console.log(res)
  }
  handleGetExamError(err: any) {
    console.log(err)
  }

}
