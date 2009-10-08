using System.Collections.Generic;
using System.Linq;

namespace Machine.Migrations.Core
{
  public class VersionState
  {
    readonly IEnumerable<long> _applied;
    readonly long _last;
    readonly long _desired;
    readonly string _scope;

    public IEnumerable<long> Applied
    {
      get { return _applied; }
    }

    public long Last
    {
      get { return _last; }
    }

    public long Desired
    {
      get { return _desired; }
    }

    public string Scope
    {
      get { return _scope; }
    }

    public bool IsReverting
    {
      get
      {
        foreach (short applied in _applied)
        {
          if (_desired < applied)
          {
            return true;
          }
        }
        return false;
      }
    }

    public VersionState(long last, long desired, IEnumerable<long> applied)
    {
      _applied = applied;
      _last = last;
      _desired = desired;
    }

    public VersionState(long last, long desired, IEnumerable<long> applied, string scope)
      : this(last, desired, applied)
    {
      _scope = scope;
    }

    public bool IsApplicable(MigrationReference migrationReference)
    {
      bool isApplied = _applied.Contains(migrationReference.Version);
      if (isApplied)
      {
        if (migrationReference.Version > _desired)
        {
          return true;
        }
        return false;
      }
      else
      {
        if (migrationReference.Version <= _desired)
        {
          return true;
        }
        return false;
      }
    }
  }
}