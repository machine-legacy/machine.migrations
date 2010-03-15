using System;
using System.Collections.Generic;
using System.Data;
using Machine.Migrations.Services;
using Machine.Migrations.Services.Impl;
using MySql.Data.MySqlClient;

namespace Machine.Migrations.MySql
{
  public class MySqlConnectionProvider : AbstractConnectionProvider
  {
    public MySqlConnectionProvider(IConfiguration configuration)
      : base(configuration)
    {
    }

    protected override IDbConnection CreateConnection(IConfiguration configuration, string key)
    {
      return new MySqlConnection(configuration.ConnectionStringByKey(key));
    }
  }
}
