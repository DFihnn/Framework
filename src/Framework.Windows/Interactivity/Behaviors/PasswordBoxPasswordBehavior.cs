using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class PasswordBoxPasswordBehavior : Behavior<PasswordBox>
  {
    /// <summary>
    /// Password Dependency Property
    /// </summary>
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxPasswordBehavior),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnPasswordChanged)));

    /// <summary>
    /// Gets or sets the Password property.  This dependency property 
    /// indicates ....
    /// </summary>
    public string Password
    {
      get { return (string)GetValue(PasswordProperty); }
      set { SetValue(PasswordProperty, value); }
    }

    /// <summary>
    /// Handles changes to the Password property.
    /// </summary>
    private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((PasswordBoxPasswordBehavior)d).OnPasswordChanged(e);
    }

    /// <summary>
    /// Provides derived classes an opportunity to handle changes to the Password property.
    /// </summary>
    protected virtual void OnPasswordChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject != null && !Equals(AssociatedObject.Password, Password))
        AssociatedObject.Password = Password;
    }
    
    protected override void OnAttached()
    {
      AssociatedObject.PasswordChanged += PasswordChanged;
      AssociatedObject.Password = Password;
    }

    protected override void OnDetaching()
    {
      AssociatedObject.PasswordChanged -= PasswordChanged;
    }
    
    private void PasswordChanged(object sender, RoutedEventArgs e)
    {
      Password = AssociatedObject.Password;
    }
  }
}