import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Designation } from '../../models/designation.model';

@Injectable({
  providedIn: 'root',
})
export class DesignationService {
  private apiUrl = 'http://localhost:5000/api/Designation'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  // Get all Designations
  getAllDesignations(): Observable<any[]> {
    return this.http.get<Designation[]>(this.apiUrl); // HTTP GET request to fetch all employees
  }
}
