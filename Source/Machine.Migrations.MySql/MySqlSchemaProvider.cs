using System;
using System.Collections.Generic;
using System.Text;
using Machine.Migrations.Builders;
using Machine.Migrations.DatabaseProviders;
using Machine.Migrations.SchemaProviders;

namespace Machine.Migrations.MySql
{
  public class MySqlSchemaProvider : ISchemaProvider
  {
    readonly IDatabaseProvider _databaseProvider;

    public MySqlSchemaProvider(IDatabaseProvider databaseProvider)
    {
      _databaseProvider = databaseProvider;
    }

    public void AddTable(string table, ICollection<Column> columns)
    {
      if (columns.Count == 0)
      {
        throw new ArgumentException("columns");
      }
      var sb = new StringBuilder();
      sb.Append("CREATE TABLE ").Append(table).Append(" (");
      var first = true;
      foreach (var column in columns)
      {
        if (!first) sb.Append(",");
        sb.AppendLine().Append(ColumnToCreateTableSql(column));
        first = false;
      }

      foreach (var column in columns)
      {
        var sql = ColumnToConstraintsSql(table, column);
        if (sql != null)
        {
          sb.Append(",").AppendLine().Append(sql);
        }
      }

      sb.AppendLine().Append(")");
      _databaseProvider.ExecuteNonQuery(sb.ToString());
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
      return _databaseProvider.ExecuteScalar<Int32>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", table) > 0;
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

    string ColumnToCreateTableSql(Column column)
    {
      return String.Format("\"{0}\" {1} {2} {3}",
        column.Name,
        ToMsSqlType(column.ColumnType, column.Size),
        column.AllowNull ? "" : "NOT NULL",
        column.IsIdentity ? "IDENTITY(1, 1)" : "").Trim();
    }

    public virtual string ColumnToConstraintsSql(string tableName, Column column)
    {
      if (column.IsPrimaryKey)
      {
        return String.Format("CONSTRAINT PK_{0}_{1} PRIMARY KEY (\"{1}\")", SchemaUtils.Normalize(tableName), SchemaUtils.Normalize(column.Name));
      }
      else if (column.IsUnique)
      {
        return String.Format("CONSTRAINT UK_{0}_{1} UNIQUE (\"{1}\" ASC)", SchemaUtils.Normalize(tableName), SchemaUtils.Normalize(column.Name));
      }
      return null;
    }

    public virtual string ToMsSqlType(ColumnType type, int size)
    {
      switch (type)
      {
        case ColumnType.Int16:
        case ColumnType.Int32:
          return "INT";
        case ColumnType.Long:
          return "BIGINT";
        case ColumnType.Money:
          return "MONEY";
        case ColumnType.NVarChar:
          // 150 is completely arbitrary isn't it? -jlewalle
          return size == 0 ? "NVARCHAR(150)" : String.Format("NVARCHAR({0})", size);
        case ColumnType.Real:
          return "REAL";
        case ColumnType.Text:
          return "TEXT";
        case ColumnType.Binary:
          return "VARBINARY(MAX)";
        case ColumnType.Bool:
          return "BIT";
        case ColumnType.Char:
          return "CHAR(1)";
        case ColumnType.DateTime:
          return "DATETIME";
        case ColumnType.Decimal:
          return "DECIMAL";
        case ColumnType.Image:
          return "IMAGE";
        case ColumnType.Guid:
          return "UNIQUEIDENTIFIER";
      }

      throw new ArgumentException("type");
    }
  }
}
