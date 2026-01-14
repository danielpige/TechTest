using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Common;

namespace TechTest.Domain.Entities
{
    public sealed class Country : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string? Iso2 { get; set; }
        public ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}
