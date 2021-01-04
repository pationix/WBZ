﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WBZ.Controls
{
    /// <summary>
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : StackPanel
	{
		public Header()
		{
			InitializeComponent();
		}

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(pIcon); }
            set { SetValue(pIcon, value); }
        }
        public static readonly DependencyProperty pIcon
            = DependencyProperty.Register(
                  nameof(Icon),
                  typeof(ImageSource),
                  typeof(Header),
                  new PropertyMetadata(null)
              );

        public string Text
        {
            get { return (string)GetValue(pText); }
            set { SetValue(pText, value); }
        }
        public static readonly DependencyProperty pText
            = DependencyProperty.Register(
                  nameof(Text),
                  typeof(string),
                  typeof(Header),
                  new PropertyMetadata(string.Empty)
              );
    }
}
