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
                    usuario.Id = dataReader.GetInt32("Id_Usuarios");
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
                command.CommandText = "SELECT * FROM Usuarios U LEFT JOIN Contatos C ON C.UsuarioId = U.Id_Usuarios LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id_Usuarios WHERE U.Id_Usuarios = @id";
                command.Parameters.AddWithValue("@Id", id);
                command.Connection = (SqlConnection)_connection;
                _connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();

                Dictionary<int, Usuario> usuarios = new Dictionary<int, Usuario>();

                while (dataReader.Read())
                {
                    Usuario usuario = new Usuario();

                    if (!(usuarios.ContainsKey(dataReader.GetInt32("Id_Usuarios"))))
                    {
                        usuario.Id = dataReader.GetInt32("Id_Usuarios");
                        usuario.Nome = dataReader.GetString("Nome");
                        usuario.Email = dataReader.GetString("Email");
                        usuario.Sexo = dataReader.GetString("Sexo");
                        usuario.RG = dataReader.GetString("RG");
                        usuario.CPF = dataReader.GetString("CPF");
                        usuario.NomeMae = dataReader.GetString("NomeMae");
                        usuario.SituacaoCadastro = dataReader.GetString("SituacaoCadastro");
                        //Para um Object, é necessário informar o número da coluna, ao invés do nome da coluna.
                        usuario.DataCadastro = dataReader.GetDateTimeOffset(8);

                        Contato contato = new Contato();
                        contato.Id = dataReader.GetInt32("Id_Contatos");
                        contato.UsuarioId = usuario.Id;
                        contato.Telefone = dataReader.GetString("Telefone");
                        contato.Celular = dataReader.GetString("Celular");

                        usuario.Contato = contato;

                        usuarios.Add(usuario.Id, usuario);
                    }
                    else
                    {
                        usuario = usuarios[dataReader.GetInt32("Id_Usuarios")];
                    }

                    EnderecoEntrega enderecoEntrega = new EnderecoEntrega();
                    enderecoEntrega.Id = dataReader.GetInt32("Id_EnderecosEntrega");
                    enderecoEntrega.UsuarioId = usuario.Id;
                    enderecoEntrega.NomeEndereco = dataReader.GetString("NomeEndereco");
                    enderecoEntrega.CEP = dataReader.GetString("CEP");
                    enderecoEntrega.Estado = dataReader.GetString("Estado");
                    enderecoEntrega.Cidade = dataReader.GetString("Cidade");
                    enderecoEntrega.Bairro = dataReader.GetString("Bairro");
                    enderecoEntrega.Endereco = dataReader.GetString("Endereco");
                    enderecoEntrega.Numero = dataReader.GetString("Numero");
                    enderecoEntrega.Complemento = dataReader.GetString("Complemento");

                    usuario.EnderecosEntrega = (usuario.EnderecosEntrega == null) ? new List<EnderecoEntrega>() : usuario.EnderecosEntrega;
                    usuario.EnderecosEntrega.Add(enderecoEntrega);

                }
                return usuarios[usuarios.Keys.First()];

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

                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);

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
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro WHERE Id_Usuarios = @Id";

                command.Connection = (SqlConnection)_connection;

                command.Parameters.AddWithValue("@Nome", usuario.Nome);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@RG", usuario.RG);
                command.Parameters.AddWithValue("@CPF", usuario.CPF);
                command.Parameters.AddWithValue("@NomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@SituacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@DataCadastro", usuario.DataCadastro);
                
                //Parâmetro de filtro
                command.Parameters.AddWithValue("@Id", usuario.Id);

                _connection.Open();

                command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
        public void Delete(int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "DELETE FROM Usuarios WHERE Id_Usuarios = @Id";
                command.Connection = (SqlConnection)_connection;

                //Parâmetro de filtro
                command.Parameters.AddWithValue("@Id", id);

                _connection.Open();

                command.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
      
    }
}
