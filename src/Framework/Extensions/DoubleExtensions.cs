﻿namespace Framework.Extensions
{
  /// <summary>
  /// DoubleExtensions uses fixed eps to provide fuzzy comparison functionality for doubles.
  /// Note that FP noise is a big problem and using any of these compare
  /// methods is not a complete solution, but rather the way to reduce
  /// the probability of repeating unnecessary work.
  /// </summary>
  public static class DoubleExtensions
  {
    /// <summary>
    /// Epsilon - more or less random, more or less small number.
    /// </summary>
    private const double eps = 1.53E-06;

    /// <summary>
    /// AreClose returns whether or not two doubles are "close".  That is, whether or 
    /// not they are within epsilon of each other.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this 
    /// returns false. 
    /// </summary>
    /// <param name="value1">The first double to compare.</param>
    /// <param name="value2">The second double to compare.</param>
    /// <returns>The result of the AreClose comparision.</returns>
    public static bool AreClose(this double value1, double value2)
    {
      if (value1.IsNonreal() || value2.IsNonreal())
        return value1.CompareTo(value2) == 0;
      if (value1.Equals(value2))
        return true;
      var delta = value1 - value2;
      return delta < eps && delta > -eps;
    }

    /// <summary>
    /// LessThan returns whether or not the first double is less than the second double.
    /// That is, whether or not the first is strictly less than *and* not within epsilon of
    /// the other number.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this 
    /// returns false.
    /// </summary>
    /// <param name="value1">The first double to compare.</param>
    /// <param name="value2">The second double to compare.</param>
    /// <returns>The result of the LessThan comparision.</returns>
    public static bool LessThan(this double value1, double value2)
    {
      return value1 < value2 && !value1.AreClose(value2);
    }

    /// <summary>
    /// GreaterThan returns whether or not the first double is greater than the second double.
    /// That is, whether or not the first is strictly greater than *and* not within epsilon of
    /// the other number.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this 
    /// returns false.
    /// </summary>
    /// <param name="value1">The first double to compare.</param>
    /// <param name="value2">The second double to compare.</param>
    /// <returns>The result of the GreaterThan comparision.</returns>
    public static bool GreaterThan(this double value1, double value2)
    {
      return value1 > value2 && !value1.AreClose(value2);
    }

    /// <summary>
    /// LessThanOrClose returns whether or not the first double is less than or close to
    /// the second double.  That is, whether or not the first is strictly less than or within
    /// epsilon of the other number.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this 
    /// returns false.
    /// </summary>
    /// <param name="value1">The first double to compare.</param>
    /// <param name="value2">The second double to compare.</param>
    /// <returns>The result of the LessThanOrClose comparision.</returns>
    public static bool LessThanOrClose(this double value1, double value2)
    {
      return value1 < value2 || value1.AreClose(value2);
    }

    /// <summary>
    /// GreaterThanOrClose returns whether or not the first double is greater than or close to
    /// the second double.  That is, whether or not the first is strictly greater than or within
    /// epsilon of the other number.
    /// There are plenty of ways for this to return false even for numbers which
    /// are theoretically identical, so no code calling this should fail to work if this 
    /// returns false.
    /// </summary>
    /// <param name="value1">The first double to compare.</param>
    /// <param name="value2">The second double to compare.</param>
    /// <returns>The result of the GreaterThanOrClose comparision.</returns>
    public static bool GreaterThanOrClose(this double value1, double value2)
    {
      return value1 > value2 || value1.AreClose(value2);
    }

    /// <summary>
    /// Test to see if a double is a finite number (is not NaN or Infinity).
    /// </summary>
    /// <param name='value'>The value to test.</param>
    /// <returns>Whether or not the value is a finite number.</returns>
    public static bool IsFinite(this double value)
    {
      return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    /// <summary>
    /// Test to see if a double a valid size value (is finite and > 0).
    /// </summary>
    /// <param name='value'>The value to test.</param>
    /// <returns>Whether or not the value is a valid size value.</returns>
    public static bool IsValidSize(this double value)
    {
      return (IsFinite(value) && GreaterThanOrClose(value, 0));
    }

    public static bool IsNonreal(this double value)
    {
      return double.IsNaN(value) || double.IsInfinity(value);
    }
  }
}

