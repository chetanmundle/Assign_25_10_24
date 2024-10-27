import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MustMatch } from './CustomeValidation/MustMatch';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../Services/UserService/user.service';
import { MyToastServiceService } from '../../Services/MyToastService/my-toast-service.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  StrongPasswordRegx: RegExp =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!?*.@#$]).{8,}$/;

  userFormData: FormGroup;

  private userService = inject(UserService);
  private router = inject(Router);
  private tostr = inject(MyToastServiceService);

  constructor(private formBuilder: FormBuilder) {
    this.userFormData = this.formBuilder.group(
      {
        fullName: ['', Validators.required],
        userName: ['@gmail.com', [Validators.required, Validators.email]],
        password: [
          '',
          [Validators.required, Validators.pattern(this.StrongPasswordRegx)],
        ],
        confirmPassword: ['', Validators.required],
        isChecked: [false, Validators.requiredTrue],
      },
      {
        validator: MustMatch('password', 'confirmPassword'),
      }
    );
  }

  resetUserForm() {
    this.userFormData = this.formBuilder.group(
      {
        fullName: ['', Validators.required],
        userName: ['@gmail.com', [Validators.required, Validators.email]],
        password: [
          '',
          [Validators.required, Validators.pattern(this.StrongPasswordRegx)],
        ],
        confirmPassword: ['', Validators.required],
        isChecked: [false, Validators.requiredTrue],
      },
      {
        validator: MustMatch('password', 'confirmPassword'),
      }
    );
  }

  onClickRegister() {
    let user = this.userFormData.value;
    console.log('User : ', user);

    if (user.password != user.confirmPassword || user.isChecked == false) {
      console.log('Password Not Match or Accept Tearms and conditions');
      return;
    }

    delete user.confirmPassword;

    this.userService.createUser(user).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.resetUserForm();
          this.router.navigateByUrl('/Login');
          this.tostr.showSuccess('Account Created Sucessfully...');
        } else {
          console.log('Unable to log in');
        }
      },
      error: (error) => {
        if (error.status == 409) {
          this.resetUserForm();
          this.tostr.showWarning('Username Already Exist!');
        }
        console.log('unable to Create User');
        console.log(error);
      },
    });
  }
}
