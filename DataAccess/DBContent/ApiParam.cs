using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiParam
    {
        public int Id { get; set; }
        public int ShowOrder { get; set; }
        public int ApiApiDetailsId { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public int Ptype { get; set; }
        public int DvalueType { get; set; }
        public string Dvalue { get; set; }
        public bool? IsMust { get; set; }
        public bool IsLower { get; set; }
        public int ParentId { get; set; }
    }
}
