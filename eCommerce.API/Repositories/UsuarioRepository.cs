using eCommerce.API.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace eCommerce.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;
        public UsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        /*
         * Connection => Estabelece conexão com o banco (classes SqlConnection, IDbConnection, DbConnection).
         * Command => INSERT, UPDATE, DELETE... Comandos que geralmente não tem retorno ou pouca informação para retornar.
         * DataReader => Arquitetura conectada no SELECT.
         * DataAdapter => Arquitetura desconectada no SELECT.
         */

       
        public List<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuarios";
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = dataReader.GetInt32("Id");
                    usuario.Nome = dataReader.GetString("Nome");
                    usuario.Email = dataReader.GetString("Email");
                    usuario.Sexo = dataReader.GetString("Sexo");
                    usuario.RG = dataReader.GetString("RG");
                    usuario.CPF = dataReader.GetString("CPF");
                    usuario.NomeMae = dataReader.GetString("NomeMae");
                    usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                    //Para um Object, é necessário informar o número da coluna, ao invés do nome da coluna.
                    usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                    usuarios.Add(usuario);
                }
            }
            finally
            {
                _connection.Close();
            }
            
            return usuarios;
        }

        public Usuario Get(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT * FROM Usuarios WHERE Id = @id";
                command.Parameters.AddWithValue("@Id", id);
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.Id = dataReader.GetInt32("Id");
                    usuario.Nome = dataReader.GetString("Nome");
                    usuario.Email = dataReader.GetString("Email");
                    usuario.Sexo = dataReader.GetString("Sexo");
                    usuario.RG = dataReader.GetString("RG");
                    usuario.CPF = dataReader.GetString("CPF");
                    usuario.NomeMae = dataReader.GetString("NomeMae");
                    usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                    //Para um Object, é necessário informar o número da coluna, ao invés do nome da coluna.
                    usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                    return usuario;

                }

            }
            finally
            {
                _connection.Close();
            }
            return null;
        }

        public void Insert(Usuario usuario)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                //"SELECT CAST(scope_identity() AS int" retorna o último Id inserido nesse escopo de execução.
                command.CommandText = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) " +
                    "VALUES(@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); " +
                    "SELECT CAST(scope_identity() AS int)";
                command.Connection = (SqlConnection)_connection;

                command.Parameters.AddWithValue("Nome", usuario.Nome);
                command.Parameters.AddWithValue("Email", usuario.Email);
                command.Parameters.AddWithValue("Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("RG", usuario.RG);
                command.Parameters.AddWithValue("CPF", usuario.CPF);
                command.Parameters.AddWithValue("NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("SituacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("DataCadastro", usuario.DataCadastro);

                _connection.Open();

                //Sem a query "SELECT CAST(scope_identity() AS int", é possível usar o comando abaixo
                //command.ExecuteNonQuery();

                //O ExecuteScalar retorna uma coluna.
                usuario.Id = (int)command.ExecuteScalar();
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            _db.Remove(_db.FirstOrDefault(a => a.Id == usuario.Id));
            _db.Add(usuario);
        }
        public void Delete(int id)
        {
            _db.Remove(_db.FirstOrDefault(a => a.Id == id));
        }
        private static List<Usuario> _db = new List<Usuario>()
        {
            new Usuario(){ Id = 1, Nome = "Filipe Rodrigues", Email = "filipe.rodrigues@gmail.com" },
            new Usuario(){ Id = 2, Nome = "Marcelo Rodrigues", Email = "marcelo.rodrigues@gmail.com" },
            new Usuario(){ Id = 3, Nome = "Jessica Rodrigues", Email = "jessica.rodrigues@gmail.com" }
        };
    }
}
