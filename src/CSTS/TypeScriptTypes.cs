﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSTS
{
  internal class TypeScriptType
  {
    public Type ClrType { get; set; }

    public override string ToString()
    {
      return ClrType != null ? this.GetType().Name + " : " + ClrType.ToString() : base.ToString();
    }
  }

  interface IModuleMember
  {
    string Module { get; set; }
  }

  internal class TypeScriptModule
  {
    public IList<IModuleMember> ModuleMembers { get; set; }

    public string Module { get; set; }
  }

  internal class GenericTypeParameter : TypeScriptType
  {

  }

  internal class ValueType : TypeScriptType
  {
    public bool IsNullable { get; set; }
  }

  internal class NumberType : ValueType
  {

  }

  internal class StringType : TypeScriptType
  {

  }

  internal class BooleanType : ValueType
  {

  }

  internal class DateTimeType : ValueType
  {

  }

  internal class TimeSpanType : ValueType
  {

  }

  internal class DictionaryType : TypeScriptType
  {
    public TypeScriptType ElementKeyType { get; set; }

    public TypeScriptType ElementValueType { get; set; }
  }

  internal class ArrayType : TypeScriptType
  {
    public TypeScriptType ElementType { get; set; }
  }

  internal class EnumType : ValueType, IModuleMember
  {
    public string Module { get; set; }
  }

  internal class TypeScriptProperty
  {
    public TypeScriptType Type { get; set; }
    public PropertyInfo Property { get; set; }

    public override string ToString()
    {
      return Property.ToString() + " - " + Type.GetType().ToString();
    }
  }

  internal class CustomType : TypeScriptType, IModuleMember
  {
    public CustomType(Type t)
    {
      this.ClrType = t;
      this.Properties = new List<TypeScriptProperty>();
      this.ImplementedInterfaces = new List<TypeScriptType>();
    }

    public IList<TypeScriptProperty> Properties { get; set; }

    public TypeScriptType BaseType { get; set; }

    public string Module { get; set; }

    public IList<TypeScriptType> GenericArguments { get; set; }

    public bool IncludeInheritedProperties { get; set; }

    public TypeScriptType DeclaringType { get; set; }

    public IList<TypeScriptType> ImplementedInterfaces { get; set; }
  }

  internal class InterfaceType : CustomType
  {
    public InterfaceType(Type t) : base(t) { }
  }

  internal class AnyType : TypeScriptType
  {

  }
}
