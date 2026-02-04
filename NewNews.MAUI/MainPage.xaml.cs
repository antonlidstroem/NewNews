using NewNews.MAUI.ViewModels;

namespace NewNews.MAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        private bool _isInitialized = false;

        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Kör initial load endast första gången
            if (!_isInitialized)
            {
                _isInitialized = true;
                await _viewModel.InitializeAsync();
            }
        }
    }
}