using System;
using System.Collections.Generic;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class SchemaStateManager : ISchemaStateManager
  {
    #region Logging
    static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(SchemaStateManager));
    #endregion

    #region Member Data
    readonly string TableName = "schema_info";
    readonly string IdColumnName = "id";
    readonly string VersionColumnName = "version";
    readonly string ScopeColumnName = "scope";
    readonly IDatabaseProvider _databaseProvider;
    readonly ISchemaProvider _schemaProvider;
    #endregion

    #region SchemaStateManager()
    public SchemaStateManager(IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider)
    {
      _databaseProvider = databaseProvider;
      _schemaProvider = schemaProvider;
    }
    #endregion

    #region ISchemaStateManager Members
    public void CheckSchemaInfoTable()
    {
      if (_schemaProvider.HasTable(TableName))
      {
        if (!_schemaProvider.HasColumn(TableName, ScopeColumnName))
        {
          _log.InfoFormat("Adding {0} column to {1}...", ScopeColumnName, TableName);
          _schemaProvider.AddColumn(TableName, ScopeColumnName, typeof(string), 25, false, true);
        }

        if (!_schemaProvider.IsColumnOfType(TableName, VersionColumnName, "bigint"))
        {
          _log.InfoFormat("Changing {0} column to {1}...", VersionColumnName, "bigint");
          _schemaProvider.ChangeColumn(TableName, VersionColumnName, typeof(Int64), 8, false);
        }

        return;
      }

      _log.InfoFormat("Creating {0}...", TableName);

      Column[] columns = new Column[]
      {
        new Column(IdColumnName, typeof(Int32), 4, true),
        new Column(VersionColumnName, typeof(Int64), 8, false),
        new Column(ScopeColumnName, typeof(string), 25, false, true)
      };
      _schemaProvider.AddTable(TableName, columns);
    }

    public IEnumerable<long> GetAppliedMigrationVersions(string scope)
    {
      CheckSchemaInfoTable();
      if (string.IsNullOrEmpty(scope))
      {
        return
          _databaseProvider.ExecuteScalarArray<Int64>(
            "SELECT CAST({1} AS BIGINT) FROM {0} WHERE {2} IS NULL ORDER BY {1}",
            TableName, VersionColumnName, ScopeColumnName);
      }
      else
      {
        return
          _databaseProvider.ExecuteScalarArray<Int64>(
            "SELECT CAST({1} AS BIGINT) FROM {0} WHERE {2} = '{3}' ORDER BY {1}",
            TableName, VersionColumnName, ScopeColumnName, scope);
      }
    }

    public void SetMigrationVersionUnapplied(long version, string scope)
    {
      if (string.IsNullOrEmpty(scope))
      {
        _databaseProvider.ExecuteNonQuery("DELETE FROM {0} WHERE {1} = {2} AND {3} IS NULL",
          TableName, VersionColumnName, version, ScopeColumnName);
      }
      else
      {
        _databaseProvider.ExecuteNonQuery("DELETE FROM {0} WHERE {1} = {2} AND {3} = '{4}'",
          TableName, VersionColumnName, version, ScopeColumnName, scope);
      }
    }

    public void SetMigrationVersionApplied(long version, string scope)
    {
      if (string.IsNullOrEmpty(scope))
      {
        _databaseProvider.ExecuteNonQuery("INSERT INTO {0} ({1}, {2}) VALUES ({3}, NULL)",
          TableName, VersionColumnName, ScopeColumnName, version);
      }
      else
      {
        _databaseProvider.ExecuteNonQuery("INSERT INTO {0} ({1}, {2}) VALUES ({3}, '{4}')",
          TableName, VersionColumnName, ScopeColumnName, version, scope);
      }
    }
    #endregion
  }
}