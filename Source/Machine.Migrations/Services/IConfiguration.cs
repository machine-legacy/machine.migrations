using System;

namespace Machine.Migrations.Services
{
  public interface IConfiguration
  {
    string Scope { get; }

    Type ConnectionProviderType { get; }

    Type TransactionProviderType { get; }

    Type SchemaProviderType { get; }

    Type DatabaseProviderType { get; }

    string ConnectionStringByKey(string key);

    string ActiveConfigurationKey { get; set; }

    string MigrationsDirectory { get; }

    string CompilerVersion { get; }

    long DesiredVersion { get; }

    bool ShowDiagnostics { get; }

    string[] References { get; }

    int CommandTimeout { get; }

    void SetCommandTimeout(int commandTimeout);
  }
}