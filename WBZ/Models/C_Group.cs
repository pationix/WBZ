﻿using WBZ.Helpers;

namespace WBZ.Models
{
	public class C_Group
	{
		public static readonly string MODULE = Global.Module.GROUPS;

		public int ID { get; set; }
		public string Module { get; set; }
		public string Name { get; set; }
		public int Instance { get; set; }
		public int Owner { get; set; }
		public bool Archival { get; set; }
		public string Comment { get; set; }
		public byte[] Icon { get; set; }
		public string Fullpath { get; set; }

		public C_Group()
		{
			ID = 0;
			Module = "";
			Name = "";
			Instance = 0;
			Owner = 0;
			Archival = false;
			Comment = "";
			Icon = null;
			Fullpath = "";
		}

		public string TranslatedModule
		{
			get
			{
				return Global.TranslateModule(Module);
			}
		}
	}
}
