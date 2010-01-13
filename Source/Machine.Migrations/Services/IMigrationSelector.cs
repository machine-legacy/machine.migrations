using System.Collections.Generic;

namespace Machine.Migrations.Services
{
  public interface IMigrationSelector
  {
    IDictionary<string, List<MigrationStep>> SelectMigrations();
  }
}