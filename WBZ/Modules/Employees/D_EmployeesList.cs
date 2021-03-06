﻿using System.Collections.Specialized;
using WBZ.Globals;
using WBZ.Modules._base;
using MODULE_MODEL = WBZ.Models.M_Employee;

namespace WBZ.Modules.Employees
{
    class D_EmployeesList : D_ModuleList<MODULE_MODEL>
	{
		/// Module
		public readonly string MODULE_TYPE = Global.Module.EMPLOYEES;
		public StringCollection SORTING = Properties.Settings.Default.sorting_EmployeesList;
	}
}
