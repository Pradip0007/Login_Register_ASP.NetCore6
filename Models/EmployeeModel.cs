using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoginFormASPCore6.Models
{
	public class EmployeeModel
	{
		public int Id { get; set; }

		public List<SelectListItem> Employeelist { get; set; }

	}
}

