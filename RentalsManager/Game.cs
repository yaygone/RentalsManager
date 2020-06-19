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
		internal int id { get; set; }
		internal string name { get; set; }
		internal decimal price { get; set; }
		internal double rating { get; set; }
		internal int minPlayers { get; set; }
		internal int maxPlayers { get; set; }
		internal int playTimeMins { get; set; }
		internal string releaseYear { get; set; }
		internal bool avail { get; set; }

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
			game.price = (decimal) values[2];
			game.rating = (double) values[3];
			game.minPlayers = (int) values[4];
			game.maxPlayers = (int) values[5];
			game.playTimeMins = (int) values[6];
			game.releaseYear = (string) values[7];
			game.avail = (bool) values[8];
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
			SQL.ExecuteQuery("UPDATE Boardgame SET avail = " + (avail ? "1" : "0") + "' WHERE id = " + id);
		}
		public void InsertSql()
		{
			SQL.ExecuteQuery("INSERT INTO Boardgame VALUES (" + id + ", '" + name + "', " + price + ", " + rating + ", " 
			                 + minPlayers + ", " + maxPlayers + ", " + playTimeMins + ", " + releaseYear + ", " + (avail ? "1" : "0") + ")");
		}

		public override string ToString()
		{
			List<Publisher> publishers = GetPublishers();
			string publishersString = "";
			List<Designer> designers = GetDesigners();
			string designersString = "";
			foreach (Publisher publisher in publishers) publishersString += publisher + ";";
			foreach (Designer designer in designers) designersString += designer + ";";
			return name + ";" + id + ";" + publishersString + designersString ;
		}
	}
}
