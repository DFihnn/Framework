using System.Windows;
using Framework.Extensions;

namespace Framework.Windows.Extensions
{
  public static class LayoutExtensions
  {
    public static bool AreClose(this Rect rect1, Rect rect2)
    {
      return rect1.Location.AreClose(rect2.Location) && rect1.Size.AreClose(rect2.Size);
    }

    public static bool AreClose(this Size size1, Size size2)
    {
      return size1.Width.AreClose(size2.Width) && size1.Height.AreClose(size2.Height);
    }

    public static bool AreClose(this Point size1, Point size2)
    {
      return size1.X.AreClose(size2.X) && size1.Y.AreClose(size2.Y);
    }
  }
}
