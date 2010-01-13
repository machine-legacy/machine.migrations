using System;
using System.Collections.Generic;
using System.Data;

namespace Machine.Migrations.Services.Impl
{
  public abstract class AbstractConnectionProvider : IConnectionProvider
  {
    #region Logging
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(SqlServerConnectionProvider));
    #endregion

    #region Member Data
    readonly IConfiguration _configuration;
    readonly Dictionary<string, IDbConnection> _connections = new Dictionary<string, IDbConnection>();
    #endregion

    #region AbstractConnectionProvider()
    protected AbstractConnectionProvider(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    #endregion

    #region IConnectionProvider Members
    protected abstract IDbConnection CreateConnection(IConfiguration configuration, string key);

    public void OpenConnection()
    {
      var connection = this.CurrentConnection;
    }

    public IDbConnection CurrentConnection
    {
      get
      {
        var activeKey = _configuration.ActiveConfigurationKey;
        if (activeKey == null)
        {
          throw new InvalidOperationException("Can't get a connection w/o setting an active configuration key, this is a bug in!");
        }
        if (!_connections.ContainsKey(activeKey))
        {
          var connection = CreateConnection(_configuration, activeKey);
          connection.Open();
          _connections[activeKey] = connection;
        }
        return _connections[activeKey];
      }
    }
    #endregion
  }
}