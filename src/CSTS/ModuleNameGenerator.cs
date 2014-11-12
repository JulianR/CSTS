using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS
{
  internal class ModuleNameGenerator
  {

    public string GetModuleName(ArrayType type)
    {
      return GetModuleName((dynamic)type.ElementType);
    }

    public string GetModuleName(TypeScriptType type)
    {
      return "";
    }

    public string GetModuleName(EnumType type)
    {
      return type.Module + ".";
    }

    public string GetModuleName(CustomType type)
    {
      return type.Module + ".";
    }

  }
}
