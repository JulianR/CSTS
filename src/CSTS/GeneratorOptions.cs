using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTS
{
  public class GeneratorOptions
  {
    public GeneratorOptions()
    {
      this.CommentingOptions = CommentingOptions.Default;
    }

    public IEnumerable<Type> Types { get; set; }

    public Func<Type, bool> TypeFilter { get; set; }

    public Func<Type, bool> BaseTypeFilter { get; set; }

    public Func<Type, string> ModuleNameGenerator { get; set; }

    public CommentingOptions CommentingOptions { get; set; }
  }

  public class CommentingOptions
  {
    public bool RenderObsoleteAttributesAsComments { get; set; }

    public static CommentingOptions Default
    {
      get
      {
        return new CommentingOptions
        {
          RenderObsoleteAttributesAsComments = true
        };
      }
    }
  }
}
