import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BusinessService } from 'src/app/services/business.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  constructor(
    private fb: FormBuilder,
    private businessService: BusinessService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.makeForm()
  }

  get username(){
    return this.form.get('username')
  }

  get password(){
    return this.form.get('password')
  }

  makeForm(){
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  submitData(){
    if(this.form.valid){
      const data = this.form.value
      const submitData = {
        "username": data.username,
        "password": data.password
      }
      console.log(submitData)
      this.businessService.login(submitData)
        .subscribe({
          next: (res:any) => {
            console.log(res)
            this.businessService.setToken(res.access_token)
            this.router.navigateByUrl('/');
          },
          error: (err) => {
            console.log(err)
          },
          complete: () => {
          }
        })
    }
  }
}
