// employee.model.ts
export interface Employee {
  employeeId?: number;
  officeEmployeeId: number | null;
  name: string;
  email: string;
  phone: string;
  address: string;
  dob: Date | null;
  departmentId: number | null;
  designationId: number | null;
}
