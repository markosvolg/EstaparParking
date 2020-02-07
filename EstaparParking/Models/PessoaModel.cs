using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EstaparParking.Models
{
    public class PessoaModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Preencha o Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Preencha a Data de Nascimento")]
        public string Dt_Nasc { get; set; }

        public virtual ICollection<CarroModel> Carros { get; set; }



        public int AdicionarPessoa()
        {

            var ret = 0;

            var model = RecuperarPorId(this.Id);


            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                conexao.Open();

                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {

                        comando.CommandText = "insert into  Pessoa (nome, cpf, dt_nasc) values(@nome,@cpf,@dt_nasc); select convert(int,scope_identity())";


                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.CPF;
                        comando.Parameters.Add("@dt_nasc", SqlDbType.VarChar).Value = this.Dt_Nasc;
                        

                        ret = (int)comando.ExecuteScalar();

                    }
                    else
                    {
                        comando.CommandText = "update Pessoa set nome = @nome ,cpf = @cpf, dt_nasc = @dt_nasc where id =@id";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@cpf", SqlDbType.VarChar).Value = this.CPF;
                        comando.Parameters.Add("@dt_nasc", SqlDbType.VarChar).Value = this.Dt_Nasc;
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {

                            ret = this.Id;

                        }
                    }

                }
            }

            return ret;
        }




        public static PessoaModel RecuperarPorId(int id)
        {

            PessoaModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString; 
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from Pessoa where id = @id ";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                     {
                        ret = new PessoaModel()
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            CPF = (string)reader["cpf"],
                            Dt_Nasc = (string)reader["dt_nasc"]

                        };
                    }

                }

            }
            return ret;
        }








        public static List<PessoaModel> RecuperarLista(int pagina, int tamPagina)
        {

            var ret = new List<PessoaModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from Pessoa order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new PessoaModel
                        {
                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            CPF = (string)reader["cpf"],
                            Dt_Nasc = (string)reader["dt_nasc"],


                        });
                    }

                }

            }
            return ret;


        }



        public static int RecuperarQuantidade()
        {

            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {


                    comando.Connection = conexao;
                    comando.CommandText = "select count(*) from Pessoa";

                    ret = (int)comando.ExecuteScalar();


                }

            }
            return ret;

        }





        public static bool ExluirPorId(int id)
        {

            var ret = false;


            if (RecuperarPorId(id) != null)
            {
                
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from Pessoa where id =@id";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = comando.ExecuteNonQuery() > 0;

                    }

                }
            }
            return ret;
        }

    }

}