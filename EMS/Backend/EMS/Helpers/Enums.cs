namespace EMS.Helpers
{
    public class Enums
    {
        public enum OperationType
        {
            GET,
            GetAll,
            GetById,
            Create,
            Update,
            Delete,
            Authenticate
        }
        public enum EntityName
        {
            Employee,
            Designation,
            Department,
            Operation,
            Attendance,
            User,
            RefreshToken
        }
        public enum DatabaseType
        {
            SqlSqlServer,
            PostgreSql,
            MongoDb
        }
    }
}
