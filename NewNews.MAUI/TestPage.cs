using System;
using System.Collections.Generic;
using System.Text;

namespace NewNews.MAUI;

public class TestPage : ContentPage
{
    public TestPage()
    {
        Content = new Label
        {
            Text = "Appen startade!",
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
    }
}

