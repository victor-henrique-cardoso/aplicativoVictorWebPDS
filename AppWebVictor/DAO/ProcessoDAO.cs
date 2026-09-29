using AppWebVictor.Configs;
using AppWebVictor.Model;
using MySql.Data.MySqlClient;

namespace AppWebVictor.DAO
{
    public class ProcessoDAO
    {
        private readonly Conexao _conexao;

        public ProcessoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ - lista todos os processos
        public List<Processo> Listar()
        {
            var lista = new List<Processo>();

            var comando = _conexao.CreateCommand(
                "SELECT * FROM processos;"
            );

            var leitor = (MySqlDataReader)
                comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearProcesso(leitor));
            }

            return lista;
        }

        // Converte uma linha do banco em objeto Processo
        private static Processo MapearProcesso(
            MySqlDataReader leitor)
        {
            return new Processo
            {
                Id = leitor.GetInt32("id_pro"),

                Numero = DAOHelper.GetString(
                    leitor,
                    "numero_pro"
                ),

                Data = DAOHelper.GetDateTime(
                    leitor,
                    "data_pro"
                ),

                Interessado = DAOHelper.GetString(
                    leitor,
                    "interessado_pro"
                ),

                Assunto = DAOHelper.GetString(
                    leitor,
                    "assunto_pro"
                ),

                Descricao = DAOHelper.GetString(
                    leitor,
                    "descricao_pro"
                ),

                Situacao = DAOHelper.GetString(
                    leitor,
                    "situacao_pro"
                )
            };
        }
        // CREATE - insere um novo processo
        public void Inserir(Processo processo)
        {
            try
            {
                using var con = _conexao.GetConnection();

                string sql = @"INSERT INTO processos
                    (numero_pro, data_pro, interessado_pro,
                     assunto_pro, descricao_pro, situacao_pro)
                    VALUES
                    (@numero, @data, @interessado,
                     @assunto, @descricao, @situacao)";

                using var comando = con.CreateCommand();
                comando.CommandText = sql;

                comando.Parameters.AddWithValue("@numero", processo.Numero);
                comando.Parameters.AddWithValue(
                    "@data",
                    processo.Data!.Value.ToDateTime(TimeOnly.MinValue)
                );
                comando.Parameters.AddWithValue("@interessado", processo.Interessado);
                comando.Parameters.AddWithValue("@assunto", processo.Assunto);
                comando.Parameters.AddWithValue("@descricao", processo.Descricao);
                comando.Parameters.AddWithValue("@situacao", processo.Situacao);

                comando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }

    }
}
