﻿using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WBZ.Classes;
using WBZ.Helpers;
using INSTANCE_CLASS = WBZ.Classes.C_Employee;

namespace WBZ.Modules.Admin
{
    /// <summary>
    /// Interaction logic for EmployeesAdd.xaml
    /// </summary>
    public partial class EmployeesAdd : Window
    {
        M_EmployeesAdd M = new M_EmployeesAdd();

        public EmployeesAdd(INSTANCE_CLASS instance, Global.ActionType mode)
        {
            InitializeComponent();
            DataContext = M;

            M.InstanceInfo = instance;
			M.Mode = mode;

			if (M.Mode == Global.ActionType.NEW)
				M.InstanceInfo.ID = SQL.NewInstance(M.INSTANCE_TYPE);
		}

		/// <summary>
		/// Validation
		/// </summary>
		private bool CheckDataValidation()
		{
			bool result = true;

			return result;
		}

		/// <summary>
		/// Save
		/// </summary>
		private void btnSave_Click(object sender, MouseButtonEventArgs e)
		{
			if (!CheckDataValidation())
				return;

			if (SQL.SetInstance(M.INSTANCE_TYPE, M.InstanceInfo))
				Close();
		}

		/// <summary>
		/// Refresh
		/// </summary>
		private void btnRefresh_Click(object sender, MouseButtonEventArgs e)
		{
			if (M.InstanceInfo.ID == 0)
				return;
			//TODO - dorobić odświeżanie zmienionych danych
		}

		/// <summary>
		/// Close
		/// </summary>
		private void btnClose_Click(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Select: User
		/// </summary>
		private void btnSelectUser_Click(object sender, RoutedEventArgs e)
		{
			var window = new UsersList(true);
			if (window.ShowDialog() == true)
				if (window.Selected != null)
				{
					M.InstanceInfo.User = window.Selected.ID;
					M.InstanceInfo.UserName = window.Selected.Fullname;
					M.InstanceInfo = M.InstanceInfo;
				}
		}
	}

	/// <summary>
	/// Model
	/// </summary>
	internal class M_EmployeesAdd : INotifyPropertyChanged
	{
		public readonly string INSTANCE_TYPE = Global.Module.EMPLOYEES;

		/// Dane o zalogowanym użytkowniku
		public C_User User { get; } = Global.User;
		/// Instancja
		private INSTANCE_CLASS instanceInfo;
		public INSTANCE_CLASS InstanceInfo
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
		/// Czy okno jest w trybie edycji (zamiast w trybie dodawania)
		public bool IsEditing { get { return Mode != Global.ActionType.NEW; } }
		/// Tryb okna
		public Global.ActionType Mode { get; set; }
		/// Dodatkowa ikona okna
		public string ModeIcon
		{
			get
			{
				if (Mode == Global.ActionType.ADD)
					return "pack://siteoforigin:,,,/Resources/icon32_add.ico";
				else if (Mode == Global.ActionType.DUPLICATE)
					return "pack://siteoforigin:,,,/Resources/icon32_duplicate.ico";
				else if (Mode == Global.ActionType.EDIT)
					return "pack://siteoforigin:,,,/Resources/icon32_edit.ico";
				else
					return "pack://siteoforigin:,,,/Resources/icon32_search.ico";
			}
		}
		/// Tytuł okna
		public string Title
		{
			get
			{
				if (Mode == Global.ActionType.ADD)
					return "Nowy pracownik";
				else if (Mode == Global.ActionType.DUPLICATE)
					return $"Duplikowanie pracownika: {InstanceInfo.Fullname}";
				else if (Mode == Global.ActionType.EDIT)
					return $"Edycja pracownika: {InstanceInfo.Fullname}";
				else
					return $"Podgląd pracownika: {InstanceInfo.Fullname}";
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