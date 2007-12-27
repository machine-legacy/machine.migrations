using System;
using System.Collections.Generic;

using Machine.Core;

using NUnit.Framework;
using Rhino.Mocks;

namespace Machine.Migrations.Services.Impl
{
  [TestFixture]
  public class MigrationSelectorTests : StandardFixture<MigrationSelector>
  {
    private ISchemaStateManager _schemaStateManager;
    private IMigrationFinder _migrationFinder;
    private List<Migration> _migrations;

    [Test]
    public void SelectMigrations_VersionIsZero_IsAll()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_schemaStateManager.GetVersion()).Return((short)0);
      }
      CollectionAssert.AreEqual(_migrations, new List<Migration>(_target.SelectMigrations()));
      _mocks.VerifyAll();
    }

    [Test]
    public void SelectMigrations_VersionIsUpToDate_IsNothing()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_schemaStateManager.GetVersion()).Return((short)4);
      }
      CollectionAssert.IsEmpty(new List<Migration>(_target.SelectMigrations()));
      _mocks.VerifyAll();
    }

    [Test]
    public void SelectMigrations_VersionIsOld_ContainsRightVersions()
    {
      using (_mocks.Record())
      {
        SetupResult.For(_schemaStateManager.GetVersion()).Return((short)2);
      }
      CollectionAssert.AreEqual(new Migration[] { _migrations[2], _migrations[3] }, new List<Migration>(_target.SelectMigrations()));
      _mocks.VerifyAll();
    }

    public override MigrationSelector Create()
    {
      _schemaStateManager = _mocks.DynamicMock<ISchemaStateManager>();
      _migrationFinder = _mocks.DynamicMock<IMigrationFinder>();
      _migrations = new List<Migration>();
      _migrations.Add(new Migration(1, "A", "001_a.cs"));
      _migrations.Add(new Migration(2, "B", "001_b.cs"));
      _migrations.Add(new Migration(3, "C", "001_c.cs"));
      _migrations.Add(new Migration(4, "D", "001_d.cs"));
      SetupResult.For(_migrationFinder.FindMigrations()).Return(_migrations);
      return new MigrationSelector(_schemaStateManager, _migrationFinder);
    }
  }
}