﻿using System;
using WBZ.Globals;

namespace WBZ.Models
{
	public class M_Log
	{
		public int ID { get; set; }
		public int User { get; set; }
		public string UserFullname { get; set; }
		public string Module { get; set; }
		public int Instance { get; set; }
		public int Group { get; set; }
		public string Content { get; set; }
		public DateTime fDateTime { get; set; }
		public DateTime DateTime { get; set; }

		public M_Log()
		{
			UserFullname = string.Empty;
			Module = string.Empty;
			Group = 1;
			Content = string.Empty;
			fDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			DateTime = DateTime.Now;
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
