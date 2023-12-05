using ProjetoPetV2.ViewModels;

namespace ProjetoPetV2
{
    public partial class MainPage : ContentPage
    {
        private readonly UsuarioViewModel _viewModel;
        public MainPage(UsuarioViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUsuarioAsync();
        }

    }

}
