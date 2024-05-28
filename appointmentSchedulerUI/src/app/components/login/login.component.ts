import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';

  loginForm!:FormGroup;

  constructor(private fb: FormBuilder,
              private _authenticationService : AuthenticationService,
              private router: Router,
              private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      passWord: ['', Validators.required]
    })
  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.type = 'text' : this.type = 'password';
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash';
  }

  onSubmit(){
    if(this.loginForm.valid){
      this._authenticationService.login(this.loginForm.value).subscribe({
        next:(res)=>{
          this._authenticationService.storeToken(res.token);
          this.toastrService.success(res.message);
          this.loginForm.reset();
          this.router.navigate(['dashboard']);
        },
        error:(err)=>{
          this.toastrService.error(err?.error.message);
        }
      });
      
    }
    else{
      ValidateForm.validateAllFormFields(this.loginForm);
      this.toastrService.error('User is not valid!');
    }
  }

}
