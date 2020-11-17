using System.Windows.Media;

namespace Framework.Windows.Extensions
{
  public static class ColorExtensions
  {
    public static Color ToMediaColor(this System.Drawing.Color color)
    {
      return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static System.Drawing.Color ToDrawingColor(this Color color)
    {
      return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
  }
}
