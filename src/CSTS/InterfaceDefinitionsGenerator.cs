﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSTS
{

  internal class InterfaceDefinitionsGenerator
  {
    private IndentedStringBuilder _sb;
    private PropertyCommenter _propertyCommenter = new PropertyCommenter();
    private ModuleNameGenerator _moduleNameGenerator = new ModuleNameGenerator();
    private TypeNameGenerator _typeNameGenerator;
    private IEnumerable<TypeScriptModule> _modules;

    public InterfaceDefinitionsGenerator(IEnumerable<TypeScriptModule> modules)
    {
      _modules = modules;
      _sb = new IndentedStringBuilder(modules.Sum(m => m.ModuleMembers.Count) * 256);
    }

    public IEnumerable<TypeScriptModule> Modules
    {
      get
      {
        return _modules;
      }
    }

    public string Generate()
    {
      _typeNameGenerator = new TypeNameGenerator(this.Modules, _moduleNameGenerator);

      foreach (var module in _modules)
      {
        _sb.AppendLine("declare module {0} {{", module.Module);
        _sb.IncreaseIndentation();
        _sb.AppendLine("");

        foreach (var type in module.ModuleMembers)
        {
          Render((dynamic)type);
        }

        _sb.DecreaseIndentation();
        _sb.AppendLine("}}");
        _sb.AppendLine("");
      }

      return _sb.ToString();
    }

    private void Render(CustomType type)
    {
      _sb.AppendLine("interface {0}{1} {{", _typeNameGenerator.GetTypeName(type), RenderBaseType(type));
      _sb.IncreaseIndentation();

      foreach (var p in type.Properties)
      {
        Render(p);
      }

      _sb.DecreaseIndentation();
      _sb.AppendLine("}}");
      _sb.AppendLine("");
    }

    private void Render(TypeScriptProperty p)
    {
      _sb.AppendLine("{0}{3} : {1}{2}; {4}", p.Property.Name, _moduleNameGenerator.GetModuleName((dynamic)p.Type), _typeNameGenerator.GetTypeName((dynamic)p.Type), HandleOptional(p.Type), _propertyCommenter.GetPropertyComment(p));
    }

    private string HandleOptional(TypeScriptType typeScriptType)
    {
      var vt = typeScriptType as ValueType;

      if (vt != null)
      {
        return vt.IsNullable ? "?" : "";
      }

      return "";
    }

    private string RenderBaseType(CustomType type)
    {
      var baseTypes = new List<TypeScriptType>();

      if (type.BaseType != null)
      {
        baseTypes.Add(type.BaseType);
      }

      baseTypes.AddRange(type.ImplementedInterfaces);

      if (baseTypes.Count == 0)
      {
        return "";
      }

      var baseType = string.Format(" extends {0}", string.Join(", ", baseTypes.Select(b => string.Format("{0}{1}", _moduleNameGenerator.GetModuleName((dynamic)b), _typeNameGenerator.GetTypeName((dynamic)b)))));

      return baseType;
    }

    private void Render(EnumType type)
    {
      _sb.AppendLine("enum {0} {{", type.ClrType.Name);
      _sb.IncreaseIndentation();

      var values = Enum.GetValues(type.ClrType);
      var names = Enum.GetNames(type.ClrType);

      int i = 0;

      foreach (var val in values)
      {
        var name = names[i];
        i++;

        _sb.AppendLine("{0} = {1},", name, (int)val);
      }

      _sb.DecreaseIndentation();
      _sb.AppendLine("}}");
      _sb.AppendLine("");
    }
  }
}
