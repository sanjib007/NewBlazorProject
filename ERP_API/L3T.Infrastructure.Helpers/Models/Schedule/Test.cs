using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Schedule
{
	public class Test
	{
		[Key]
		public int Id { get; set; }
		[Required]
        public string Name { get; set; }
    }
}
