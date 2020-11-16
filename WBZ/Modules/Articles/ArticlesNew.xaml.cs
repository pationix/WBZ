﻿using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WBZ.Models;
using WBZ.Helpers;
using WBZ.Interfaces;
using MODULE_MODEL = WBZ.Models.C_Article;

namespace WBZ.Modules.Articles
{
	/// <summary>
	/// Interaction logic for ArticlesNew.xaml
	/// </summary>
	public partial class ArticlesNew : _ArticlesNew
	{
		D_ArticlesNew D = new D_ArticlesNew();

		public ArticlesNew(MODULE_MODEL instance, Commands.Type mode)
		{
			InitializeComponent();
			DataContext = D;

			if (instance != null)
				D.InstanceInfo = instance;
			D.Mode = mode;
		}

		/// <summary>
		/// Measures - CellEditEnding
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgMeasures_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			int index = e.Row.GetIndex();
			dgMeasures_Convert(index);
		}

		private async void dgMeasures_Convert(int index)
		{
			await Task.Delay(10);
			await Task.Run(() => {
				if (D.InstanceInfo.ID != 0 && index < D.InstanceInfo.Measures.Rows.Count)
				{
					double conv = Convert.IsDBNull(D.InstanceInfo.Measures.Rows[index]["converter"]) ? 1 : (double)D.InstanceInfo.Measures.Rows[index]["converter"];
					D.InstanceInfo.Measures.Rows[index]["amount"] = Convert.ToDouble(D.InstanceInfo.AmountRaw) / conv;
					D.InstanceInfo.Measures.Rows[index]["reserved"] = Convert.ToDouble(D.InstanceInfo.ReservedRaw) / conv;
				}
			});
		}

		/// <summary>
		/// Tab changed
		/// </summary>
		private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var tab = (e.AddedItems.Count > 0 ? e.AddedItems[0] : null) as TabItem;
			if (tab?.Name?.EndsWith("_Stores") == true)
			{
				if (D.InstanceInfo.ID != 0 && D.InstanceSources_Stores == null)
					D.InstanceSources_Stores = SQL.ListInstances<C_Store>(Global.Module.STORES, $"sa.article={D.InstanceInfo.ID}");
			}
			else if (tab?.Name?.EndsWith("_Documents") == true)
			{
				if (D.InstanceInfo.ID != 0 && D.InstanceSources_Documents == null)
					D.InstanceSources_Documents = SQL.ListInstances<C_Document>(Global.Module.DOCUMENTS, $"dp.article={D.InstanceInfo.ID}");
			}
			else if (tab?.Name?.EndsWith("_Distributions") == true)
			{
				if (D.InstanceInfo.ID != 0 && D.InstanceSources_Distributions == null)
					D.InstanceSources_Distributions = SQL.ListInstances<C_Distribution>(Global.Module.DISTRIBUTIONS, $"dp.article={D.InstanceInfo.ID}");
			}
		}

		/// <summary>
		/// Open: Store
		/// </summary>
		private void dgList_Stores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			dgList_Module_MouseDoubleClick<C_Store>(sender, e, Global.Module.STORES);
		}

		/// <summary>
		/// Open: Document
		/// </summary>
		private void dgList_Documents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			dgList_Module_MouseDoubleClick<C_Document>(sender, e, Global.Module.DOCUMENTS);
		}

		/// <summary>
		/// Open: Distribution
		/// </summary>
		private void dgList_Distributions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			dgList_Module_MouseDoubleClick<C_Distribution>(sender, e, Global.Module.ARTICLES);
		}
	}

	public class _ArticlesNew : ModuleNew<MODULE_MODEL> { }
}
