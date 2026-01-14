using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Common;

namespace TechTest.Domain.Entities
{
    public sealed class Municipality : BaseEntity<int>
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;

        public Department Department { get; set; } = null!;
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
