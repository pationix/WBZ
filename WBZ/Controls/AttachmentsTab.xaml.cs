﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WBZ.Classes;
using WBZ.Helpers;

namespace WBZ.Controls
{
    /// <summary>
    /// Interaction logic for AttachmentsTab.xaml
    /// </summary>
    public partial class AttachmentsTab : UserControl
    {
        M_AttachmentsTab M = new M_AttachmentsTab();
        private string InstanceType;
        private int ID;

        public AttachmentsTab()
        {
            InitializeComponent();
            DataContext = M;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Window win = Window.GetWindow(this);

                if (ID != 0 && M.InstanceAttachments == null)
                {
                    M.InstanceAttachments = SQL.ListAttachments(InstanceType, ID);
                    win.Closed += UserControl_Closed;
                }

                dynamic d = win?.DataContext;
                if (d != null)
                {
                    InstanceType = (string)d.INSTANCE_TYPE;
                    ID = (int)d.InstanceInfo.ID;
                    M.EditMode = (bool)d.EditMode;
                }
            }
            catch { }
        }

        private void btnAttachmentAdd_Click(object sender, MouseButtonEventArgs e)
        {
            var window = new AttachmentsAdd();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
            {
                string filePath, fileName;
                byte[] file;
                if (string.IsNullOrEmpty(window.GetDrive))
                {
                    filePath = window.GetLink;
                    fileName = filePath.Split('/').Last();
                    using (WebClient client = new WebClient())
                    {
                        file = client.DownloadData(filePath);
                    }
                }
                else
                {
                    filePath = window.GetDrive;
                    fileName = Path.GetFileName(filePath);
                    file = File.ReadAllBytes(filePath);
                }
                SQL.SetAttachment(InstanceType, ID, fileName, file, filePath);
                M.InstanceAttachments = SQL.ListAttachments(InstanceType, ID);
            }
        }
        private void btnAttachmentRemove_Click(object sender, MouseButtonEventArgs e)
        {
            if (lbAttachments.SelectedIndex < 0)
                return;

            SQL.DeleteAttachment(M.InstanceAttachments[lbAttachments.SelectedIndex].ID);
            SQL.SetLog(Global.User.ID, InstanceType, ID, $"Usunięto załącznik: {M.InstanceAttachments[lbAttachments.SelectedIndex].Name}");

            M.InstanceAttachments = SQL.ListAttachments(InstanceType, ID);
        }
        private void lbAttachments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAttachments.SelectedIndex < 0)
                return;

            var selection = (sender as ListBox).SelectedItem as C_Attachment;
            if (selection.File == null)
                selection.File = SQL.GetAttachmentFile(selection.ID);

            File.WriteAllBytes(Path.Combine(Path.GetTempPath(), selection.Name), selection.File);

            wbFile.Source = new Uri(@"file:///" + Path.Combine(Path.GetTempPath(), selection.Name));
        }

        private void UserControl_Closed(object sender, EventArgs e)
        {
            try
            {
                if (M.InstanceAttachments != null)
                    foreach (var attachment in M.InstanceAttachments)
                        if (attachment.File != null)
                            File.Delete(Path.Combine(Path.GetTempPath(), attachment.Name));
            }
            catch { }
        }
    }

    /// <summary>
    /// Model
    /// </summary>
    internal class M_AttachmentsTab : INotifyPropertyChanged
    {
        /// Załączniki
        private List<C_Attachment> instanceAttachments;
        public List<C_Attachment> InstanceAttachments
        {
            get
            {
                return instanceAttachments;
            }
            set
            {
                instanceAttachments = value;
                NotifyPropertyChanged(MethodBase.GetCurrentMethod().Name.Substring(4));
            }
        }
        /// Czy okno jest w trybie edycji
		public bool EditMode { get; set; }

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