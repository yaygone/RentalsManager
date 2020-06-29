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
	/// Interaction logic for ManagerDetailWindow.xaml
	/// </summary>
	public partial class ManagerDetailWindow : Window
	{
		Manager manager;
		public ManagerDetailWindow(Manager manager)
		{
			InitializeComponent();
			this.manager = manager;
			if (manager != null) PopulateFields();
			else PasswordBoxOldPassword.IsEnabled = false;
		}

		public void PopulateFields()
		{
			TextBoxUsername.Text = manager.username;
			TextBoxUsername.IsEnabled = false;
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (!PasswordBoxNewPassword.Password.Equals(PasswordBoxConfirmPassword.Password) || PasswordBoxConfirmPassword.Password.Equals("") || TextBoxUsername.Text.Equals(""))
			{
				MessageBox.Show("Details incorrect, please try again", "Error", MessageBoxButton.OK);
				return;
			}
			if (manager != null)
			{
				if (!PasswordBoxOldPassword.Password.Equals(manager.password))
				{
					MessageBox.Show("Details incorrect, please try again", "Error", MessageBoxButton.OK);
					return;
				}
				else
				{
					manager.password = PasswordBoxConfirmPassword.Password;
					SQL.ExecuteQuery("UPDATE Manager SET password = '" + manager.password + "' WHERE username = '" + manager.username + "'");
				}
			}
			else
			{
				List<object[]> result = SQL.GetOutput("SELECT * FROM Manager WHERE username = '" + TextBoxUsername.Text + "'");
				if (result.Count == 0)
				{
					MessageBox.Show("Username already taken", "Error", MessageBoxButton.OK);
					return;
				}
				Global.managers.Add(new Manager(TextBoxUsername.Text, PasswordBoxNewPassword.Password));
				SQL.ExecuteQuery("INSERT INTO Manager VALUES ('" + TextBoxUsername.Text + "', '" + PasswordBoxNewPassword.Password + "')");
			}
			Close();
		}
	}
}
