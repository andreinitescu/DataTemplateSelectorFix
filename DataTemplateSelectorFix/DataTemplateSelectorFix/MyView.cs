using Xamarin.Forms;

namespace DataTemplateSelectorFix
{
    public class MyView : ContentView
    {
        public static BindableProperty ItemTemplateSelectorProperty =
            BindableProperty.Create(nameof(ItemTemplateSelector), typeof(IDataTemplateSelector), typeof(MyView), default(IDataTemplateSelector),
                propertyChanged: (s, o, n) => ((MyView)s).OnItemTemplateSelectorPropertyChanged());

        public IDataTemplateSelector ItemTemplateSelector
        {
            get => (IDataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
            set => SetValue(ItemTemplateSelectorProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateContent();
        }

        void OnItemTemplateSelectorPropertyChanged()
        {
            UpdateContent();
        }

        void UpdateContent()
        {
            Content = (View)ItemTemplateSelector?.SelectTemplate(BindingContext, this)?.CreateContent();
        }
    }
}
