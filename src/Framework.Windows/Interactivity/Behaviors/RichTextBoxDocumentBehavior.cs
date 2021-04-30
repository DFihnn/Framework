using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Xaml.Behaviors;

namespace Framework.Windows.Interactivity.Behaviors
{
  public class RichTextBoxDocumentBehavior : Behavior<RichTextBox>
  {
    public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.Register("Document", typeof(FlowDocument), typeof(RichTextBoxDocumentBehavior),
            new FrameworkPropertyMetadata(null, OnDocumentChanged));

    public FlowDocument Document
    {
      get { return (FlowDocument)GetValue(DocumentProperty); }
      set { SetValue(DocumentProperty, value); }
    }

    private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((RichTextBoxDocumentBehavior)d).OnDocumentChanged(e);
    }

    protected virtual void OnDocumentChanged(DependencyPropertyChangedEventArgs e)
    {
      if (AssociatedObject != null)
        AssociatedObject.Document = Document;
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      if (Document != null)
        AssociatedObject.Document = Document;
    }
  }
}