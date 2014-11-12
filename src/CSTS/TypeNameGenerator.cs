﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSTS
{
  internal class TypeNameGenerator
  {
    private ModuleNameGenerator _moduleNameGenerator;

    private Dictionary<Type, string> _interfaceNamingOverride = new Dictionary<Type, string>();

    private static string NormalizeName(CustomType t)
    {
      string name;

      if (t.ClrType.IsGenericTypeDefinition)
      {
        name = TypeHelper.GetNameOfGenericType(t.ClrType);
      }
      else
      {
        name = t.ClrType.Name;
      }

      return t.Module + "." + name;
    }

    private static string NormalizeName(EnumType t)
    {
      return t.Module + "." + t.ClrType.Name;
    }

    public TypeNameGenerator(IEnumerable<TypeScriptModule> modules, ModuleNameGenerator moduleNameGenerator)
    {
      _moduleNameGenerator = moduleNameGenerator;

      var allTypes = (from m in modules
                      from t in m.ModuleMembers
                      select new
                      {
                        TypeName = NormalizeName((dynamic)t),
                        ModuleMember = t
                      }).GroupBy(x => x.TypeName)
                     .Select(x => new
                     {
                       TypeName = x.Key,
                       Interfaces = x.Select(y => y.ModuleMember).ToList()
                     })
                     .Where(x => x.Interfaces.Count > 1);

      foreach (var type in allTypes)
      {
        var genericType = type.Interfaces.OfType<CustomType>().SingleOrDefault(i => i.ClrType.IsGenericTypeDefinition);

        if (genericType != null)
        {
          _interfaceNamingOverride.Add(genericType.ClrType, TypeHelper.GetNameOfGenericType(genericType.ClrType) + "Generic");
        }
      }

    }

    public string GetTypeName(NumberType tst)
    {
      return "number";
    }

    public string GetTypeName(EnumType tst)
    {
      return tst.ClrType.Name;
    }

    public string GetTypeName(StringType tst)
    {
      return "string";
    }

    public string GetTypeName(DateTimeType tst)
    {
      return "string";
    }

    public string GetTypeName(BooleanType tst)
    {
      return "boolean";
    }

    public string GetTypeName(GenericTypeParameter tst)
    {
      return tst.ClrType.Name;
    }

    private string GetTypeName(DictionaryType tst)
    {
      return string.Format("{{ [ key : {2}{0} ] : {3}{1} }}",
        GetTypeName((dynamic)tst.ElementKeyType), GetTypeName((dynamic)tst.ElementValueType),
        _moduleNameGenerator.GetModuleName((dynamic)tst.ElementKeyType), _moduleNameGenerator.GetModuleName((dynamic)tst.ElementValueType));
    }

    public string GetTypeName(ArrayType tst)
    {
      return GetTypeName((dynamic)tst.ElementType) + "[]";
    }

    public string GetTypeName(TypeScriptType tst)
    {
      return "any";
    }

    private Regex _genericTypeReplacer = new Regex("`(\\d+)");

    public string GetTypeName(CustomType tst)
    {
      var type = tst.ClrType;

      string typeName;

      if (type.IsGenericTypeDefinition)
      {
        var genericParams = type.GetGenericArguments();

        string nameOverride;

        _interfaceNamingOverride.TryGetValue(type, out nameOverride);

        typeName = string.Format("{0}<{1}>", nameOverride ?? _genericTypeReplacer.Replace(type.Name, ""), string.Join(", ", genericParams.Select(p => p.Name)));
      }
      else if (type.IsGenericType)
      {
        var genericParams = tst.GenericArguments;

        string nameOverride;

        _interfaceNamingOverride.TryGetValue(type.GetGenericTypeDefinition(), out nameOverride);

        typeName = string.Format("{0}<{1}>", nameOverride ?? _genericTypeReplacer.Replace(type.Name, ""), string.Join(", ", genericParams.Select(p => _moduleNameGenerator.GetModuleName((dynamic)p) + GetTypeName((dynamic)p))));
      }
      else
      {
        typeName = type.Name;
      }

      if(tst.DeclaringType != null)
      {
        typeName = GetTypeName((dynamic)tst.DeclaringType) + typeName;
      }

      return typeName;
    }

  }
}
