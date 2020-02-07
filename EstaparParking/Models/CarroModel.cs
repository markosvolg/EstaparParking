using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EstaparParking.Models
{
    public class CarroModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha a Marca")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Preencha o Modelo")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Preencha a Placa")]
        public string Placa { get; set; }

        public virtual ICollection<PessoaModel> Pessoas { get; set; }



    public int AdicionarCarro()
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

                        comando.CommandText = "insert into  Carro (marca, modelo, placa) values(@marca,@modelo,@placa); select convert(int,scope_identity())";


                        comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = this.Marca;
                        comando.Parameters.Add("@modelo", SqlDbType.VarChar).Value = this.Modelo;
                        comando.Parameters.Add("@placa", SqlDbType.VarChar).Value = this.Placa;


                        ret = (int)comando.ExecuteScalar();

                    }
                    else
                    {
                        comando.CommandText = "update Carro set marca = @marca ,modelo = @modelo, placa = @placa where id =@id";

                        comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = this.Marca;
                        comando.Parameters.Add("@modelo", SqlDbType.VarChar).Value = this.Modelo;
                        comando.Parameters.Add("@placa", SqlDbType.VarChar).Value = this.Placa;
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




        public static CarroModel RecuperarPorId(int id)
        {

            CarroModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from Carro where id = @id ";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        ret = new CarroModel()
                        {
                            Id = (int)reader["id"],
                            Marca = (string)reader["marca"],
                            Modelo = (string)reader["modelo"],
                            Placa = (string)reader["placa"]

                        };
                    }

                }

            }
            return ret;
        }




        public static List<CarroModel> RecuperarLista(int pagina, int tamPagina)
        {

            var ret = new List<CarroModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["EstapaDB"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    var pos = (pagina - 1) * tamPagina;

                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from Carro order by marca offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        ret.Add(new CarroModel
                        {
                            Id = (int)reader["id"],
                            Marca = (string)reader["marca"],
                            Modelo = (string)reader["modelo"],
                            Placa = (string)reader["placa"]


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
                    comando.CommandText = "select count(*) from Carro";

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
                        comando.CommandText = "delete from Carro where id =@id";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = comando.ExecuteNonQuery() > 0;

                    }

                }
            }
            return ret;
        }

    }


}



