import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PatientService {
  private http = inject(HttpClient);

  private Url = `https://localhost:7047/api/Patient`;

  createPatient(patient: any): Observable<any> {
    return this.http.post<any>(`${this.Url}/CreatePatient`, patient);
  }

  getAllPatientByAgentAndPageNumber(
    agentId: number,
    pageNum: number,
    pageSize: number
  ): Observable<any> {
    return this.http.get<any>(
      `${this.Url}/GetPatientByAgentIdInPage/agentId/${agentId}/pageSize/${pageSize}/pageNumber/${pageNum}`
    );
  }

  deletePatientById(patientId: string): Observable<any> {
    return this.http.delete<any>(
      `${this.Url}/DeletePatientById/patientId/${patientId}`
    );
  }

  getPatientById(patientId: string): Observable<any> {
    return this.http.get<any>(
      `${this.Url}/GetPatientById/patientId/${patientId}`
    );
  }

  updatePatient(patient: any): Observable<any> {
    return this.http.put<any>(`${this.Url}/UpdatePatient`, patient);
  }
}
