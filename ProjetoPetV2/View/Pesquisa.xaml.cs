using ProjetoPetV2.ViewModels;

namespace ProjetoPetV2.View;

public partial class Pesquisa : ContentPage
{
	private readonly UsuarioViewModel _viewModel;

	public Pesquisa()
	{
        InitializeComponent();
    }

    public Pesquisa(UsuarioViewModel viewModel)
	{
        BindingContext = viewModel;
		_viewModel = viewModel;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadUsuarioAsync();
	}
}