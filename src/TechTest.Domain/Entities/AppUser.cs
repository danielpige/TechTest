using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Common;

namespace TechTest.Domain.Entities
{
    public sealed class AppUser : BaseEntity<long>
    {
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int MunicipalityId { get; set; }

        public Municipality Municipality { get; set; } = null!;
    }
}
