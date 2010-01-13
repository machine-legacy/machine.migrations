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
    string _currentConfigurationKey = string.Empty;
    #endregion

    #region AbstractConnectionProvider()
    protected AbstractConnectionProvider(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    #endregion

    #region IConnectionProvider Members
    protected abstract IDbConnection CreateConnection(IConfiguration configuration, string key);

    public void UseConfiguration(string key)
    {
      _currentConfigurationKey = key;
    }

    public void OpenConnection()
    {
      var connection = this.CurrentConnection;
    }

    public IDbConnection CurrentConnection
    {
      get
      {
        if (!_connections.ContainsKey(_currentConfigurationKey))
        {
          var connection = CreateConnection(_configuration, _currentConfigurationKey);
          connection.Open();
          _connections[_currentConfigurationKey] = connection;
        }
        return _connections[_currentConfigurationKey];
      }
    }
    #endregion
  }
}