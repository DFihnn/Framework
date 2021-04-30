using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Framework.Extensions;

namespace Framework.Windows.Extensions
{
  public static class UIElementExtensions
  {
    ///// <summary>
    ///// Returns true if this UI element is the old focus and the new focus is the context menu that belongs to it.
    ///// </summary>
    public static bool FocusLostToOwnContextMenu(this UIElement uiElement, KeyboardFocusChangedEventArgs args)
    {
      if (args == null)
        return false;

      var placementTarget = args.NewFocus is ContextMenu contextMenu ? contextMenu.PlacementTarget: null;

      return uiElement == args.OldFocus && uiElement == placementTarget;
    }

    ///// <summary>
    ///// Returns true if this UI element is the new focus and the old focus is its context menu (or a menu item in it).
    ///// </summary>
    public static bool GotFocusFromOwnContextMenu(this UIElement uiElement, KeyboardFocusChangedEventArgs args)
    {
      if (args == null)
        return false;

      var contextMenu = args.OldFocus is MenuItem menuItem ? menuItem.Parent as ContextMenu : args.OldFocus as ContextMenu;
      var placementTarget = contextMenu?.PlacementTarget;

      return uiElement == args.NewFocus && uiElement == placementTarget;
    }

    public static bool IsClipped(this UIElement element)
    {
      var ancestor = element.FindAncestorOfType<ScrollContentPresenter>();
      if (ancestor == null)
        return false;
      var rect = element.TransformToAncestor(ancestor).TransformBounds(new Rect(0.0, 0.0, element.RenderSize.Width, element.RenderSize.Height));
      var renderSize = ancestor.RenderSize;
      var width = renderSize.Width;
      renderSize = ancestor.RenderSize;
      var height = renderSize.Height;
      var rect2 = new Rect(0.0, 0.0, width, height);
      rect2.Intersect(rect);
      return !rect.AreClose(rect2);
    }

    public static bool IsTrimmed(this UIElement element)
    {
      return element is TextBlock textBlock && textBlock.IsTextTrimmed();
    }

    public static bool IsTextTrimmed(this TextBlock textBlock)
    {
      if (textBlock.TextTrimming == TextTrimming.None)
        return false;
      var textFormattingMode = TextOptions.GetTextFormattingMode(textBlock);
      var typeface = new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch);
      var formattedText = new FormattedText(textBlock.Text, CultureInfo.CurrentCulture, textBlock.FlowDirection, typeface, textBlock.FontSize, textBlock.Foreground, new NumberSubstitution(), textFormattingMode, VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);
      return textBlock.SnapsToDevicePixels || textBlock.UseLayoutRounding || textFormattingMode == TextFormattingMode.Display ? (int)textBlock.ActualWidth < (int)formattedText.Width : textBlock.ActualWidth.LessThan(formattedText.Width);
    }
  }
}