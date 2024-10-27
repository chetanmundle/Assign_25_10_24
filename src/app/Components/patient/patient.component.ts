import { Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { LocationService } from '../../Services/Location/location.service';
import { CommonModule, DatePipe, JsonPipe } from '@angular/common';
import { PatientService } from '../../Services/PatientService/patient.service';
import { MyToastServiceService } from '../../Services/MyToastService/my-toast-service.service';
import { jwtDecode } from 'jwt-decode';
import { TokenService } from '../../Services/TokenService/token.service';
import { LoaderComponent } from '../loader/loader.component';

@Component({
  selector: 'app-patient',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, DatePipe, LoaderComponent],
  templateUrl: './patient.component.html',
  styleUrl: './patient.component.css',
})
export class PatientComponent implements OnInit {
  currentDate: string;
  patientFormData: FormGroup;

  constructor(private formBuilder: FormBuilder) {
    const today = new Date();
    this.currentDate = today.toISOString().split('T')[0];

    // Initialize FormGroup
    this.patientFormData = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      contactNumber: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', [Validators.required]],
      countryId: ['', [Validators.required]],
      stateId: ['', [Validators.required]],
      cityId: ['', [Validators.required]],
      bloodGroup: ['', [Validators.required]],
      isChecked: [false, [Validators.requiredTrue]],
      createdBy: [null],
    });
  }

  ngOnInit(): void {
    this.getAllCountries();
    this.getAllPatients(this.pageNumber);
  }

  resetPatientForm() {
    // Initialize FormGroup
    this.patientFormData = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      contactNumber: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', [Validators.required]],
      countryId: ['', [Validators.required]],
      stateId: ['', [Validators.required]],
      cityId: ['', [Validators.required]],
      bloodGroup: ['', [Validators.required]],
      isChecked: [false, [Validators.requiredTrue]],
      createdBy: [null],
    });

    this.UpdatePatientObj = null;
    this.isUpdate = false;
  }

  private locationService = inject(LocationService);
  private patientService = inject(PatientService);
  private tostr = inject(MyToastServiceService);
  private tokenService = inject(TokenService);

  countriesList: any[] = [];
  stateList: any[] = [];
  cityList: any[] = [];
  allPatients: any[] = [];
  pageNumber: number = 1;
  pageSize: number = 5;
  isUpdate: boolean = false;
  UpdatePatientObj: any;
  isLoader: boolean = false;

  onClickSave() {
    this.isLoader = true;
    let patient = this.patientFormData.value;

    patient.createdBy = Number(this.tokenService.getUserIdFromToken());
    patient.countryId = Number(patient.countryId);
    patient.stateId = Number(patient.stateId);
    patient.cityId = Number(patient.cityId);
    patient.contactNumber = patient.contactNumber.toString();
    console.log('Patient : ', patient);

    this.patientService.createPatient(patient).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.resetPatientForm();
          this.getAllPatients(this.pageNumber);
          this.tostr.showSuccess('Patient Created Successfully');
        } else {
          console.log('Error to Save Patient : ', res);
        }
        this.isLoader = false;
      },
      error: (error: any) => {
        if (error.status == 404) {
          console.log('Bad Request : ', error.error.message);
        } else {
          console.log(error);
        }
        this.isLoader = false;
      },
    });
  }

  getAllPatients(pageNum: number) {
    let agentId = Number(this.tokenService.getUserIdFromToken());
    this.patientService
      .getAllPatientByAgentAndPageNumber(agentId, pageNum, this.pageSize)
      .subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            this.allPatients = res.data;
          } else {
            console.log('Unable to fetch data');
          }
        },
        error: (err: any) => {
          if (err.status == 404) {
            this.pageNumber -= 1;
            this.tostr.showWarning('No Data');
          }
          console.log(err);
        },
      });
  }

  onClickDelete(patientId: string) {
    if (confirm('Sure You Want to Delete this Patient')) {
      this.patientService.deletePatientById(patientId).subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            this.getAllPatients(this.pageNumber);
            this.tostr.showSuccess(
              `Patient with PatientId : ${patientId} Deleted Successfully`
            );
          } else {
            console.log('Unable to Delete Patient');
          }
        },
        error: (error) => {
          console.log('delete Error', error);
        },
      });
    }
  }

  onClickEdit(patientId: string) {
    this.patientService.getPatientById(patientId).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.UpdatePatientObj = res.data;
          this.setPatientDataOnEdit(res.data);
        } else {
          console.log('unbale to Edit');
        }
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  onClickUpdate() {
    this.UpdatePatientObj.firstName =
      this.patientFormData.get('firstName')?.value;

    this.UpdatePatientObj.lastName =
      this.patientFormData.get('lastName')?.value;

    this.UpdatePatientObj.dateOfBirth =
      this.patientFormData.get('dateOfBirth')?.value;

    this.UpdatePatientObj.gender = this.patientFormData.get('gender')?.value;

    this.UpdatePatientObj.contactNumber = this.patientFormData
      .get('contactNumber')
      ?.value.toString();

    this.UpdatePatientObj.email = this.patientFormData.get('email')?.value;

    this.UpdatePatientObj.address = this.patientFormData.get('address')?.value;

    this.UpdatePatientObj.countryId =
      this.patientFormData.get('countryId')?.value;

    this.UpdatePatientObj.stateId = this.patientFormData.get('stateId')?.value;

    this.UpdatePatientObj.cityId = this.patientFormData.get('cityId')?.value;

    this.UpdatePatientObj.bloodGroup =
      this.patientFormData.get('bloodGroup')?.value;

    this.patientService.updatePatient(this.UpdatePatientObj).subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.getAllPatients(this.pageNumber);
          this.resetPatientForm();
          this.UpdatePatientObj = null;
          console.log('obj : ', this.UpdatePatientObj);

          this.tostr.showSuccess('Patient Updated Successfully');
        }
      },
      error: (error) => {
        if (error.status == 404) {
          console.log('Not found');
        } else if (error.status == 400) {
          console.log('Bad Request');
        } else {
          console.log('Errot to update the Patient');
        }
        console.log(error);
      },
    });
  }

  //   async setPatientDataOnEdit(patient: any) {
  //     this.patientFormData.get('firstName')?.setValue(patient.firstName);
  //     this.patientFormData.get('lastName')?.setValue(patient.lastName);
  //     this.patientFormData.get('gender')?.setValue(patient.gender);
  //     this.patientFormData.get('contactNumber')?.setValue(patient.contactNumber);
  //     this.patientFormData.get('email')?.setValue(patient.email);
  //     this.patientFormData.get('address')?.setValue(patient.address);
  //     this.patientFormData.get('countryId')?.setValue(patient.countryId);

  //     await this.onChangeCountry();

  //     this.patientFormData.get('stateId')?.setValue(patient.stateId);
  //     await this.onChangeState();

  //     this.patientFormData.get('cityId')?.setValue(patient.cityId);
  //     this.patientFormData.get('bloodGroup')?.setValue(patient.bloodGroup);
  //     this.patientFormData.get('isChecked')?.setValue(patient.isChecked);
  //     this.patientFormData.get('createdBy')?.setValue(patient.createdBy);
  //     this.patientFormData
  //       .get('dateOfBirth')
  //       ?.setValue(
  //         patient.dateOfBirth
  //           ? new Date(patient.dateOfBirth).toISOString().split('T')[0]
  //           : ''
  //       );

  //     this.isUpdate = true;
  //   }

  setPatientDataOnEdit(patient: any) {
    this.patientFormData.get('firstName')?.setValue(patient.firstName);

    this.patientFormData.get('lastName')?.setValue(patient.lastName);

    this.patientFormData.get('gender')?.setValue(patient.gender);

    this.patientFormData.get('contactNumber')?.setValue(patient.contactNumber);

    this.patientFormData.get('email')?.setValue(patient.email);

    this.patientFormData.get('address')?.setValue(patient.address);

    this.patientFormData.get('countryId')?.setValue(patient.countryId);

    this.onChangeCountry();

    this.patientFormData.get('stateId')?.setValue(patient.stateId);

    this.onChangeState();

    this.patientFormData.get('cityId')?.setValue(patient.cityId);

    this.patientFormData.get('bloodGroup')?.setValue(patient.bloodGroup);

    this.patientFormData.get('isChecked')?.setValue(patient.isChecked);

    this.patientFormData.get('createdBy')?.setValue(patient.createdBy);

    this.patientFormData
      .get('dateOfBirth')
      ?.setValue(
        patient.dateOfBirth
          ? new Date(patient.dateOfBirth).toISOString().split('T')[0]
          : ''
      );

    this.isUpdate = true;
  }

  getAllCountries() {
    this.locationService.getAllCountries().subscribe({
      next: (res: any) => {
        if (res.status == 200) {
          this.countriesList = res.data;
        } else {
          console.log('Unable to get the Data');
        }
      },
      error: (err: any) => {
        console.log(err);
      },
    });
  }

  onChangeCountry() {
    this.patientFormData.get('stateId')?.setValue('');
    this.patientFormData.get('cityId')?.setValue('');
    this.cityList = [];
    this.locationService
      .getAllStatesByCountryId(this.patientFormData.get('countryId')?.value)
      .subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            this.stateList = res.data;
          } else {
            console.log('Unable to get the State');
          }
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  //   onChangeCountry(): Promise<void> {
  //     return new Promise((resolve, reject) => {
  //       this.patientFormData.get('stateId')?.setValue('');
  //       this.patientFormData.get('cityId')?.setValue('');
  //       this.cityList = [];
  //       this.locationService
  //         .getAllStatesByCountryId(this.patientFormData.get('countryId')?.value)
  //         .subscribe({
  //           next: (res: any) => {
  //             if (res.status == 200) {
  //               this.stateList = res.data;
  //               resolve();
  //             } else {
  //               console.log('Unable to get the State');
  //               reject('Unable to get the State');
  //             }
  //           },
  //           error: (error: any) => {
  //             console.log(error);
  //             reject(error);
  //           },
  //         });
  //     });
  //   }

  onChangeState() {
    this.patientFormData.get('cityId')?.setValue('');
    this.locationService
      .getAllCitiesByStateId(this.patientFormData.get('stateId')?.value)
      .subscribe({
        next: (res: any) => {
          if (res.status == 200) {
            this.cityList = res.data;
            // console.log(this.cityList);
          } else {
            console.log('Unable to get the State');
          }
        },
        error: (error: any) => {
          console.log(error);
        },
      });
  }

  onClickPrev() {
    this.pageNumber -= 1;
    this.getAllPatients(this.pageNumber);
  }
  onClickNext() {
    this.pageNumber += 1;
    this.getAllPatients(this.pageNumber);
  }

  trackByContries(index: number, country: any): number {
    return country.countryId;
  }

  trackByState(index: number, state: any): number {
    return state.stateId;
  }
  trackByCity(index: number, state: any): number {
    return state.cityId;
  }
}
