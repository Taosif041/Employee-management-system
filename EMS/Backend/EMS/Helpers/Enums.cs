namespace EMS.Helpers
{
    public class Enums
    {
        public enum OperationType
        {
            GetAll,
            GetById,
            Create,
            Update,
            Delete,

        }
        public enum EntityName
        {
            Employee,
            Designation,
            Department,
            Operation,
            Attendance,

        }
        public enum DatabaseType
        {
            SqlSqlServer,
            PostgreSql,
            MongoDb
        }


    }
}
