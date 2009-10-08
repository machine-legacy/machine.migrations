using System.Collections.Generic;

namespace Machine.Migrations.Services
{
  public interface ISchemaStateManager
  {
    void CheckSchemaInfoTable();
    IEnumerable<long> GetAppliedMigrationVersions(string scope);
    void SetMigrationVersionUnapplied(long version, string scope);
    void SetMigrationVersionApplied(long version, string scope);
  }
}