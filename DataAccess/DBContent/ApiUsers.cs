using System;
using System.Collections.Generic;

namespace DataAccess.DBContent
{
    public partial class ApiUsers
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public int State { get; set; }
        public DateTime CreateDate { get; set; }
        public int? AddUserId { get; set; }
        public string UserFace { get; set; }
    }
}
