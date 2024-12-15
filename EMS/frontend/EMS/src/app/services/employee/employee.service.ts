import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../../models/employee.model';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5000/api/employee';

  constructor(private http: HttpClient) {}

  // Get all employees
  getAllEmployees(): Observable<any[]> {
    return this.http.get<Employee[]>(this.apiUrl);
  }

  // Get all employees
  getAll<T>(entity: string): Observable<T[]> {
    return this.http.get<T[]>(`${this.apiUrl}${entity}`);
  }

  // Get employee by ID
  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/${id}`);
  }

  // Create a new employee
  createEmployee(employee: Employee): Observable<Employee> {
    return this.http.post<Employee>(this.apiUrl, employee);
  }

  // Update an existing employee
  updateEmployee(id: number, employee: Employee): Observable<Employee> {
    return this.http.put<Employee>(`${this.apiUrl}${id}`, employee);
  }

  // Delete an employee
  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
