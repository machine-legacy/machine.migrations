﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Machine.Migrations.Services.Impl
{
  public class MigrationFactoryChooser : IMigrationFactoryChooser
  {
    #region Member Data
    private readonly CSharpMigrationFactory _cSharpMigrationFactory;
    #endregion

    #region MigrationApplicatorChooser()
    public MigrationFactoryChooser(CSharpMigrationFactory cSharpMigrationFactory)
    {
      _cSharpMigrationFactory = cSharpMigrationFactory;
    }
    #endregion

    #region IMigrationApplicatorChooser Members
    public IMigrationFactory ChooseFactory(MigrationReference migrationReference)
    {
      string extension = Path.GetExtension(migrationReference.Path);
      if (extension.Equals(".cs"))
      {
        return _cSharpMigrationFactory;
      }
      throw new ArgumentException(migrationReference.Path);
    }
    #endregion
  }
}
