using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	public class Review
	{
		public Customer customer { get; set; }
		public Game boardgame { get; set; }
		public DateTime dateReviewed { get; set; }
		public string content { get; set; }
		public int rating { get; set; }

		public string boardgameName
		{
			get { return boardgame.name; }
		}

		public string customerUsername
		{
			get { return customer.fname + " " + customer.lname; }
		}

		public string fullText
		{
			get { return "Rated " + rating + "/10 by " + customer.username + " on " + dateReviewed.ToShortDateString() + ":\n" + content; }
		}

		public Review(Customer customer, Game boardgame)
		{
			this.customer = customer;
			this.boardgame = boardgame;
			this.content = "";
		}

		public static void AddFromSql(Object[] values)
		{
			Customer customer = Global.customers.Find(cust => cust.username == (string) values[0]);
			Game game = Global.games.Find(bg => bg.id == (int) values[1]);
			Review review = new Review(customer, game);
			review.dateReviewed = (DateTime) values[2];
			review.content = (string) values[3];
			review.rating = (int) values[4];
			Global.reviews.Add(review);
		}

		public void UpdateSql()
		{
			SQL.ExecuteQuery("Update Review SET content = '" + content + "' WHERE customerUsername = '" + customer.username + "' AND boardgameID = " + boardgame.id);
			SQL.ExecuteQuery("Update Review SET rating = " + rating + " WHERE customerUsername = '" + customer.username + "' AND boardgameID = " + boardgame.id);
		}

		public void InsertSql()
		{
			SQL.ExecuteQuery("INSERT INTO Review VALUES ('" + customer.username + "', " + boardgame.id + ", '" + dateReviewed.ToShortDateString() + "', '" + content + "', " + rating + ")");
		}

		public override string ToString()
		{
			return customer + ";" + boardgame + ";" + dateReviewed.ToShortDateString();
		}
	}
}
