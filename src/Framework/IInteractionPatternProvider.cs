namespace Framework
{
  public interface IInteractionPatternProvider
  {
    TPattern GetPattern<TPattern>() where TPattern : class;
  }
}
