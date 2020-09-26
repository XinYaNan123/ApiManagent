using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiAttribute
    {
        public int Id { get; set; }
        public int ApiDomain { get; set; }
        public int ApiProjectId { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeVal { get; set; }
    }
}
