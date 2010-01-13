using System.Collections.Generic;

namespace Machine.Migrations.Services
{
  public interface ISchemaStateManager
  {
    IEnumerable<long> GetAppliedMigrationVersions(string scope);
    void SetMigrationVersionUnapplied(long version, string scope);
    void SetMigrationVersionApplied(long version, string scope);
  }
}