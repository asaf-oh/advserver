namespace NHibernate.Test.Generatedkeys.ByTrigger
{
	public class MyEntity
	{
		public virtual int Id { get; private set; }

		public virtual string Name { get; set; }
	}
}