import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  constructor(private accountService: AccountService , private toastr :ToastrService){}
  @Output() cancelRegister = new EventEmitter();
    model :any ={}
    register(){
      this.accountService.register(this.model).subscribe({
        next:()=>{

          this.cancel();
        },
        error:error=> this.toastr.error(error.error) //toastr mở ra hộp thư thông báo
      })
    }
    cancel(){
      this.cancelRegister.emit(false);
    }
}
