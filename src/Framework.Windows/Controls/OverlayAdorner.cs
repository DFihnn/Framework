using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Framework.Windows.Controls
{
  /// <summary>
  /// Renders a TextBlock in an element's adorner layer.
  /// </summary>
  public class OverlayAdorner : Adorner
  {
    private ArrayList logicalChildren;
    private readonly ContentControl contentControl;

    public OverlayAdorner(UIElement element): base(element)
    {
      contentControl = new ContentControl();
      contentControl.VerticalAlignment = VerticalAlignment.Center;
      contentControl.IsHitTestVisible = false;
      contentControl.Margin = new Thickness(6, 0, 0, 0);
      contentControl.Focusable = false;
      contentControl.Opacity = 0.67;

      element.GotFocus += AdornedElementGotFocus;
      element.LostFocus += AdornedElementLostFocus;

      AddLogicalChild(contentControl);
      AddVisualChild(contentControl);
    }

    public object Content
    {
      get { return contentControl.Content; }
      set { contentControl.Content = value; }
    }

    private void AdornedElementGotFocus(object sender, RoutedEventArgs e)
    {
      contentControl.Opacity = 0.33;
    }

    private void AdornedElementLostFocus(object sender, RoutedEventArgs e)
    {
      contentControl.Opacity = 0.67;
    }

    #region Measure/Arrange

    /// <summary>
    /// Allows the TextBlock to determine how big it wants to be.
    /// </summary>
    /// <param name="constraint">A limiting size for the TextBlock.</param>
    [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#")]
    protected override Size MeasureOverride(Size constraint)
    {
      return new Size(((FrameworkElement)AdornedElement).ActualWidth, ((FrameworkElement)AdornedElement).ActualHeight);
    }

    /// <summary>
    /// Positions and sizes the TextBlock.
    /// </summary>
    /// <param name="finalSize">The actual size of the TextBlock.</param>		
    protected override Size ArrangeOverride(Size finalSize)
    {
      var location = new Point(0, 0);
      var rect = new Rect(location, finalSize);
      contentControl.Arrange(rect);

      return finalSize;
    }

    #endregion // Measure/Arrange

    #region Visual Children

    /// <summary>
    /// Required for the TextBlock to be rendered.
    /// </summary>
    protected override int VisualChildrenCount
    {
      get { return 1; }
    }

    /// <summary>
    /// Required for the TextBlock to be rendered.
    /// </summary>
    protected override Visual GetVisualChild(int index)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException("index");

      return contentControl;
    }

    #endregion // Visual Children

    #region Logical Children

    /// <summary>
    /// Required for the TextBlock to inherit property values
    /// from the logical tree, such as FontSize.
    /// </summary>
    protected override IEnumerator LogicalChildren
    {
      get
      {
        if (logicalChildren == null)
        {
          logicalChildren = new ArrayList();
          logicalChildren.Add(contentControl);
        }

        return logicalChildren.GetEnumerator();
      }
    }

    #endregion // Logical Children
  }
}