using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NewNews.MAUI.ViewModels.Base
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? title;
    }
}
