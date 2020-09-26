using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiDebugUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? DelDate { get; set; }
    }
}
