export interface OperationLog {
  id: string;
  operationType: string;
  entityName: string;
  entityId: number;
  date: string;
  time: string;
  operationDetails: string;
  databaseType: string;
  dateTime?: number;
}
