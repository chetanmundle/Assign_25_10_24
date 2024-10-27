import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MyToastServiceService } from '../../Services/MyToastService/my-toast-service.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  private toastr = inject(MyToastServiceService);
  private router = inject(Router);
  onClickLogout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('userId');
    this.router.navigateByUrl('/Login');
    this.toastr.showSuccess('Log Out Successfully');
  }
}
