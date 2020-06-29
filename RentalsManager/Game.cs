using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	public class Game
	{
		public int id { get; set; }
		public string name { get; set; }
		public decimal price { get; set; }
		public double rating
		{
			get
			{
				object[] ratingSearchResult = SQL.GetOutput("SELECT AVG(rating) FROM Review WHERE boardgameID = " + this.id.ToString())[0];
				return (ratingSearchResult[0] is DBNull) ? 0.0 : (int) ratingSearchResult[0];
			}
		}
		public int minPlayers { get; set; }
		public int maxPlayers { get; set; }
		public int playTimeMins { get; set; }
		public string releaseYear { get; set; }
		public bool avail { get; set; }
		public string inStock
		{ get { return avail ? "Available" : "On loan"; } }

		public Game(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		public static void AddFromSql(object[] values)
		{
			int id = (int) values[0];
			string name = (string) values[1];
			Game game = new Game(id, name);
			game.price = values[2] is DBNull ? (decimal) 0.00 : (decimal) values[2];
			game.minPlayers = values[4] is DBNull ? 0 : (int) values[4];
			game.maxPlayers = values[5] is DBNull ? 0 : (int) values[5];
			game.playTimeMins = values[6] is DBNull ? 0 : (int) values[6];
			game.releaseYear = values[7] is DBNull ? null : (string) values[7];
			game.avail = (int)values[8] == 1 ? true : false;
			Global.games.Add(game);
		}

		internal List<Publisher> GetPublishers()
		{
			List<object[]> publishersRaw = SQL.GetOutput("SELECT publisherName FROM GamePublishedBy WHERE boardgameID = " + this.id);
			List<Publisher> thisGamePublishers = new List<Publisher>();
			foreach (object[] values in publishersRaw)
				foreach (Publisher publisher in Global.publishers)
					if (((string)values[0]).Equals(publisher.name)) thisGamePublishers.Add(publisher);
			return thisGamePublishers;
		}

		internal List<Designer> GetDesigners()
		{
			List<object[]> designersRaw = SQL.GetOutput("SELECT DesignerName FROM GameDesignedBy WHERE boardgameID = " + this.id);
			List<Designer> thisGameDesigners = new List<Designer>();
			foreach (object[] values in designersRaw)
				foreach (Designer designer in Global.designers)
					if (((string)values[0]).Equals(designer.name)) thisGameDesigners.Add(designer);
			return thisGameDesigners;
		}

		internal List<Genre> GetGenres()
		{
			List<object[]> genresRaw = SQL.GetOutput("SELECT genreName FROM GameGenreValue WHERE boardgameID = " + this.id);
			List<Genre> thisGameGenres = new List<Genre>();
			foreach (object[] values in genresRaw)
				foreach (Genre genre in Global.genres)
					if (((string) values[0]).Equals(genre.name)) thisGameGenres.Add(genre);
			return thisGameGenres;
		}
		public void UpdateSql()
		{
			SQL.ExecuteQuery("UPDATE Boardgame SET name = '" + name + "' WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET price = " + price + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET rating = " + rating + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET minPlayers = " + minPlayers + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET maxPlayers = " + maxPlayers + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET playTimeMins = " + playTimeMins + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET releaseYear = " + releaseYear + " WHERE id = " + id);
			SQL.ExecuteQuery("UPDATE Boardgame SET avail = " + (avail ? "1" : "0") + " WHERE id = " + id);

		}
		public void InsertSql()
		{
			SQL.ExecuteQuery("INSERT INTO Boardgame VALUES (" + id + ", '" + name + "', " + price + ", " + rating + ", " 
			                 + minPlayers + ", " + maxPlayers + ", " + playTimeMins + ", " + releaseYear + ", " + (avail ? "1" : "0") + ")");
		}

		public string ToShortString()
		{
			return name + ";" + id;
		}

		public override string ToString()
		{
			List<Publisher> publishers = GetPublishers();
			string publishersString = "";
			List<Designer> designers = GetDesigners();
			string designersString = "";
			foreach (Publisher publisher in publishers) publishersString += publisher + ";";
			foreach (Designer designer in designers) designersString += designer + ";";
			return name + ";" + id + ";" + publishersString + designersString;
		}
	}
}
