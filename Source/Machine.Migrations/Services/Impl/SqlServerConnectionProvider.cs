using System.Data;
using System.Data.SqlClient;

namespace Machine.Migrations.Services.Impl
{
  public class SqlServerConnectionProvider : AbstractConnectionProvider
  {
    #region SqlServerConnectionProvider()
    public SqlServerConnectionProvider(IConfiguration configuration)
      : base(configuration)
    {
    }
    #endregion

    #region IConnectionProvider Members
    protected override IDbConnection CreateConnection(IConfiguration configuration, string key)
    {
      return new SqlConnection(configuration.ConnectionStringByKey(key));
    }
    #endregion
  }
}