using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	class Review
	{
		internal Customer customer { get; set; }
		internal Game boardgame { get; set; }
		internal DateTime dateReviewed { get; set; }
		internal string content { get; set; }
		internal int rating { get; set; }

		public Review(Customer customer, Game boardgame)
		{
			this.customer = customer;
			this.boardgame = boardgame;
		}

		public static void AddFromSql(Object[] values)
		{
			Customer customer = Global.customers.Find(cust => cust.username == (string) values[0]);
			Game game = Global.games.Find(bg => bg.id == (int) values[1]);
			Review review = new Review(customer, game);
			review.dateReviewed = Global.StringToDate((string) values[2]);
			review.content = (string) values[3];
			review.rating = (int) values[4];
			Global.reviews.Add(review);
		}

		public override string ToString()
		{
			return customer + ";" + boardgame + ";" + dateReviewed.ToShortDateString();
		}
	}
}
