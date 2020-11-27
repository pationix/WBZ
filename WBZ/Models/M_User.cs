﻿using System.Collections.Generic;

namespace WBZ.Models
{
	public class M_User : M
	{
		public string Username { get; set; }
		public string Newpass { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Forename { get; set; }
		public string Lastname { get; set; }
		public bool Blocked { get; set; }
		public List<string> Perms { get; set; }

		public M_User()
		{
			Username = string.Empty;
			Newpass = string.Empty;
			Email = string.Empty;
			Phone = string.Empty;
			Forename = string.Empty;
			Lastname = string.Empty;
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
