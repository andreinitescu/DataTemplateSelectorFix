using Xamarin.Forms;

namespace DataTemplateSelectorFix
{
    public interface IDataTemplateSelector
    {
        DataTemplate SelectTemplate(object item, BindableObject container);
    }
}
