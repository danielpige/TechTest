using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Domain.Common;

namespace TechTest.Domain.Entities
{
    public sealed class Department : BaseEntity<int>
    {
        public int CountryId { get; set; }
        public string Name { get; set; } = null!;

        public Country Country { get; set; } = null!;
        public ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
    }
}
