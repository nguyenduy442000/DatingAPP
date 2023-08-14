import { Observable, of } from 'rxjs';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit{
  model: any = {};

  currentUser$ : Observable<User | null > = of(null);

  constructor(private accountService: AccountService , private router: Router , private toastr: ToastrService) {}

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }


  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/members');

      },
      error: (error) => this.toastr.error(error.error) //toastr mở ra hộp thư thông báo sai mật khẩu hoặc ussername
    });
  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
