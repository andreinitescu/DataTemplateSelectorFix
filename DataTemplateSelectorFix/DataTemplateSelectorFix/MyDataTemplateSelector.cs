using Xamarin.Forms;

namespace DataTemplateSelectorFix
{
    class MyDataTemplateSelector : IDataTemplateSelector
    {
        public DataTemplate OddTemplate { get; set; }
        public DataTemplate EvenTemplate { get; set; }

        public DataTemplate SelectTemplate(object item, BindableObject container)
        {
            if (item == null)
                return null;
            return (int)item % 2 == 0 ? EvenTemplate : OddTemplate;
        }
    }
}
