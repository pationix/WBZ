﻿using System.Collections.Specialized;
using WBZ.Globals;
using WBZ.Interfaces;
using MODULE_MODEL = WBZ.Models.M_Attachment;

namespace WBZ.Modules.Attachments
{
    class D_AttachmentsList : D_ModuleList<MODULE_MODEL>
    {
		/// Module
		public readonly string MODULE_NAME = Global.Module.ATTACHMENTS;
		public StringCollection SORTING = Properties.Settings.Default.sorting_AttachmentsList;
	}
}
