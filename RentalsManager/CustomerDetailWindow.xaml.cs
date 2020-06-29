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
	/// Interaction logic for CustomerDetailWindow.xaml
	/// </summary>
	public partial class CustomerDetailWindow : Window
	{
		Customer customer;
		List<Genre> genres;
		bool isCustomer;
		public CustomerDetailWindow(Customer customer, bool isCustomer)
		{
			InitializeComponent();
			this.customer = customer;
			this.isCustomer = isCustomer;
			if (customer != null) PopulateFields(isCustomer);
		}

		private void PopulateFields(bool isCustomer)
		{
			IdTextBlock.Text = "Edit customer";
			TextBoxFname.Text = customer.fname;
			TextBoxLname.Text = customer.lname;
			TextBoxPhone.Text = customer.phone;
			TextBoxEmail.Text = customer.email;
			TextBoxStreetAddress.Text = customer.streetAddress;
			TextBoxCity.Text = customer.city;
			TextBoxBankAcct.Text = customer.bankAccount;
			TextBoxStanding.Text = isCustomer ? "" : customer.goodness.ToString();
			DatePickerJoinDate.SelectedDate = customer.joinDate;
			DatePickerDob.SelectedDate = customer.dob;
			TextBoxGender.Text = customer.gender;
			TextBoxMaxRent.Text = customer.maxRentNum.ToString();
			TextBoxUsername.Text = customer.username;
			TextBoxUsername.IsEnabled = false;
			PasswordBoxPassword.Password = customer.password;

			genres = customer.GetGenres();
			foreach (Genre genre in genres) TextBoxGenrePref.Text += genre + ";";

			if (isCustomer)
			{
				TextBoxStanding.IsEnabled = false;
				DatePickerJoinDate.IsEnabled = false;
				TextBoxMaxRent.IsEnabled = false;
				TextBoxUsername.IsEnabled = false;
			}
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (TextBoxUsername.Text.Equals("") || PasswordBoxPassword.Password.Equals("") ||
			    TextBoxFname.Text.Equals("") || TextBoxPhone.Text.Equals("") || TextBoxBankAcct.Text.Equals(""))
			{
				MessageBox.Show("Essential information missing", "Error", MessageBoxButton.OK);
				return;
			}

			bool newRecord = (customer == null);

			if (newRecord)
			{
				if (SQL.GetOutput("SELECT * FROM Customer WHERE username = " + TextBoxUsername.Text).Count != 0)
				{
					MessageBox.Show("That username is already taken!", "", MessageBoxButton.OK);
					return;
				}
				customer = new Customer("", "", "", "", "", "", DateTime.Now);
			}

			try
			{
				customer.username = TextBoxUsername.Text;
				customer.password = PasswordBoxPassword.Password;
				customer.fname = TextBoxFname.Text;
				customer.lname = TextBoxLname.Text;
				customer.phone = TextBoxPhone.Text;
				customer.email = TextBoxEmail.Text;
				customer.streetAddress = TextBoxStreetAddress.Text;
				customer.city = TextBoxCity.Text;
				customer.bankAccount = TextBoxBankAcct.Text;
				customer.goodness = isCustomer ? customer.goodness : int.Parse(TextBoxStanding.Text);
				customer.joinDate = (DateTime) DatePickerJoinDate.SelectedDate;
				customer.dob = (DateTime) DatePickerDob.SelectedDate;
				customer.gender = TextBoxGender.Text;
				customer.maxRentNum = int.Parse(TextBoxMaxRent.Text);

				if (newRecord) customer.InsertSql();
				else customer.UpdateSql();

				if (!TextBoxGenrePref.Text.Equals(""))
				{
					List<string> updatedGenreRaw = new List<string>(TextBoxGenrePref.Text.Split(new char[] { ';' }));
					List<Genre> updatedGenres = new List<Genre>();
					for (int i = 0; i < updatedGenreRaw.Count; i++)
					{
						string genreName = updatedGenreRaw[i];
						foreach (Genre genre in Global.genres)
							if (genreName.Equals(genre.name))
							{
								updatedGenres.Add(genre);
								updatedGenreRaw.RemoveAt(i);
							}
					}
					foreach (string s in updatedGenreRaw)
					{
						Genre newGenre = new Genre(s);
						updatedGenres.Add(newGenre);
						Global.genres.Add(newGenre);
					}

					var addedGenres = updatedGenres.ToArray().Except(genres.ToArray());
					var removedGenres = genres.ToArray().Except(updatedGenres.ToArray());

					foreach (var s in addedGenres)
						SQL.ExecuteQuery("INSERT INTO GenrePref VALUES (" + customer.username + ", '" + s + "')");
					foreach (var s in removedGenres)
						SQL.ExecuteQuery("DELETE FROM GenrePref WHERE customerUsername = " + customer.username +
										 " AND genreName = '" + s + "'");
				}

				Close();
			}
			catch (FormatException exception)
			{
				MessageBox.Show("Error processing inputs. Please check that no letters are present in numeric field", "Error", MessageBoxButton.OK);
				Console.WriteLine(exception);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.StackTrace);
			}


		}
	}
}
