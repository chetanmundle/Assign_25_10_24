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
import { CommonModule } from '@angular/common';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CommonModule, LoaderComponent],
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

  passwordStrength: string = '';
  isFocucedonPass: boolean = false;
  isLoader: boolean = false;

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
    this.isLoader = true;
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
        this.isLoader = false;
      },
      error: (error) => {
        if (error.status == 409) {
          this.resetUserForm();
          this.tostr.showWarning('Username Already Exist!');
        }
        console.log('unable to Create User');
        console.log(error);
        this.isLoader = false;
      },
    });
  }

  PrintStrongNess(event: any) {
    let input_string = event.target.value;
    const n = input_string.length;
    // Checking lower alphabet in string
    let hasLower = false;
    let hasUpper = false;
    let hasDigit = false;
    let specialChar = false;
    const normalChars =
      'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890 ';

    for (let i = 0; i < n; i++) {
      if (input_string[i] >= 'a' && input_string[i] <= 'z') {
        hasLower = true;
      }
      if (input_string[i] >= 'A' && input_string[i] <= 'Z') {
        hasUpper = true;
      }
      if (input_string[i] >= '0' && input_string[i] <= '9') {
        hasDigit = true;
      }
      if (!normalChars.includes(input_string[i])) {
        specialChar = true;
      }
    }

    // Strength of password
    this.passwordStrength = 'Weak';

    if (hasLower && hasUpper && hasDigit && specialChar && n >= 8) {
      this.passwordStrength = 'Strong';
    } else if ((hasLower || hasUpper) && specialChar && n >= 6) {
      this.passwordStrength = 'Moderate';
    }

    console.log(`Strength of password: ${this.passwordStrength}`);
  }

  onPasswordBlur() {
    this.isFocucedonPass = false;
  }

  onPasswordFocus() {
    this.isFocucedonPass = true;
  }
}
