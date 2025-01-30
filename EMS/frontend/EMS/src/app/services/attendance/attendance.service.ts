import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Attendance } from '../../models/attendance.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5000/api/Attendance';

  constructor(private http: HttpClient) {}

  private allAttendances: Attendance[] = [];

  getAllAttendances(): Observable<Attendance[]> {
    return this.http.get<Attendance[]>(this.apiUrl);
  }

  setAllAttendances(attendances: Attendance[]): void {
    this.allAttendances = attendances;
  }

  fetchAndSetAllAttendances(): void {
    this.getAllAttendances().subscribe({
      next: (attendances) => {
        this.setAllAttendances(attendances);
      },
      error: (err) => {
        console.error('Error fetching attendances:', err);
      },
    });
  }

  getDesignationById(id: number): Attendance[] | undefined {
    this.fetchAndSetAllAttendances();
    return this.allAttendances.filter(
      (attendance) => attendance.employeeId === id
    );
  }

  getAttendanceById(id: number): Attendance | undefined {
    return this.allAttendances.find(
      (attendance) => attendance.attendanceId === id
    );
  }

  createAttendance(attendance: Attendance): Observable<Attendance> {
    return this.http.post<Attendance>(this.apiUrl, attendance);
  }

  updateAttendance(id: number, attendance: Attendance): Observable<Attendance> {
    return this.http.put<Attendance>(`${this.apiUrl}/${id}`, attendance);
  }

  getAttendanceCSVData(): Observable<Blob> {
    return this.http.get<Blob>(`${this.apiUrl}/download-csv`, {
      responseType: 'blob' as 'json',
    });
  }

  getAttendanceExcelData(): Observable<Blob> {
    return this.http.get<Blob>(`${this.apiUrl}/download-xlsx`, {
      responseType: 'blob' as 'json',
    });
  }
}
