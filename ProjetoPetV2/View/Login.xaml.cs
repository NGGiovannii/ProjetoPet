using MyPet.Views;

namespace MyPet
{
    public partial class Login : ContentPage
    {
        

        public Login()
        {
            InitializeComponent();


        }


        private void btnCadastrar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Cadastro());
        }

        private async void btnLoginClicked(object sender, EventArgs e)
        {
            string email = txtUser.Text;
            string senha = txtPass.Text;



        }
    }

}
