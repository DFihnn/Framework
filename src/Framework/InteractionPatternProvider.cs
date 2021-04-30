using System.Collections;
using System.Collections.Generic;

namespace Framework
{
  public static class InteractionPatternProvider
  {
    public static TPattern GetPattern<TPattern>(object item) where TPattern : class
    {
      return item is IInteractionPatternProvider interactionPatternProvider ? interactionPatternProvider.GetPattern<TPattern>() : default(TPattern);
    }

    public static IEnumerable<TPattern> GetPatterns<TPattern>(IEnumerable sourceItems) where TPattern : class
    {
      if (sourceItems != null)
      {
        foreach (var sourceItem in sourceItems)
        {
          var pattern = GetPattern<TPattern>(sourceItem);
          if (pattern != null)
            yield return pattern;
        }
      }
    }
  }
}
