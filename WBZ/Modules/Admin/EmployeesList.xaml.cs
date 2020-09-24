﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WBZ.Classes;
using WBZ.Helpers;
using INSTANCE_CLASS = WBZ.Classes.C_Employee;

namespace WBZ.Modules.Admin
{
    /// <summary>
    /// Interaction logic for EmployeesList.xaml
    /// </summary>
    public partial class EmployeesList : Window
    {
        M_EmployeesList M = new M_EmployeesList();

        public EmployeesList(bool selectingMode = false)
        {
            InitializeComponent();
            DataContext = M;
            btnRefresh_Click(null, null);

            M.SelectingMode = selectingMode;
            if (M.SelectingMode)
                dgList.SelectionMode = DataGridSelectionMode.Single;
        }

		/// <summary>
		/// Update filters
		/// </summary>
		private void UpdateFilters()
		{
			M.FilterSQL = $"LOWER(COALESCE(e.forename,'')) like '%{M.Filters.Forename.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.lastname,'')) like '%{M.Filters.Lastname.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.department,'')) like '%{M.Filters.Department.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.position,'')) like '%{M.Filters.Position.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.email,'')) like '%{M.Filters.Email.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.phone,'')) like '%{M.Filters.Phone.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.postcode,'')) like '%{M.Filters.Postcode.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.city,'')) like '%{M.Filters.City.ToLower()}%' and "
						+ $"LOWER(COALESCE(e.address,'')) like '%{M.Filters.Address.ToLower()}%' and "
						+ (M.Filters.Archival ? $"e.archival=false and " : "");

			M.FilterSQL = M.FilterSQL.TrimEnd(" and ".ToCharArray());
		}

		/// <summary>
		/// Apply filters
		/// </summary>
		private void dpFilter_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				btnRefresh_Click(null, null);
		}

		/// <summary>
		/// Clear filters
		/// </summary>
		private void btnFiltersClear_Click(object sender, MouseButtonEventArgs e)
		{
			M.Filters = new INSTANCE_CLASS();
			btnRefresh_Click(null, null);
		}

		/// <summary>
		/// Preview
		/// </summary>
		private void btnPreview_Click(object sender, MouseButtonEventArgs e)
		{
			var selectedInstances = dgList.SelectedItems.Cast<INSTANCE_CLASS>();
			foreach (INSTANCE_CLASS instance in selectedInstances)
			{
				var window = new EmployeesAdd(instance, Global.ActionType.PREVIEW);
				window.Show();
			}
		}

		/// <summary>
		/// Add
		/// </summary>
		private void btnAdd_Click(object sender, MouseButtonEventArgs e)
		{
			var window = new EmployeesAdd(new INSTANCE_CLASS(), Global.ActionType.NEW);
			window.Show();
		}

		/// <summary>
		/// Duplicate
		/// </summary>
		private void btnDuplicate_Click(object sender, MouseButtonEventArgs e)
		{
			var selectedInstances = dgList.SelectedItems.Cast<INSTANCE_CLASS>();
			foreach (INSTANCE_CLASS instance in selectedInstances)
			{
				var window = new EmployeesAdd(instance, Global.ActionType.NEW);
				window.Show();
			}
		}

		/// <summary>
		/// Edit
		/// </summary>
		private void btnEdit_Click(object sender, MouseButtonEventArgs e)
		{
			var selectedInstances = dgList.SelectedItems.Cast<INSTANCE_CLASS>();
			foreach (INSTANCE_CLASS instance in selectedInstances)
			{
				var window = new EmployeesAdd(instance, Global.ActionType.EDIT);
				window.Show();
			}
		}

		/// <summary>
		/// Delete
		/// </summary>
		private void btnDelete_Click(object sender, MouseButtonEventArgs e)
		{
			var selectedInstances = dgList.SelectedItems.Cast<INSTANCE_CLASS>();
			if (selectedInstances.Count() > 0 && MessageBox.Show("Czy na pewno usunąć zaznaczone rekordy?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				foreach (INSTANCE_CLASS instance in selectedInstances)
					SQL.DeleteInstance(M.INSTANCE_TYPE, instance.ID);
				btnRefresh_Click(null, null);
			}
		}

		/// <summary>
		/// Refresh
		/// </summary>
		private async void btnRefresh_Click(object sender, MouseButtonEventArgs e)
		{
			await Task.Run(() => {
				UpdateFilters();
				M.TotalItems = SQL.CountInstances(M.INSTANCE_TYPE, M.FilterSQL);
				M.InstancesList = SQL.ListInstances(M.INSTANCE_TYPE, M.FilterSQL, M.Page = 0) as List<INSTANCE_CLASS>;
			});
		}

		/// <summary>
		/// Close
		/// </summary>
		private void btnClose_Click(object sender, MouseButtonEventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Select
		/// </summary>
		public INSTANCE_CLASS Selected;
		private void dgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				if (!M.SelectingMode)
				{
					if (Global.User.Perms.Contains($"{M.INSTANCE_TYPE}_{Global.UserPermType.SAVE}"))
						btnEdit_Click(null, null);
					else
						btnPreview_Click(null, null);
				}
				else
				{
					Selected = dgList.SelectedItems.Cast<INSTANCE_CLASS>().FirstOrDefault();
					DialogResult = true;
				}
			}
		}

		/// <summary>
		/// Load more
		/// </summary>
		private void dgList_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (e.VerticalChange > 0 && e.VerticalOffset + e.ViewportHeight == e.ExtentHeight && M.InstancesList.Count < M.TotalItems)
			{
				DataContext = null;
				M.InstancesList.AddRange(SQL.ListInstances(M.INSTANCE_TYPE, M.FilterSQL, ++M.Page) as List<INSTANCE_CLASS>);
				DataContext = M;
				Extensions.GetVisualChild<ScrollViewer>(sender as DataGrid).ScrollToVerticalOffset(e.VerticalOffset);
			}
		}
	}

	/// <summary>
	/// Model
	/// </summary>
	internal class M_EmployeesList : INotifyPropertyChanged
	{
		public readonly string INSTANCE_TYPE = Global.Module.EMPLOYEES;

		/// Dane o zalogowanym użytkowniku
		public C_User User { get; } = Global.User;
		/// Lista instancji
		private List<INSTANCE_CLASS> instancesList;
		public List<INSTANCE_CLASS> InstancesList
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
		/// Tryb wyboru dla okna
		public bool SelectingMode { get; set; }
		/// Filtr SQL
		public string FilterSQL { get; set; }
		/// Instancja filtra
		private INSTANCE_CLASS filters = new INSTANCE_CLASS();
		public INSTANCE_CLASS Filters
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
		/// Numer strony
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
		/// Łączna liczba instancji
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