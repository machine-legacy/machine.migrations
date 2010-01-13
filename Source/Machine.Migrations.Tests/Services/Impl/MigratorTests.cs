using System;
using System.Collections.Generic;
using System.Data;

using Machine.Core;
using Machine.Migrations.DatabaseProviders;

using NUnit.Framework;

using Rhino.Mocks;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class MigratorTests : StandardFixture<Migrator>
  {
    IDatabaseProvider _databaseProvider;
    IMigrationSelector _migrationSelector;
    IMigrationRunner _migrationRunner;
    ISchemaStateManager _schemaStateManager;
    IWorkingDirectoryManager _workingDirectoryManager;
    Dictionary<string, List<MigrationStep>> _steps;

    public override Migrator Create()
    {
      _steps = new Dictionary<string, List<MigrationStep>>();
      _databaseProvider = _mocks.DynamicMock<IDatabaseProvider>();
      _migrationSelector = _mocks.DynamicMock<IMigrationSelector>();
      _schemaStateManager = _mocks.DynamicMock<ISchemaStateManager>();
      _migrationRunner = _mocks.StrictMock<IMigrationRunner>();
      _workingDirectoryManager = _mocks.StrictMock<IWorkingDirectoryManager>();
      return new Migrator(_migrationSelector, _migrationRunner, _workingDirectoryManager);
    }

    [Test]
    public void RunMigrator_CanMigrate_RunsMigrations()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_migrationSelector.SelectMigrations()).Return(_steps);
        _workingDirectoryManager.Create();
        SetupResult.For(_migrationRunner.CanMigrate(_steps)).Return(true);
        _migrationRunner.Migrate(_steps);
      }
      _target.RunMigrator();
      _mocks.VerifyAll();
    }

    [Test]
    public void RunMigrator_CantMigrate_DoesNotRunMigrations()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_migrationSelector.SelectMigrations()).Return(_steps);
        _workingDirectoryManager.Create();
        SetupResult.For(_migrationRunner.CanMigrate(_steps)).Return(false);
      }
      _target.RunMigrator();
      _mocks.VerifyAll();
    }
  }
}