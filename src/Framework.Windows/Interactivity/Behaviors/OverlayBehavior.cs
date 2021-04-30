using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Xaml.Behaviors;
using Framework.Windows.Controls;

namespace Framework.Windows.Interactivity.Behaviors
{
  [DefaultProperty("Content")]
  [System.Windows.Markup.ContentProperty("Content")]
  public abstract class OverlayBehavior<T> : Behavior<T> where T: DependencyObject
  {
    private OverlayAdorner overlayAdorner;
    private FrameworkElement associatedElement;
    private bool showOverlay = true;
    private bool isVisible;

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(OverlayBehavior<T>), new FrameworkPropertyMetadata(null, OnContentChanged));

    [Bindable(true)]
    public object Content
    {
      get { return GetValue(ContentProperty); }
      set { SetValue(ContentProperty, value); }
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((OverlayBehavior<T>)d).OnContentChanged(e);
    }

    protected virtual void OnContentChanged(DependencyPropertyChangedEventArgs e)
    {
      if (overlayAdorner != null)
        overlayAdorner.Content = e.NewValue;
    }

    protected override void OnAttached()
    {
      base.OnAttached();

      associatedElement = AssociatedObject as  FrameworkElement;

      if (associatedElement != null)
        associatedElement.Loaded += AssociatedElementOnLoaded;
    }

    private void AssociatedElementOnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (associatedElement != null)
      {
        var adornerLayer = AdornerLayer.GetAdornerLayer(associatedElement);
        overlayAdorner = new OverlayAdorner(associatedElement);
        overlayAdorner.Content = Content;
        adornerLayer.Add(overlayAdorner);
        associatedElement.IsVisibleChanged += AdornedElementIsVisibleChanged;
        IsVisible = associatedElement.IsVisible;
      }
    }

    private void AdornedElementIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      IsVisible = (bool)e.NewValue;
    }

    private bool IsVisible
    {
      get { return isVisible; }
      set
      {
        isVisible = value;
        UpdateVisibility();
      }
    }

    protected bool ShowOverlay
    {
      get { return showOverlay; }
      set
      {
        showOverlay = value;
        UpdateVisibility();
      }
    }

    private void UpdateVisibility()
    {
      if (overlayAdorner != null)
        overlayAdorner.Visibility = ShowOverlay && IsVisible ? Visibility.Visible : Visibility.Collapsed;
    }
  }
}