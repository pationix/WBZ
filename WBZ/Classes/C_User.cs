﻿using System.Collections.Generic;

namespace WBZ.Classes
{
	public class C_User
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string Newpass { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Forename { get; set; }
		public string Lastname { get; set; }
		public bool Blocked { get; set; }
		public bool Archival { get; set; }
		public List<string> Perms { get; set; }

		public C_User()
		{
			ID = 0;
			Username = "";
			Newpass = "";
			Email = "";
			Phone = "";
			Forename = "";
			Lastname = "";
			Blocked = false;
			Archival = false;
			Perms = new List<string>();
		}

		public string Fullname
		{
			get
			{
				return $"{Forename} {Lastname}";
			}
		}
	}
}