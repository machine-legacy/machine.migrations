using System.Data;
using System.Data.SqlClient;

namespace Machine.Migrations.Services.Impl
{
  public class SqlServerConnectionProvider : AbstractConnectionProvider
  {
    public SqlServerConnectionProvider(IConfiguration configuration)
      : base(configuration)
    {
    }

    protected override IDbConnection CreateConnection(IConfiguration configuration, string key)
    {
      return new SqlConnection(configuration.ConnectionStringByKey(key));
    }
  }
}