import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LocationService {
  private http = inject(HttpClient);

  getAllCountries(): Observable<any> {
    return this.http.get<any>(
      'https://localhost:7047/api/Country/GetAllCountries'
    );
  }

  getAllStatesByCountryId(countryId: number): Observable<any> {
    return this.http.get<any>(
      `https://localhost:7047/api/State/GetAllStateByCountryId/${countryId}`
    );
  }

  getAllCitiesByStateId(stateId: number): Observable<any> {
    return this.http.get<any>(
      `https://localhost:7047/api/City/GetAllCitiesByStateId/${stateId}`
    );
  }
}
