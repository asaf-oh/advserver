namespace advertiser
{
	public class Account
	{
		public Account (int id, string name, string password)
		{
			this.id = id;
			this.name = name;
			this.password = password;
		}
		

		private int id;
		public int Id
		{
			get { return Id; }
		}

		private string name;
		public string Name
		{
			get { return name; }
		}
		
		private string password;
		public string Password
		{
			get { return password; }
		}

	}
}

