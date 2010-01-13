using System;
using System.Data;

using Machine.Migrations.Services;

namespace Machine.Migrations.NHibernate.Services.Impl
{
  public class NHibernateConnectionProvider : IConnectionProvider
  {
    #region Logging
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(NHibernateConnectionProvider));
    #endregion

    #region Member Data
    readonly INHibernateSessionProvider _sessionProvider;
    #endregion

    #region NHibernateConnectionProvider()
    public NHibernateConnectionProvider(INHibernateSessionProvider sessionProvider)
    {
      _sessionProvider = sessionProvider;
    }
    #endregion

    #region IConnectionProvider Members
    public void OpenConnection()
    {
      var connection = this.CurrentConnection;
    }

    public void UseConfiguration(string key)
    {
      if (key != string.Empty)
      {
        throw new NotSupportedException("NH mode doesn't support multiple configurations.");
      }
    }

    public IDbConnection CurrentConnection
    {
      get { return _sessionProvider.FindCurrentSession().Connection; }
    }
    #endregion
  }
}