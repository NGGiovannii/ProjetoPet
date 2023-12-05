using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyPet.Models;
using ProjetoPetV2.Data;
using ProjetoPetV2.Models;
using System.Collections.ObjectModel;
namespace ProjetoPetV2.ViewModels
{
    public partial class UsuarioViewModel : ObservableObject
    {

        private readonly DatabaseContext _context;
        public UsuarioViewModel(DatabaseContext context)
        {
            _context = context;
        }

        // ?!
        [ObservableProperty]
        private ObservableCollection<Usuario> _usuarios;

        [ObservableProperty]
        private Usuario _operatingUsuario;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        //procura dados da tabela do banco, pode ser inseridas novas models aqui
        public async Task LoadUsuarioAsync()
        {
            await ExecuteAsync(async () =>
            {
                var usuarios = await _context.GetAllAsync<Usuario>();
                var pets = await _context.GetAllAsync<Pet>();

                //se usuarios não for nulo e tiver qualquer valor, será adicionado à tabela usuario
                if (usuarios is not null && usuarios.Any())
                {
                    Usuarios ??= new ObservableCollection<Usuario>();

                    foreach (var usuario in usuarios)
                    {
                        Usuarios.Add(usuario);
                    }
                }
            }, "Procurando usuários...");

        }

        [RelayCommand]
        private void SetOperatingUsuario(Usuario? usuario) => OperatingUsuario = usuario ?? new();

        [RelayCommand]
        private async Task SaveUsuarioAsync()
        {
            if (OperatingUsuario is null)
                return;
            var busyText = OperatingUsuario.Id == 0 ? "Criando usuário..." : "Atualizando usuário";
            await ExecuteAsync(async () =>
            {
                if (OperatingUsuario.Id == 0)
                {
                    // Cria Usuário
                    await _context.AddItemAsync<Usuario>(OperatingUsuario);
                    Usuarios.Add(OperatingUsuario);
                }

                else
                {
                    //Atualiza usuário
                    await _context.UpdateItemAsync<Usuario>(OperatingUsuario);

                    var usuarioCopy = OperatingUsuario.Clone();

                    var index = Usuarios.IndexOf(OperatingUsuario);
                    Usuarios.RemoveAt(index);

                    Usuarios.Insert(index, usuarioCopy);
                }
                OperatingUsuario = new();
                SetOperatingUsuarioCommand.Execute(new());
            }, busyText);


        }

        [RelayCommand]
        private async Task DeleteUsuarioAsync(int id)
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByKeyAsync<Usuario>(id))
                {
                    var usuario = Usuarios.FirstOrDefault(p => p.Id == id);
                    Usuarios.Remove(usuario);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erro na remoção", "Usuario não foi apagado", "Ok");
                }
            }, "Deletando Usuario...");
        }

        private async Task ExecuteAsync(Func<Task> Operation, string busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processando...";
            try
            {
                await Operation?.Invoke();

            }
            finally
            {
                IsBusy = false;
                BusyText = "Processando...";
            }
        }
    }
}
