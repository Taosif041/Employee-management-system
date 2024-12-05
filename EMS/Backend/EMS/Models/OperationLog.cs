using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EMS.Models
{
    public class OperationLog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string OperationType { get; set; } // e.g., "CREATE", "READ", "UPDATE", "DELETE"

        public string EntityName { get; set; } // e.g., "Employee", "Department"

        public int EntityId { get; set; } // EntityId of the operation (0 for GETALL)

        public DateTime TimeStamp { get; set; }

        public string OperationDetails { get; set; } // Details about the operation
    }
}
