import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import ValidateForm from 'src/app/helpers/validateform';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';

  signUpForm!:FormGroup;

  constructor(private fb: FormBuilder,
              private _authenticationService : AuthenticationService,
              private router: Router,
              private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.signUpForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', Validators.required],
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
    if(this.signUpForm.valid){
      this._authenticationService.signUp(this.signUpForm.value).subscribe({
        next:(res)=>{
          this.toastrService.success(res.message);
          this.signUpForm.reset();
          this.router.navigate(['login']);
        },
        error:(err)=>{
          this.toastrService.error(err?.error.message);
        }
      });
      
    }
    else{
      ValidateForm.validateAllFormFields(this.signUpForm);
      this.toastrService.error('User is not valid!');
    }
  }

}
