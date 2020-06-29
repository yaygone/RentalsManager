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
	/// Interaction logic for ReviewListWindow.xaml
	/// </summary>
	public partial class ReviewListWindow : Window
	{
		Game game;
		bool readOnly;
		public ReviewListWindow(Game game, bool readOnly)
		{
			this.game = game;
			this.readOnly = readOnly;
			InitializeComponent();
			List<Review> thisGameReviews = new List<Review>();
			foreach (Review review in Global.reviews)
				if (review.boardgame == game) thisGameReviews.Add(review);
			listViewContent.ItemsSource = thisGameReviews;
			TextBlockTitle.Text += game.name;
			ButtonEdit.IsEnabled = !readOnly;
			CollectionViewSource.GetDefaultView(listViewContent.ItemsSource).Refresh();
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			new ReviewDetailWindow(listViewContent.SelectedItem as Review, readOnly).ShowDialog();
		}
	}
}
