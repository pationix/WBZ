﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WBZ.Globals;
using WBZ.Models;
using WBZ.Modules.Groups;

namespace WBZ.Controls
{
	/// <summary>
	/// Interaction logic for GroupsTab.xaml
	/// </summary>
	public partial class GroupsTab : UserControl
	{
		D_GroupsTab D = new D_GroupsTab();
		private string Module;
		private int ID;

		public GroupsTab()
		{
			InitializeComponent();
			DataContext = D;
		}

		/// <summary>
		/// Loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
			try
			{
				Window win = Window.GetWindow(this);

				if (ID != 0 && D.InstanceGroups == null)
					D.InstanceGroups = SQL.ListInstances<M_Group>(Global.Module.GROUPS, $"g.module='{Module}' and g.instance={ID}");

				dynamic d = win?.DataContext;
				if (d != null)
				{
					Module = (string)d.MODULE_TYPE;
					ID = (int)d.InstanceInfo.ID;
				}
			}
			catch { }
		}

		/// <summary>
		/// Add
		/// </summary>
		private void btnGroupAdd_Click(object sender, MouseButtonEventArgs e)
		{
			var window = new GroupsList(Module, Commands.Type.SELECTING);
			window.Owner = Window.GetWindow(this);
			if (window.ShowDialog() == true)
            {
				var group = window.Selected;
				group.Owner = group.ID;
				group.ID = SQL.NewInstanceID(Global.Module.GROUPS);
				group.Instance = ID;
				SQL.SetInstance(Global.Module.GROUPS, group, Commands.Type.NEW);
				D.InstanceGroups = SQL.ListInstances<M_Group>(Global.Module.GROUPS, $"g.module='{Module}' and g.instance={ID}");
			}
		}

		/// <summary>
		/// Remove
		/// </summary>
		private void btnGroupRemove_Click(object sender, MouseButtonEventArgs e)
		{
			var selectedInstances = dgList.SelectedItems.Cast<M_Group>();
			if (selectedInstances.Count() > 0)
			{
				foreach (var instance in selectedInstances)
					SQL.DeleteInstance(Global.Module.GROUPS, instance.ID, instance.Name);
				D.InstanceGroups = SQL.ListInstances<M_Group>(Global.Module.GROUPS, $"g.module='{Module}' and g.instance={ID}");
			}
		}

		/// <summary>
		/// Select as main
		/// </summary>
		private void btnGroupSelectAsMain_Click(object sender, MouseButtonEventArgs e)
		{

		}
	}

	/// <summary>
	/// DataContext
	/// </summary>
	internal class D_GroupsTab : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		/// Groups
		private List<M_Group> instanceGroups;
		public List<M_Group> InstanceGroups
		{
			get
			{
				return instanceGroups;
			}
			set
			{
				instanceGroups = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
	}
}
