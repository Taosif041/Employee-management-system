import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../../models/employee.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5000/api/employee';

  employeeMap: { [key: number]: Employee } = {};

  constructor(private http: HttpClient) {}
  private token: string | null = localStorage.getItem('auth_token');

  getAllEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  createEmployee(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  updateEmployee(id: number, employee: Employee): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}/${id}`, employee);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getEmployeeCSVData(): Observable<Blob> {
    return this.http.get<Blob>(`${this.apiUrl}/download-csv`, {
      responseType: 'blob' as 'json',
    });
  }

  getEmployeeExcelData(): Observable<Blob> {
    return this.http.get<Blob>(`${this.apiUrl}/download-xlsx`, {
      responseType: 'blob' as 'json',
    });
  }
}
