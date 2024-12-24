import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Designation } from '../../models/designation.model'; // Assuming you have a Designation model

@Injectable({
  providedIn: 'root',
})
export class DesignationService {
  private apiUrl = 'http://localhost:5000/api/Designation'; // Replace with your actual API URL

  constructor(private http: HttpClient) {}

  private allDesignations: Designation[] = [];

  // Get all designations
  getAllDesignations(): Observable<any> {
    return this.http.get<Designation[]>(this.apiUrl);
  }

  // Set the local list of designations
  setAllDesignations(designations: any): any {
    this.allDesignations = designations.data;
  }

  // Get a designation by ID from the stored list
  getDesignationById(id: number): any {
    return this.allDesignations.find(
      (designation) => designation.designationId === id
    );
  }

  // Update a designation
  updateDesignation(designation: Designation, id: number): Observable<any> {
    return this.http.put<Designation>(`${this.apiUrl}/${id}`, designation);
  }

  // Create a new designation
  createDesignation(designation: Designation): Observable<any> {
    return this.http.post<Designation>(this.apiUrl, designation);
  }

  // Delete a designation
  deleteDesignation(id: number): Observable<any> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
