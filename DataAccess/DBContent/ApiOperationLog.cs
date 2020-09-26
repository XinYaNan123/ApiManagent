using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiOperationLog
    {
        public int Id { get; set; }
        public int Otype { get; set; }
        public int ApiDomainId { get; set; }
        public int ApiProject { get; set; }
        public int ApiApiDetailsId { get; set; }
        public DateTime CredateDate { get; set; }
        public int UserId { get; set; }
    }
}
