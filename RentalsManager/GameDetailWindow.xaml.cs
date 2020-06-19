using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
	/// Interaction logic for GameDetailWindow.xaml
	/// </summary>
	public partial class GameDetailWindow : Window
	{
		Game game;
		List<Designer> designers = new List<Designer>();
		List<Publisher> publishers = new List<Publisher>();
		List<Genre> genres = new List<Genre>();
		public GameDetailWindow(Game game)
		{
			this.game = game;
			if (game != null) PopulateFields();
			InitializeComponent();
		}

		private void PopulateFields()
		{
			IdTextBlock.Text = game.id.ToString();
			TextBoxName.Text = game.name;
			TextBoxPrice.Text = game.price.ToString();
			TextBoxRating.Text = game.rating.ToString();
			TextBoxMinPlayers.Text = game.minPlayers.ToString();
			TextBoxMaxPlayers.Text = game.maxPlayers.ToString();
			TextBoxPlayTime.Text = game.playTimeMins.ToString();
			TextBoxYear.Text = game.releaseYear;
			CheckBoxAvail.IsChecked = game.avail;

			publishers = game.GetPublishers();
			foreach (Publisher publisher in publishers) TextBoxPub.Text += publisher + ";";
			designers = game.GetDesigners();
			foreach (Designer designer in designers) TextBoxDes.Text += designer + ";";
			genres = game.GetGenres();
			foreach (Genre genre in genres) TextBoxGenres.Text += genre + ";";
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				bool newRecord = (game == null);
				if (newRecord)
				{
					Random rand = new Random();
					int newId = rand.Next();
					while (SQL.GetOutput("SELECT * FROM Boardgame WHERE id = " + newId).Count != 0) newId = rand.Next();
					game = new Game(newId, "");
				}
				else game.id = int.Parse(IdTextBlock.Text);

				game.name = TextBoxName.Text;
				game.price = decimal.Parse(TextBoxPrice.Text);
				game.rating = double.Parse(TextBoxRating.Text);
				game.minPlayers = int.Parse(TextBoxMinPlayers.Text);
				game.maxPlayers = int.Parse(TextBoxMaxPlayers.Text);
				game.playTimeMins = int.Parse(TextBoxPlayTime.Text);
				game.releaseYear = TextBoxYear.Text;
				game.avail = (bool) CheckBoxAvail.IsChecked;

				if (newRecord) game.InsertSql();
				else game.UpdateSql();

				List<string> updatedDesRaw = new List<string>(TextBoxDes.Text.Split(new char[] {';'}));
				List<string> updatedPubRaw = new List<string>(TextBoxPub.Text.Split(new char[] {';'}));
				List<string> updatedGenreRaw = new List<string>(TextBoxGenres.Text.Split(new char[] {';'}));
				List<Designer> updatedDesigners = new List<Designer>();
				List<Publisher> updatedPublishers = new List<Publisher>();
				List<Genre> updatedGenres = new List<Genre>();

				// Find from updated designers those that are already on record
				for (int i = 0; i < updatedDesRaw.Count; i++)
				{
					string designerName = updatedDesRaw[i].Trim();
					foreach (Designer designer in Global.designers)
						if (designerName.Equals(designer.name))
						{
							updatedDesigners.Add(designer);
							updatedDesRaw.RemoveAt(i);
							break;
						}
				}
				// Only left in the list are designers that have not been seen before
				foreach (string s in updatedDesRaw)
				{
					Designer newDesigner = new Designer(s);
					updatedDesigners.Add(newDesigner);
					Global.designers.Add(newDesigner);
				}

				for (int i = 0; i < updatedPubRaw.Count; i++)
				{
					string publisherName = updatedPubRaw[i].Trim();
					foreach (Publisher publisher in Global.publishers)
						if (publisherName.Equals(publisher.name))
						{
							updatedPublishers.Add(publisher);
							updatedPubRaw.RemoveAt(i);
							break;
						}
				}
				foreach (string s in updatedPubRaw)
				{
					Publisher newPublisher = new Publisher(s);
					updatedPublishers.Add(newPublisher);
					Global.publishers.Add(newPublisher);
				}

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

				var addedDes = updatedDesigners.ToArray().Except(designers.ToArray());
				var removedDes = designers.ToArray().Except(updatedDesigners.ToArray());
				var addedPub = updatedPublishers.ToArray().Except(publishers.ToArray());
				var removedPub = publishers.ToArray().Except(updatedPublishers.ToArray());
				var addedGenres = updatedGenres.ToArray().Except(genres.ToArray());
				var removedGenres = genres.ToArray().Except(updatedGenres.ToArray());

				foreach (var s in addedDes)
					SQL.ExecuteQuery("INSERT INTO GameDesignedBy VALUES (" + game.id + ", '" + s + "')");
				foreach (var s in removedDes)
					SQL.ExecuteQuery("DELETE FROM GameDesignedBy WHERE boardgameID = " + game.id +
					                 " AND designerName = '" + s + "'");
				foreach (var s in addedPub)
					SQL.ExecuteQuery("INSERT INTO GamePublishedBy VALUES (" + game.id + ", '" + s + "')");
				foreach (var s in removedPub)
					SQL.ExecuteQuery("DELETE FROM GamePublishedBy WHERE boardgameID = " + game.id +
					                 " AND publisherName = '" + s + "'");
				foreach (var s in addedGenres)
					SQL.ExecuteQuery("INSERT INTO GameGenreValue VALUES (" + game.id + ", '" + s + "')");
				foreach (var s in removedGenres)
					SQL.ExecuteQuery("DELETE FROM GameGenreValue WHERE boardgameID = " + game.id +
					                 " AND genreName = '" + s + "'");

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
