using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Application.Users.Dtos
{
    public sealed class UserDetailsRow
    {
        public long? o_user_id { get; set; }
        public string? o_full_name { get; set; }
        public string? o_phone { get; set; }
        public string? o_address { get; set; }
        public int? o_country_id { get; set; }
        public string? o_country_name { get; set; }
        public int? o_department_id { get; set; }
        public string? o_department_name { get; set; }
        public int? o_municipality_id { get; set; }
        public string? o_municipality_name { get; set; }
        public DateTimeOffset? o_created_at { get; set; }
    }
}
