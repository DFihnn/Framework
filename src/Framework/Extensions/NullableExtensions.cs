namespace Framework.Extensions
{
  public static class NullableExtensions
  {
    public static bool IsTrue(this bool? nullableBool)
    {
      return nullableBool.HasValue && nullableBool.Value;
    }
  }
}
