import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5000/api/employee'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  // Get all employees
  getAllEmployees(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl); // HTTP GET request to fetch all employees
  }
}
