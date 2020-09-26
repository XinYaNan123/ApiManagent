using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiApiDetails
    {
        public int Id { get; set; }
        public int ShowOrder { get; set; }
        public int ApiDomainId { get; set; }
        public int ApiProjectId { get; set; }
        public string Title { get; set; }
        public string Describe { get; set; }
        public int State { get; set; }
        public int Methods { get; set; }
        public string SwitchName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string NameClass { get; set; }
        public string Path { get; set; }
        public string VersionCode { get; set; }
    }
}
