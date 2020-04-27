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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentalsManager
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{ InitializeComponent(); }

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text;
			string password = PasswordPwBox.Password;
			if (findManager(username, password) != -1)
			{
				Hide();
				new MainViewWindow();
			}
		}

		private int findManager(string username, string password)
		{
			Console.WriteLine("Finding manager record...");
			List<string> managerList = SQL.GetOutput("SELECT password FROM Manager WHERE username = '" + username + "'");
			if (managerList == null) new PopupWindow("Incorrect username and/or password entered.");
			
			char[] textSplitParam = {'\t'};
			foreach (string s in managerList) Console.WriteLine(s);
			foreach (string managerText in managerList)
			{
				string[] managerArray = managerText.Split(textSplitParam, 2);
				if (managerArray[0].Equals(username) && managerArray[1].Equals(password)) return 0;
			}
			return -1;
		}

		private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
		{ if (e.Key == Key.Enter) PasswordPwBox.Focus(); }

		private void GoToButtonClick(object sender, KeyEventArgs e)
		{ if (e.Key == Key.Enter) ButtonBase_OnClick(sender, e); }
	}

}
