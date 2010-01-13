using System.Collections.Generic;

namespace Machine.Migrations.Services
{
  public interface IMigrationRunner
  {
    bool CanMigrate(IDictionary<string, List<MigrationStep>> steps);
    void Migrate(IDictionary<string, List<MigrationStep>> steps);
  }
}