using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ApiDomainId { get; set; }
        public int? ShowOrder { get; set; }
        public int? IsDel { get; set; }
    }
}
