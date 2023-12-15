import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { BusinessService } from 'src/app/services/business.service';

@Component({
  selector: 'app-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss'],
})
export class DefaultComponent implements OnInit {
  constructor(
    private authGuard: AuthGuard,
    private router: Router,
    private businessService: BusinessService
  ) {}

  ngOnInit(): void {
    this.check = this.authGuard.isAuthenticated();
  }

  check: boolean = false;
  logout1() {
    this.businessService.logout();
    this.router.navigateByUrl(`/login`);
  }
}
