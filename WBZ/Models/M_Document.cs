﻿using System;
using System.Data;
using WBZ.Globals;

namespace WBZ.Models
{
	public class M_Document : M
	{
		public enum DocumentStatus
		{
			Withdrawn = -1,
			Buffer = 0,
			Approved = 1
		}

		public string Type { get; set; }
		public string Name { get; set; }
		public int Store { get; set; }
		public int Contractor { get; set; }
		public DateTime fDateIssue { get; set; }
		public DateTime DateIssue { get; set; }
		public short Status { get; set; }
		public DataTable Positions { get; set; }
		public string StoreName { get; set; }
		public string ContractorName { get; set; }
		public int PositionsCount { get; set; }
		public decimal Weight { get; set; }
		public decimal Cost { get; set; }

		public M_Document()
		{
			var stores = SQL.ListInstances<M_Store>(Global.Module.STORES, "true");

			Type = "FS";
			Name = string.Empty;
			Store = stores.Count == 1 ? stores[0].ID : 0;
			fDateIssue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
			DateIssue = DateTime.Now;
			Status = (short)DocumentStatus.Buffer;
			Positions = new DataTable();
			StoreName = stores.Count == 1 ? stores[0].Name : string.Empty;
			ContractorName = string.Empty;
		}
	}

	public class M_DocumentPosition
	{
		public int ID { get; set; }
		public int Document { get; set; }
		public short Position { get; set; }
		public int Article { get; set; }
		public decimal Amount { get; set; }
		public decimal Cost { get; set; }
		public decimal Tax { get; set; }
	}
}
