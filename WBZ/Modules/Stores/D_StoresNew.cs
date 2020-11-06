﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using WBZ.Helpers;
using WBZ.Models;
using MODULE_CLASS = WBZ.Models.C_Store;

namespace WBZ.Modules.Stores
{
    class D_StoresNew : INotifyPropertyChanged
    {
		public readonly string MODULE_NAME = Global.Module.STORES;

		/// Instance
		private MODULE_CLASS instanceInfo;
		public MODULE_CLASS InstanceInfo
		{
			get
			{
				return instanceInfo;
			}
			set
			{
				instanceInfo = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Instance source - articles
		private List<C_Article> instanceSources_Articles;
		public List<C_Article> InstanceSources_Articles
		{
			get
			{
				return instanceSources_Articles;
			}
			set
			{
				instanceSources_Articles = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Instance source - documents
		private List<C_Document> instanceSources_Documents;
		public List<C_Document> InstanceSources_Documents
		{
			get
			{
				return instanceSources_Documents;
			}
			set
			{
				instanceSources_Documents = value;
				NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
			}
		}
		/// Editing mode
		public bool EditingMode { get { return Mode != Global.ActionType.PREVIEW; } }
		/// Window mode
		public Global.ActionType Mode { get; set; }
		/// Additional window icon
		public string ModeIcon
		{
			get
			{
				if (Mode == Global.ActionType.NEW)
					return "pack://siteoforigin:,,,/Resources/icon32_add.ico";
				else if (Mode == Global.ActionType.DUPLICATE)
					return "pack://siteoforigin:,,,/Resources/icon32_duplicate.ico";
				else if (Mode == Global.ActionType.EDIT)
					return "pack://siteoforigin:,,,/Resources/icon32_edit.ico";
				else
					return "pack://siteoforigin:,,,/Resources/icon32_search.ico";
			}
		}
		/// Window title
		public string Title
		{
			get
			{
				if (Mode == Global.ActionType.NEW)
					return "Nowy magazyn";
				else if (Mode == Global.ActionType.DUPLICATE)
					return $"Duplikowanie magazynu: {InstanceInfo.Name}";
				else if (Mode == Global.ActionType.EDIT)
					return $"Edycja magazynu: {InstanceInfo.Name}";
				else
					return $"Podgląd magazynu: {InstanceInfo.Name}";
			}
		}

		/// <summary>
		/// PropertyChangedEventHandler
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}