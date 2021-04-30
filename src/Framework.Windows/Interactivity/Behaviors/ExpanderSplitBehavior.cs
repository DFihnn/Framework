using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class ExpanderSplitBehavior : Behavior<Expander>
  {
    private GridSplitter gridSplitter;
    private Grid parentGrid;
    private int gridSplitterLenth;

    #region Width

    /// <summary>
    /// Width Dependency Property
    /// </summary>
    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register("Width", typeof(GridLength), typeof(ExpanderSplitBehavior),
            new FrameworkPropertyMetadata((GridLength)GridLength.Auto,
                FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnWidthChanged));

    /// <summary>
    /// Gets or sets the Width property.  This dependency property 
    /// indicates ....
    /// </summary>
    public GridLength Width
    {
      get { return (GridLength)GetValue(WidthProperty); }
      set { SetValue(WidthProperty, value); }
    }

    private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderSplitBehavior)d).OnWidthChanged(e);
    }

    private void OnWidthChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject == null || !AssociatedObject.IsExpanded)
        return;

      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
        parentGrid.ColumnDefinitions[Grid.GetColumn(AssociatedObject)].Width = Width;
    }

    #endregion

    #region ExpandedMinWidth

    /// <summary>
    /// ExpandedMinWidth Dependency Property
    /// </summary>
    public static readonly DependencyProperty ExpandedMinWidthProperty =
        DependencyProperty.Register("ExpandedMinWidth", typeof(double), typeof(ExpanderSplitBehavior),
            new FrameworkPropertyMetadata((double)0,
                new PropertyChangedCallback(OnExpandedMinWidthChanged)));

    /// <summary>
    /// Gets or sets the ExpandedMinWidth property.  This dependency property 
    /// indicates ....
    /// </summary>
    public double ExpandedMinWidth
    {
      get { return (double)GetValue(ExpandedMinWidthProperty); }
      set { SetValue(ExpandedMinWidthProperty, value); }
    }

    /// <summary>
    /// Handles changes to the ExpandedMinWidth property.
    /// </summary>
    private static void OnExpandedMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderSplitBehavior)d).OnExpandedMinWidthChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the ExpandedMinWidth property.
    /// </summary>
    protected virtual void OnExpandedMinWidthChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject == null || !AssociatedObject.IsExpanded)
        return;

      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
        parentGrid.ColumnDefinitions[Grid.GetColumn(AssociatedObject)].MinWidth = ExpandedMinWidth;
    }

    #endregion

    #region Height

    /// <summary>
    /// Height Dependency Property
    /// </summary>
    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register("Height", typeof(GridLength), typeof(ExpanderSplitBehavior),
            new FrameworkPropertyMetadata((GridLength)GridLength.Auto,
                FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnHeightChanged));

    /// <summary>
    /// Gets or sets the Height property.  This dependency property 
    /// indicates ....
    /// </summary>
    public GridLength Height
    {
      get { return (GridLength)GetValue(HeightProperty); }
      set { SetValue(HeightProperty, value); }
    }

    private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderSplitBehavior)d).OnHeightChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the He property.
    /// </summary>
    protected virtual void OnHeightChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject == null || !AssociatedObject.IsExpanded)
        return;

      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Up || AssociatedObject.ExpandDirection == ExpandDirection.Down)
        parentGrid.RowDefinitions[Grid.GetRow(AssociatedObject)].Height = Height;
    }

    #endregion

    #region ExpandedMinHeight

    /// <summary>
    /// ExpandedMinHeight Dependency Property
    /// </summary>
    public static readonly DependencyProperty ExpandedMinHeightProperty =
        DependencyProperty.Register("ExpandedMinHeight", typeof(double), typeof(ExpanderSplitBehavior),
            new FrameworkPropertyMetadata((double)0,
                new PropertyChangedCallback(OnExpandedMinHeightChanged)));

    /// <summary>
    /// Gets or sets the ExpandedMinHeight property.  This dependency property 
    /// indicates ....
    /// </summary>
    public double ExpandedMinHeight
    {
      get { return (double)GetValue(ExpandedMinHeightProperty); }
      set { SetValue(ExpandedMinHeightProperty, value); }
    }

    /// <summary>
    /// Handles changes to the ExpandedMinHeight property.
    /// </summary>
    private static void OnExpandedMinHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderSplitBehavior)d).OnExpandedMinHeightChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the ExpandedMinHeight property.
    /// </summary>
    protected virtual void OnExpandedMinHeightChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject == null || !AssociatedObject.IsExpanded)
        return;

      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Up || AssociatedObject.ExpandDirection == ExpandDirection.Down)
        parentGrid.RowDefinitions[Grid.GetRow(AssociatedObject)].MinHeight = ExpandedMinHeight;
    }

    #endregion
    
    #region GridSplitterStyle

    /// <summary>
    /// GridSplitterStyle Dependency Property
    /// </summary>
    public static readonly DependencyProperty GridSplitterStyleProperty =
        DependencyProperty.Register("GridSplitterStyle", typeof(Style), typeof(ExpanderSplitBehavior),
            new FrameworkPropertyMetadata((Style)null,
                new PropertyChangedCallback(OnGridSplitterStyleChanged)));

    /// <summary>
    /// Gets or sets the GridSplitterStyle property.  This dependency property 
    /// indicates ....
    /// </summary>
    public Style GridSplitterStyle
    {
      get { return (Style)GetValue(GridSplitterStyleProperty); }
      set { SetValue(GridSplitterStyleProperty, value); }
    }

    /// <summary>
    /// Handles changes to the GridSplitterStyle property.
    /// </summary>
    private static void OnGridSplitterStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((ExpanderSplitBehavior)d).OnGridSplitterStyleChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the GridSplitterStyle property.
    /// </summary>
    protected virtual void OnGridSplitterStyleChanged(DependencyPropertyChangedEventArgs e)
    {
      if (gridSplitter != null)
        gridSplitter.Style = e.NewValue as Style;
    }

    #endregion

    protected override void OnAttached()
    {
      gridSplitterLenth = 5;
      parentGrid = AssociatedObject.Parent as Grid;

      if (parentGrid == null)
        return;

      gridSplitter = new GridSplitter();
      gridSplitter.Visibility = Visibility.Collapsed;
      gridSplitter.DragCompleted += GridSplitterOnDragCompleted;

      if (GridSplitterStyle != null)
        gridSplitter.Style = GridSplitterStyle;

      switch (AssociatedObject.ExpandDirection)
      {
        case ExpandDirection.Down:
          gridSplitter.Height = gridSplitterLenth;
          gridSplitter.ResizeDirection = GridResizeDirection.Rows;
          gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
          gridSplitter.VerticalAlignment = VerticalAlignment.Bottom;
          gridSplitter.ResizeBehavior = GridResizeBehavior.CurrentAndNext;
          break;
        case ExpandDirection.Up:
          gridSplitter.Height = gridSplitterLenth;
          gridSplitter.ResizeDirection = GridResizeDirection.Rows;
          gridSplitter.HorizontalAlignment = HorizontalAlignment.Stretch;
          gridSplitter.VerticalAlignment = VerticalAlignment.Top;
          gridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
          break;
        case ExpandDirection.Left:
          gridSplitter.Width = gridSplitterLenth;
          gridSplitter.ResizeDirection = GridResizeDirection.Columns;
          gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
          gridSplitter.HorizontalAlignment = HorizontalAlignment.Right;
          gridSplitter.ResizeBehavior = GridResizeBehavior.CurrentAndNext;
          break;
        case ExpandDirection.Right:
          gridSplitter.Width = gridSplitterLenth;
          gridSplitter.ResizeDirection = GridResizeDirection.Columns;
          gridSplitter.VerticalAlignment = VerticalAlignment.Stretch;
          gridSplitter.HorizontalAlignment = HorizontalAlignment.Left;
          gridSplitter.ResizeBehavior = GridResizeBehavior.PreviousAndCurrent;
          break;
      }

      parentGrid.Children.Add(gridSplitter);

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
        Grid.SetColumn(gridSplitter, Grid.GetColumn(AssociatedObject));
      else
        Grid.SetRow(gridSplitter, Grid.GetRow(AssociatedObject));

      AssociatedObject.Collapsed += OnCollapsed;
      AssociatedObject.Expanded += OnExpanded;

      if (!AssociatedObject.IsExpanded)
        return;

      OnExpand();
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();

      if (AssociatedObject != null)
      {
        AssociatedObject.Collapsed -= OnCollapsed;
        AssociatedObject.Expanded -= OnExpanded;
      }

      if (gridSplitter != null)
        gridSplitter.DragCompleted -= GridSplitterOnDragCompleted;
    }

    private void GridSplitterOnDragCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
    {
      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
      {
        var column = Grid.GetColumn(AssociatedObject);
        Width = parentGrid.ColumnDefinitions[column].Width;
      }
      else
      {
        var row = Grid.GetRow(AssociatedObject);
        Height = parentGrid.RowDefinitions[row].Height;
      }
    }

    private void OnExpanded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (routedEventArgs.OriginalSource == AssociatedObject)
        OnExpand();
    }

    private void OnCollapsed(object sender, RoutedEventArgs routedEventArgs)
    {
      if (routedEventArgs.OriginalSource == AssociatedObject)
        OnCollapse();
    }

    private void OnCollapse()
    {
      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
      {
        var column = Grid.GetColumn(AssociatedObject);
        Width = parentGrid.ColumnDefinitions[column].Width;
        parentGrid.ColumnDefinitions[column].MinWidth = 0;
        parentGrid.ColumnDefinitions[column].Width = GridLength.Auto;

        if (AssociatedObject.ExpandDirection == ExpandDirection.Right)
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left - gridSplitterLenth;
          var top = AssociatedObject.Margin.Top;
          var margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
        else
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top;
          margin = AssociatedObject.Margin;
          var right = margin.Right - gridSplitterLenth;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
      }
      else
      {
        var row = Grid.GetRow(AssociatedObject);
        Height = parentGrid.RowDefinitions[row].Height;
        parentGrid.RowDefinitions[row].MinHeight = 0;
        parentGrid.RowDefinitions[row].Height = GridLength.Auto;

        if (AssociatedObject.ExpandDirection == ExpandDirection.Up)
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top - gridSplitterLenth;
          margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
        else
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top;
          margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom - gridSplitterLenth;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
      }

      if (gridSplitter != null)
        gridSplitter.Visibility = Visibility.Collapsed;
    }

    private void OnExpand()
    {
      if (parentGrid == null)
        return;

      if (AssociatedObject.ExpandDirection == ExpandDirection.Left || AssociatedObject.ExpandDirection == ExpandDirection.Right)
      {
        var column = Grid.GetColumn(AssociatedObject);

        parentGrid.ColumnDefinitions[column].Width = Width;
        parentGrid.ColumnDefinitions[column].MinWidth = ExpandedMinWidth;

        if (AssociatedObject.ExpandDirection == ExpandDirection.Right)
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left + gridSplitterLenth;
          var top = AssociatedObject.Margin.Top;
          var margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
        else
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top;
          margin = AssociatedObject.Margin;
          var right = margin.Right + gridSplitterLenth;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
      }
      else
      {
        var row = Grid.GetRow(AssociatedObject);
        parentGrid.RowDefinitions[row].Height = Height;
        parentGrid.RowDefinitions[row].MinHeight = ExpandedMinHeight;

        if (AssociatedObject.ExpandDirection == ExpandDirection.Up)
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top + gridSplitterLenth;
          margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
        else
        {
          var associatedObject = AssociatedObject;
          var left = AssociatedObject.Margin.Left;
          var margin = AssociatedObject.Margin;
          var top = margin.Top;
          margin = AssociatedObject.Margin;
          var right = margin.Right;
          margin = AssociatedObject.Margin;
          var bottom = margin.Bottom + gridSplitterLenth;
          var thickness = new Thickness(left, top, right, bottom);
          associatedObject.Margin = thickness;
        }
      }

      if (gridSplitter != null)
        gridSplitter.Visibility = Visibility.Visible;
    }
  }
}