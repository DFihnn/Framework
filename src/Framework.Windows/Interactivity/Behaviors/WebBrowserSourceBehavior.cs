using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class WebBrowserSourceBehavior : Behavior<WebBrowser>
  {
    public static readonly DependencyProperty SourceUriProperty =
        DependencyProperty.Register("SourceUri", typeof(Uri), typeof(WebBrowserSourceBehavior),
            new FrameworkPropertyMetadata(null, OnSourceUriChanged));

    public bool SourceUri
    {
      get { return (bool)GetValue(SourceUriProperty); }
      set { SetValue(SourceUriProperty, value); }
    }

    private static void OnSourceUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((WebBrowserSourceBehavior)d).OnSourceUriChanged(e);
    }

    protected virtual void OnSourceUriChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject != null)
        AssociatedObject.Source = e.NewValue as Uri;
    }
  }
}