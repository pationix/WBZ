﻿using System.Collections.Specialized;
using WBZ.Globals;
using WBZ.Interfaces;
using MODULE_MODEL = WBZ.Models.C_Distribution;

namespace WBZ.Modules.Distributions
{
    class D_DistributionsList : D_ModuleList<MODULE_MODEL>
	{
		/// Module
		public readonly string MODULE_TYPE = Global.Module.DISTRIBUTIONS;
		public StringCollection SORTING = Properties.Settings.Default.sorting_DistributionsList;
	}
}
