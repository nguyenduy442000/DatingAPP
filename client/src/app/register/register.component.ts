import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(private accountService: AccountService , private toastr :ToastrService, private fb: FormBuilder , private router: Router){}
  @Output() cancelRegister = new EventEmitter();
    model :any ={}
    registerForm : FormGroup = new FormGroup({});
    maxDate :Date =new Date();
    validationErrors :string[] | undefined;
    ngOnInit(): void {
      this.inititalizeForm();
      this.maxDate.setFullYear(this.maxDate.getFullYear() -18); // giới hạn độ tuổi trên 18
    }
    inititalizeForm(){
      this.registerForm = this.fb.group({
        gender: ['', Validators.required],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: ['',[Validators.required,
        Validators.minLength(4),Validators.maxLength(8)
        ]],
        confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      });
      //nếu thay đổi password thì confirmPassword cũng phải thay đổi
      this.registerForm.controls['password'].valueChanges.subscribe({
        next: ()=> this.registerForm.controls['confirmPassword'].updateValueAndValidity()
      })

    }
    // xác nhận lại password
    matchValues(matchTo: string):ValidatorFn{
      return (control : AbstractControl)=>{
        return control.value === control.parent?.get(matchTo)?.value ? null :{notMatching : true}
      }
    }


    register(){
      const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
      const values ={...this.registerForm.value , dateOfBirth: dob } ; // update date of birth
      this.accountService.register(values).subscribe({
        next:()=>{

          this.router.navigateByUrl('/members') // chuyển hướng đến Home sau khi đăng ký thành công
        },
        error:error=> {
          // this.toastr.error(error.error) // toastr mở ra hộp thư thông báo
          this.validationErrors = error
        }
      })
    }
    cancel(){
      this.cancelRegister.emit(false);
    }

    // chỉ lấy ngày tháng năm
    private getDateOnly(dob: string| undefined){
      if(!dob) return;
      let theDob = new Date(dob);
      return new Date(theDob.setMinutes(theDob.getMinutes()- theDob.getTimezoneOffset())).toISOString().slice(0,10);
    }
}
