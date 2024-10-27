import { Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { authGuard } from './Guards/auth.guard';
import { FogetPasswordComponent } from './Components/foget-password/foget-password.component';
import { PatientComponent } from './Components/patient/patient.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'Home',
    pathMatch: 'full',
  },
  {
    path: 'Home',
    component: HomeComponent,
    canActivate: [authGuard],
  },
  {
    path: 'Login',
    component: LoginComponent,
  },
  {
    path: 'Register',
    component: RegisterComponent,
  },
  {
    path: 'Forget/Password',
    component: FogetPasswordComponent,
  },
  {
    path: 'Patient',
    component: PatientComponent,
    canActivate: [authGuard],
  },
];
