﻿namespace WBZ.Models
{
	public class M_Contractor : MA
	{
		public string Codename { get; set; }
		public string Name { get; set; }
		public string Branch { get; set; }
		public string NIP { get; set; }
		public string REGON { get; set; }

		public M_Contractor()
        {
			Codename = string.Empty;
			Name = string.Empty;
			Branch = string.Empty;
			NIP = string.Empty;
			REGON = string.Empty;
        }
	}
}
