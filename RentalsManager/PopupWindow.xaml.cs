using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RentalsManager
{
	/// <summary>
	/// Interaction logic for PopupWindow.xaml
	/// </summary>
	public partial class PopupWindow : Window
	{
		Window parent;

		public PopupWindow(string content, Window parentWindow)
		{
			InitializeComponent();
			MainTextBlock.Text = content;
			CloseButton.Focus();
			parent = parentWindow;
		}

		private void CloseButton_KeyUp(object sender, KeyEventArgs e)
		{
			Close();
			parent.Show();
		}
	}
}
