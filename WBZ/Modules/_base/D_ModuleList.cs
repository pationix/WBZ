﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using WBZ.Globals;

namespace WBZ.Modules._base
{
	class D_ModuleList<MODULE_MODEL> : INotifyPropertyChanged where MODULE_MODEL : class, new()
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		/// Instances list
		private ObservableCollection<MODULE_MODEL> instancesList;
		public ObservableCollection<MODULE_MODEL> InstancesList
		{
			get
			{
				return instancesList;
			}
			set
			{
				instancesList = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Mode
		public Commands.Type Mode { get; set; }
		/// Selecting mode
		public bool SelectingMode { get { return Mode == Commands.Type.SELECTING; } }
		/// SQL filter
		public string FilterSQL { get; set; }
		/// Filter instance
		private MODULE_MODEL filters = new MODULE_MODEL();
		public MODULE_MODEL Filters
		{
			get
			{
				return filters;
			}
			set
			{
				filters = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Page number
		private int page;
		public int Page
		{
			get
			{
				return page;
			}
			set
			{
				page = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Total instances number
		private int totalItems;
		public int TotalItems
		{
			get
			{
				return totalItems;
			}
			set
			{
				totalItems = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
	}
}
