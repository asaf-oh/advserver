using System;


namespace advertiser
{
	public class NHSessionMock : NHibernate.ISession
	{
		#region ISession implementation
		void NHibernate.ISession.Flush ()
		{
			throw new System.NotImplementedException();
		}
		
		
		System.Data.IDbConnection NHibernate.ISession.Disconnect ()
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Reconnect ()
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Reconnect (System.Data.IDbConnection connection)
		{
			throw new System.NotImplementedException();
		}
		
		
		System.Data.IDbConnection NHibernate.ISession.Close ()
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.CancelQuery ()
		{
			throw new System.NotImplementedException();
		}
		
		
		bool NHibernate.ISession.IsDirty ()
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.GetIdentifier (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		bool NHibernate.ISession.Contains (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Evict (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Load (Type theType, object id, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Load (string entityName, object id, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Load (Type theType, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		T NHibernate.ISession.Load<T> (object id, NHibernate.LockMode lockMode)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		T NHibernate.ISession.Load<T> (object id)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Load (string entityName, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Load (object obj, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Replicate (object obj, NHibernate.ReplicationMode replicationMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Replicate (string entityName, object obj, NHibernate.ReplicationMode replicationMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Save (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Save (object obj, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Save (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.SaveOrUpdate (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.SaveOrUpdate (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Update (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Update (object obj, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Update (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Merge (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Merge (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Persist (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Persist (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.SaveOrUpdateCopy (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.SaveOrUpdateCopy (object obj, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Delete (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Delete (string entityName, object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		int NHibernate.ISession.Delete (string query)
		{
			throw new System.NotImplementedException();
		}
		
		
		int NHibernate.ISession.Delete (string query, object value, NHibernate.Type.IType type)
		{
			throw new System.NotImplementedException();
		}
		
		
		int NHibernate.ISession.Delete (string query, object[] values, NHibernate.Type.IType[] types)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Lock (object obj, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Lock (string entityName, object obj, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Refresh (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Refresh (object obj, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.LockMode NHibernate.ISession.GetCurrentLockMode (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ITransaction NHibernate.ISession.BeginTransaction ()
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ITransaction NHibernate.ISession.BeginTransaction (System.Data.IsolationLevel isolationLevel)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria<T> ()
		
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria<T> (string alias)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria (Type persistentClass)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria (Type persistentClass, string alias)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria (string entityName)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ICriteria NHibernate.ISession.CreateCriteria (string entityName, string alias)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQueryOver<T, T> NHibernate.ISession.QueryOver<T> ()
		
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQueryOver<T, T> NHibernate.ISession.QueryOver<T> (System.Linq.Expressions.Expression<Func<T>> alias)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQuery NHibernate.ISession.CreateQuery (string queryString)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQuery NHibernate.ISession.CreateQuery (NHibernate.IQueryExpression queryExpression)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQuery NHibernate.ISession.CreateFilter (object collection, string queryString)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IQuery NHibernate.ISession.GetNamedQuery (string queryName)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ISQLQuery NHibernate.ISession.CreateSQLQuery (string queryString)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.Clear ()
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Get (Type clazz, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Get (Type clazz, object id, NHibernate.LockMode lockMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		object NHibernate.ISession.Get (string entityName, object id)
		{
			throw new System.NotImplementedException();
		}
		
		
		T NHibernate.ISession.Get<T> (object id)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		T NHibernate.ISession.Get<T> (object id, NHibernate.LockMode lockMode)
		
		{
			throw new System.NotImplementedException();
		}
		
		
		string NHibernate.ISession.GetEntityName (object obj)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IFilter NHibernate.ISession.EnableFilter (string filterName)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IFilter NHibernate.ISession.GetEnabledFilter (string filterName)
		{
			throw new System.NotImplementedException();
		}
		
		
		void NHibernate.ISession.DisableFilter (string filterName)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IMultiQuery NHibernate.ISession.CreateMultiQuery ()
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ISession NHibernate.ISession.SetBatchSize (int batchSize)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.Engine.ISessionImplementor NHibernate.ISession.GetSessionImplementation ()
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.IMultiCriteria NHibernate.ISession.CreateMultiCriteria ()
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.ISession NHibernate.ISession.GetSession (NHibernate.EntityMode entityMode)
		{
			throw new System.NotImplementedException();
		}
		
		
		NHibernate.EntityMode NHibernate.ISession.ActiveEntityMode {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		NHibernate.FlushMode NHibernate.ISession.FlushMode {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
		
		NHibernate.CacheMode NHibernate.ISession.CacheMode {
			get {
				throw new System.NotImplementedException();
			}
			set {
				throw new System.NotImplementedException();
			}
		}
		
		
		NHibernate.ISessionFactory NHibernate.ISession.SessionFactory {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		System.Data.IDbConnection NHibernate.ISession.Connection {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		bool NHibernate.ISession.IsOpen {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		bool NHibernate.ISession.IsConnected {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		NHibernate.ITransaction NHibernate.ISession.Transaction {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		
		NHibernate.Stat.ISessionStatistics NHibernate.ISession.Statistics {
			get {
				throw new System.NotImplementedException();
			}
		}
		
		#endregion
		public NHSessionMock ()
		{
		}
		#region IDisposable implementation
		void IDisposable.Dispose ()
		{
			throw new System.NotImplementedException();
		}
		
		#endregion
	}
}

