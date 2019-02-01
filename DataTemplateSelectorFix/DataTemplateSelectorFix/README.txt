# IDataTemplateSelector

This is a Xamarin Forms proof-of-concept app to explore the possibility to add a new IDataTemplateSelector interface to Xamarin Forms.

To understand the story behind this, either see https://github.com/xamarin/Xamarin.Forms/issues/3544 or just continue to read below.

### What's DataTemplateSelector for?

(Skip this part if you know what is it)

DataTemplateSelector is a simple abstract class used as a strategy to return a DataTemplate depending on the data item.
Controls which display items have a property of the DataTemplateSelector type to allow display items differently depending on the item.

```
public abstract class DataTemplateSelector : DataTemplate
{
    public DataTemplate SelectTemplate(object item, BindableObject container)
    {
        // Calls OnSelectTemplate(item, container)
    }

    protected abstract DataTemplate OnSelectTemplate(object item, BindableObject container);
}
```

To use it, you create your own data-template selector:

```
class MyDataTemplateSelector : DataTemplateSelector
{
    public MyDataTemplateSelector ()
    {
        // Retain instances
        this.templateOne = new DataTemplate (typeof (ViewA));
        this.templateTwo = new DataTemplate (typeof (ViewB));
    }

    protected override DataTemplate OnSelectTemplate (object item, BindableObject container)
    {
        if (item is double)
            return this.templateOne;
        return this.templateTwo;
    }

    private readonly DataTemplate templateOne;
    private readonly DataTemplate templateTwo;
}
```

See https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/templates/data-templates/selector

### Why a new interface, what's wrong with current DataTemplateSelector in Xamarin Forms?

Currently, DataTemplateSelector class derives from DataTemplate which has some inconveniences:
 - It breaks [Liskov subsitution principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle). Since DataTemplateSelector derives from DataTemplate, you'd expect to be able to call CreateContent() on a DataTemplateSelector instance, but it doesn't work, the implementation throws an error, see it [here](https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/ElementTemplate.cs#L79). 
   There are several issues opened in Xamarin Forms repo related to this, these are just a few: [3444](https://github.com/xamarin/Xamarin.Forms/issues/3544), [3749](https://github.com/xamarin/Xamarin.Forms/issues/3749)
 - It's unecessarily heavy. Since DataTemplateSelector derives from DataTemplate (which in turn derives from Element), it carries with it data and logic which is not used by the DataTemplateSelector implementation in any way.
   
### OK, so what are you suggesting?

Two things:

1. Add an interface IDataTemplateSelector. See IDataTemplateSelector.cs in this repo

2. Add a new bindable property to ListView, called ItemTemplateSelector:

```
public class ListView : .. 
{ 
      IDataTemplateSelector ItemTemplateSelector { get; set; }
}
```

All new controls should follow same pattern, by having a similar separate property for item template selector.

### Isn't having both ItemTemplate and ItemTemplateSelector properties confusing?

It shouldn't, it's been like this in Windows XAML for more than a decade.
The rule is ItemTemplate takes priority over ItemTemplateSelector if both are set.

### Would it break anything in ListView?

No. Apps can continue using an instance of a DataTemplateSelector set on the ListBox.ItemTemplate property. However, the recommended way will be to use ItemTemplateSelector property instead.

### But can't all this be solved if we just change DataTemplate.CreateContent() to have parameters?

I suppose, something like 

```
abstract class DataTemplate
{
   protected abstract OnCreateContent(object context, BindableObject container);
}
```

Why context? Why container? A DataTemplate should not care about these.
Also, what about the fact that DataTemplateSelector holds data and logic which doesn't use at all?
Another problem: similar to the data-template selector pattern, what if we want to have a StyleSelector too? Should StyleSelector derive from Style too? 

 
