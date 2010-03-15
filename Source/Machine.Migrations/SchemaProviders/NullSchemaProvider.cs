using System;
using System.Collections.Generic;

namespace Machine.Migrations.SchemaProviders
{
  public class NullSchemaProvider : ISchemaProvider
  {
    public void AddTable(string table, ICollection<Column> columns)
    {
      throw new NotImplementedException();
    }

    public void DropTable(string table)
    {
      throw new NotImplementedException();
    }

    public void RenameTable(string table, string newName)
    {
      throw new NotImplementedException();
    }

    public void AddColumn(string table, string column, Type type)
    {
      throw new NotImplementedException();
    }

    public void AddColumn(string table, string column, Type type, bool allowNull)
    {
      throw new NotImplementedException();
    }

    public void AddColumn(string table, string column, Type type, short size, bool isPrimaryKey, bool allowNull)
    {
      throw new NotImplementedException();
    }

    public void AddColumn(string table, string column, Type type, short size, bool allowNull)
    {
      throw new NotImplementedException();
    }

    public void RemoveColumn(string table, string column)
    {
      throw new NotImplementedException();
    }

    public void RenameColumn(string table, string column, string newName)
    {
      throw new NotImplementedException();
    }

    public void AddSchema(string name)
    {
      throw new NotImplementedException();
    }

    public void RemoveSchema(string name)
    {
      throw new NotImplementedException();
    }

    public bool HasTable(string table)
    {
      throw new NotImplementedException();
    }

    public bool HasColumn(string table, string column)
    {
      throw new NotImplementedException();
    }

    public bool HasSchema(string name)
    {
      throw new NotImplementedException();
    }

    public void ChangeColumn(string table, string column, Type type, short size, bool allowNull)
    {
      throw new NotImplementedException();
    }

    public string[] Columns(string table)
    {
      throw new NotImplementedException();
    }

    public string[] Tables()
    {
      throw new NotImplementedException();
    }

    public void AddForeignKeyConstraint(string table, string name, string column, string foreignTable, string foreignColumn)
    {
      throw new NotImplementedException();
    }

    public void AddUniqueConstraint(string table, string name, params string[] columns)
    {
      throw new NotImplementedException();
    }

    public void DropConstraint(string table, string name)
    {
      throw new NotImplementedException();
    }

    public bool IsColumnOfType(string table, string column, string type)
    {
      throw new NotImplementedException();
    }
  }
}
