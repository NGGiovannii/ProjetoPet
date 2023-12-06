using ProjetoPetV2.ViewModels;
using System.Security.Cryptography;

namespace ProjetoPetV2.View;

public partial class Pesquisa : ContentPage
{
	private readonly UsuarioViewModel _viewModel;

    public Pesquisa(UsuarioViewModel viewModel)
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