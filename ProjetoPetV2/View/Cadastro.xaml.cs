using ProjetoPetV2.Models;

namespace MyPet.Views;

public partial class Cadastro : ContentPage
{
	Usuario _usuario;
	public Cadastro()
	{
		InitializeComponent();
		_usuario = new Usuario();
		
		this.BindingContext = _usuario;
	}

    private async void btnCadastrarClicked(object sender, EventArgs e)
    {

    }

    private void btnVoltarLogin(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Login());
    }
}