import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-exam',
  templateUrl: './exam.component.html',
  styleUrls: ['./exam.component.scss'],
})
export class ExamComponent implements OnInit {
  @Input() exam: any;
  constructor() {}

  ngOnInit(): void {}
}
