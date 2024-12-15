import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Department } from '../../models/department.model';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private apiUrl = 'http://localhost:5000/api/Department'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  // Get all Departments
  getAllDepartments(): Observable<any[]> {
    return this.http.get<Department[]>(this.apiUrl); // HTTP GET request to fetch all employees
  }
}
