using SQLite;
using System.Linq.Expressions;

namespace ProjetoPetV2.Data
{
    public class DatabaseContext : IAsyncDisposable
    {
        //definindo o nome do arquivo do banco de dados
        private const string DbName = "MyDatabase.db3";

        //onde o arquivo vai ser salvo (com especificação de diretório baseado na API de cada sistema, FileSystem para android, AppDataDirectory para iOS)
        //DbName especifica que deve ser o usar o nome guardado na ultima constante "MyDatabase.db3"
        private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

        //define a conexão do SQL como uma variável (não sei se é chamada uma variável, mas pode ser chamada em outras funções)
        private SQLiteAsyncConnection _connection;

        private SQLiteAsyncConnection Database =>

            //vai testar se há conexão com o SQL, se não houver, irá criar uma
            (_connection ??= new SQLiteAsyncConnection(DbPath,
                SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));

        //TTable é criado em cada função como um modelo (ou variável) temporário

        //irá criar uma nova tabela no banco de dados com os dados do modelo de TTable
        private async Task CreateTableIfNotExists<TTable>() where TTable : class, new()
        {
            await Database.CreateTableAsync<TTable>();
        }

        //pega os dados armazenados no TTable
        private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return Database.Table<TTable>();
        }

        //esse método é usado para direcionamento de função que são repetidas, depois de fazer essa função ele executará e retornará o que a função do ViewModel procura
        //esse método foi adicionado, mas não vai ser utilizado no app atual
        private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await action();
        }

        //exemplo de uso do método execute
        /*public async Task<TTable> GetItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            //await CreateTableIfNotExists<TTable>();
            //return await Database.GetAsync<TTable>(primaryKey);

            //executa a função com o valor TTable e depois Retorna o resultado pelo TTable, também pode ser usado para retornar um boolean
            return await Execute<TTable, TTable> (async () => await Database.GetAsync<TTable> (primaryKey));
        }*/

        //pega os dados armazenados no banco
        public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.ToListAsync();
        }

        //retorna as gravações feitas no banco
        public async Task<IEnumerable<TTable>> GetFileteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
        {
            var table = await GetTableAsync<TTable>();
            return await table.Where(predicate).ToListAsync();
        }

        //pega os itens por key
        public async Task<TTable> GetItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.GetAsync<TTable>(primaryKey);
        }

        //adicionar um item e retorna "true" se o número de gravações for maior que 0
        public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.InsertAsync(item) > 0;
        }
        //atualiza um item e retorna "true" se o número de atualizações for maior que 0
        public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.UpdateAsync(item) > 0;
        }
        //remove um item e retorna "true" se o número de remoções for maior que 0 (não necessário no app)
        public async Task<bool> DeleteItemByIdAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync<TTable>(primaryKey) > 0;
        }

        public async Task<bool> DeleteItemByKeyAsync<TTable>(object primaryKey) where TTable : class, new()
        {
            await CreateTableIfNotExists<TTable>();
            return await Database.DeleteAsync<TTable>(primaryKey) > 0;
        }

        public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
        

        /*public async Task Try()
        {
            //irá criar uma nova tabela no banco de dados com os dados do modelo de Usuario
            await Database.CreateTableAsync<Usuario>();

            //Função de listar os dados da tabela Usuario
            Database.Table<Usuario>().ToListAsync();
        }*/
    }
}
