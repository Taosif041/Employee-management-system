using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using static EMS.Data.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS.Models
{
    public class OperationLog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string OperationType { get; set; } 

        public string EntityName { get; set; } 

        public int EntityId { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }

        public string OperationDetails { get; set; }
        public string DatabaseType;

        public OperationLog(string operationType, string entityName, int entityId, string operationDetails, string Database)
        {
            OperationType = operationType;
            EntityName = entityName;
            EntityId = entityId;
            Date = DateTime.Now.ToString("yyyy/MM/dd");
            Time = DateTime.Now.ToString();
            OperationDetails = operationDetails;
            DatabaseType = Database;
        }

    }
}
