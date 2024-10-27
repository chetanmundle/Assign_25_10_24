import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../Services/UserService/user.service';
import { MyToastServiceService } from '../../Services/MyToastService/my-toast-service.service';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule, LoaderComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginUserForm: FormGroup;

  private userService = inject(UserService);
  private router = inject(Router);
  private tostr = inject(MyToastServiceService);

  isLoader: boolean = false;

  constructor(private formBuilder: FormBuilder) {
    this.loginUserForm = this.formBuilder.group({
      userName: ['@gmail.com', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      isChecked: [false],
    });
  }

  resetUserForm() {
    this.loginUserForm = this.formBuilder.group({
      userName: ['@gmail.com', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onClickLogIn() {
    this.isLoader = true;
    let loginData = this.loginUserForm.value;

    this.userService.loginUser(loginData).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          localStorage.setItem('userId', res.data.userId);
          localStorage.setItem('access_token', 'true');
          this.router.navigateByUrl('/Home');
          //   alert('Login Successfully');
          this.isLoader = false;
        }
      },
      error: (error) => {
        if (error.status == 404) {
          this.tostr.showError('Invalid UserName and Password');
        } else {
          this.tostr.showError('Unable to Login! Server Issue');
        }
        this.resetUserForm();
        console.log(error);
        this.isLoader = false;
      },
    });
  }
}
