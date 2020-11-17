﻿using System.Windows;
using System.Windows.Controls;
using WBZ.Helpers;
using WBZ.Interfaces;
using MODULE_MODEL = WBZ.Models.C_User;

namespace WBZ.Modules.Users
{
	/// <summary>
	/// Interaction logic for UsersNew.xaml
	/// </summary>
	public partial class UsersNew : New
	{
		D_UsersNew D = new D_UsersNew();

		public UsersNew(MODULE_MODEL instance, Commands.Type mode)
		{
			InitializeComponent();
			DataContext = D;

			if (instance != null)
				D.InstanceInfo = instance;
			D.Mode = mode;

			D.InstanceInfo.Perms = SQL.GetUserPerms(D.InstanceInfo.ID);
			if (D.Mode.In(Commands.Type.NEW, Commands.Type.DUPLICATE))
				D.InstanceInfo.ID = SQL.NewInstanceID(D.MODULE_TYPE);
		}

		/// <summary>
		/// Add perm
		/// </summary>
		private void chckPerms_Checked(object sender, RoutedEventArgs e)
		{
			var perm = (sender as CheckBox).Tag.ToString();
			if (!D.InstanceInfo.Perms.Contains(perm))
				D.InstanceInfo.Perms.Add(perm);
		}

		/// <summary>
		/// Remove perm
		/// </summary>
		private void chckPerms_Unchecked(object sender, RoutedEventArgs e)
		{
			var perm = (sender as CheckBox).Tag.ToString();
			if (D.InstanceInfo.Perms.Contains(perm))
				D.InstanceInfo.Perms.Remove(perm);
		}
	}

	public class New : ModuleNew<MODULE_MODEL> { }
}
