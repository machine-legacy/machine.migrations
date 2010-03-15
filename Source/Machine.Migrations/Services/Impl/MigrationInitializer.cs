using Machine.Migrations.Core;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationInitializer : IMigrationInitializer
  {
    readonly IConfiguration _configuration;
    readonly IDatabaseProvider _databaseProvider;
    readonly ISchemaProvider _schemaProvider;
    readonly ICommonTransformations _commonTransformations;
    readonly IConnectionProvider _connectionProvider;

    public MigrationInitializer(IConfiguration configuration, IDatabaseProvider databaseProvider, ISchemaProvider schemaProvider, ICommonTransformations commonTransformations, IConnectionProvider connectionProvider)
    {
      _configuration = configuration;
      _commonTransformations = commonTransformations;
      _databaseProvider = databaseProvider;
      _schemaProvider = schemaProvider;
      _connectionProvider = connectionProvider;
    }

    public void InitializeMigration(IDatabaseMigration migration)
    {
      migration.Initialize(new MigrationContext(_configuration, _databaseProvider, _schemaProvider, _commonTransformations, _connectionProvider));
    }
  }
}