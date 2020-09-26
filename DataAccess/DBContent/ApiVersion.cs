using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiVersion
    {
        public int Id { get; set; }
        public int ApiProject { get; set; }
        public string VersionCode { get; set; }
    }
}
