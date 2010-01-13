using System.Collections.Generic;

using Machine.Migrations.DatabaseProviders;

namespace Machine.Migrations.Services.Impl
{
  public class Migrator : IMigrator
  {
    #region Member Data
    readonly IMigrationSelector _migrationSelector;
    readonly IMigrationRunner _migrationRunner;
    readonly IWorkingDirectoryManager _workingDirectoryManager;
    #endregion

    #region Migrator()
    public Migrator(IMigrationSelector migrationSelector, IMigrationRunner migrationRunner, IWorkingDirectoryManager workingDirectoryManager)
    {
      _migrationSelector = migrationSelector;
      _workingDirectoryManager = workingDirectoryManager;
      _migrationRunner = migrationRunner;
    }
    #endregion

    #region IMigrator Members
    public void RunMigrator()
    {
      _workingDirectoryManager.Create();
      var steps = _migrationSelector.SelectMigrations();
      if (_migrationRunner.CanMigrate(steps))
      {
        _migrationRunner.Migrate(steps);
      }
    }
    #endregion
  }
}