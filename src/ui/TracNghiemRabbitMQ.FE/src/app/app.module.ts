import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DefaultComponent } from './layouts/default/default.component';
import { HomeComponent } from './pages/home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './pages/login/login.component';
import { ExamComponent } from './pages/home/exam/exam.component';
import { ExamDetailComponent } from './pages/exam-detail/exam-detail.component';
import { ConsumerService } from './services/consumer.service';
import { ProducerService } from './services/producer.service';
import { BusinessService } from './services/business.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DoTestComponent } from './pages/do-test/do-test.component';

@NgModule({
  declarations: [
    AppComponent,
    DefaultComponent,
    HomeComponent,
    LoginComponent,
    ExamComponent,
    ExamDetailComponent,
    DoTestComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgbModule
  ],
  providers: [
    ConsumerService,
    ProducerService,
    BusinessService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
