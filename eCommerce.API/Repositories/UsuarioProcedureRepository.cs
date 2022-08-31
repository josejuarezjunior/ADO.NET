using eCommerce.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace eCommerce.API.Repositories
{
    public class UsuarioProcedureRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public UsuarioProcedureRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=eCommerce;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public List<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)_connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SelecionarUsuarios";

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
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SelecionarUsuario";
                command.Parameters.AddWithValue("@id", id);
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
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand();
                command.Connection = (SqlConnection)_connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CadastrarUsuario";

                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@rG", usuario.RG);
                command.Parameters.AddWithValue("@cpf", usuario.CPF);
                command.Parameters.AddWithValue("@nomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@situacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@dataCadastro", usuario.DataCadastro);

                usuario.Id = (int)command.ExecuteScalar();
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario usuario)
        {
            _connection.Open();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "AtualizarUsuario";
                command.Connection = (SqlConnection)_connection;

                command.Parameters.AddWithValue("@nome", usuario.Nome);
                command.Parameters.AddWithValue("@email", usuario.Email);
                command.Parameters.AddWithValue("@sexo", usuario.Sexo);
                command.Parameters.AddWithValue("@rG", usuario.RG);
                command.Parameters.AddWithValue("@cpf", usuario.CPF);
                command.Parameters.AddWithValue("@nomeMae", usuario.NomeMae);
                command.Parameters.AddWithValue("@situacaoCadastro", usuario.SituacaoCadastro);
                command.Parameters.AddWithValue("@dataCadastro", usuario.DataCadastro);

                //Parâmetro de filtro
                command.Parameters.AddWithValue("@id", usuario.Id);

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
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "DeletarUsuario";
                command.Connection = (SqlConnection)_connection;

                //Parâmetro de filtro
                command.Parameters.AddWithValue("@id", id);

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
