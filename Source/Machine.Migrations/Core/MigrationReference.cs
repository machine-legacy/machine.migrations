using System;
using System.Collections.Generic;
using Machine.Core.Utility;

namespace Machine.Migrations
{
  public class MigrationReference
  {
    readonly string _configurationKey;
    long _version;
    string _name;
    string _path;
    Type _ref;

    public long Version
    {
      get { return _version; }
    }

    public string Name
    {
      get { return _name; }
    }

    public IEnumerable<string> Aliases
    {
      get
      {
        yield return _name;
        yield return StringHelpers.ToUnderscoreDelimited(_name);
      }
    }

    public string Path
    {
      get { return _path; }
    }

    public Type Reference
    {
      get { return _ref; }
      set { _ref = value; }
    }

    public string ConfigurationKey
    {
      get { return _configurationKey; }
    }

    public MigrationReference(long version, string name, string path)
      : this(version, name, path, string.Empty)
    {
    }

    public MigrationReference(long version, string name, string path, string configurationKey)
    {
      _version = version;
      _name = name;
      _path = path;
      _configurationKey = configurationKey;
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(_configurationKey))
        return String.Format("{0} {1}", _version, _name);
      return String.Format("{0} ({1}) {2}", _version, _configurationKey, _name);
    }
  }
}