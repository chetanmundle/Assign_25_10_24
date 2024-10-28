import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MustMatch } from '../register/CustomeValidation/MustMatch';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../Services/UserService/user.service';
import { MyToastServiceService } from '../../Services/MyToastService/my-toast-service.service';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-foget-password',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, LoaderComponent],
  templateUrl: './foget-password.component.html',
  styleUrl: './foget-password.component.css',
})
export class FogetPasswordComponent {
  StrongPasswordRegx: RegExp =
    /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!?*.@#$]).{8,}$/;
  forgetForm: FormGroup;

  private userService = inject(UserService);
  private tostr = inject(MyToastServiceService);
  private router = inject(Router);

  isLoader: boolean = false;

  constructor(private formBuilder: FormBuilder) {
    this.forgetForm = this.formBuilder.group(
      {
        userName: ['@gmail.com', [Validators.required, Validators.email]],
        oldPassword: ['', [Validators.required]],
        newPassword: [
          '',
          [Validators.required, Validators.pattern(this.StrongPasswordRegx)],
        ],
        confirmNewPassword: ['', Validators.required],
      },
      {
        validator: MustMatch('newPassword', 'confirmNewPassword'),
      }
    );
  }

  resetform() {
    this.forgetForm = this.formBuilder.group(
      {
        userName: ['@gmail.com', [Validators.required, Validators.email]],
        oldPassword: ['', [Validators.required]],
        newPassword: [
          '',
          [Validators.required, Validators.pattern(this.StrongPasswordRegx)],
        ],
        confirmNewPassword: ['', Validators.required],
        isChecked: [false, Validators.requiredTrue],
      },
      {
        validator: MustMatch('newPassword', 'confirmNewPassword'),
      }
    );
  }

  onClickUpdateBtn() {
    this.isLoader = true;
    let Data = this.forgetForm.value;
    if (Data.newPassword !== Data.confirmNewPassword) {
      alert('New Password and ConfirmNewPassword Must be same');
      return;
    }

    this.userService.forgetPassword(Data).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.router.navigateByUrl('/Login');
          this.tostr.showSuccess('Password Updated Successfully');
        } else {
          console.log('Unable to update Password');
        }
        this.isLoader = false;
      },
      error: (error) => {
        if (error.status == 404) {
          this.tostr.showError(error.error.message);
        } else if (error.status === 400) {
          this.tostr.showError(error.error.message);
        } else {
          this.tostr.showError('Unable to update Password');
        }
        this.isLoader = false;
      },
    });
  }
}
