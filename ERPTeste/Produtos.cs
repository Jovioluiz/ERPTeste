using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;

namespace ERPTeste
{
    class Produtos
    {

        InfoProdutos prod;

        public Produtos()
        {
            InfoProdutos[] prod = new InfoProdutos[1];
        }
        private struct InfoProdutos
        {
            public string codigo;
            public Int64 id_item;
            public string nome_produto;
            public string un_medida;
        }

        private Boolean InserirProduto(Int64 IdItem)
        {
            Int64 id = 0;
            ConexaoPG con = new ConexaoPG();
            NpgsqlConnection conexao = con.ConectaBanco();

            using (var select = new NpgsqlCommand("select id_item from produtos where id_item = @idItem", conexao))
            {
                select.Parameters.AddWithValue("idItem", IdItem);
                NpgsqlDataReader query = select.ExecuteReader();

                //vai para o proximo registro
                while (query.Read())
                {
                    id = Int64.Parse(query["id_item"].ToString());
                }

                return id == 0;
            }
        }

        public void AdicionaProdutos()
        {
            const string insert = "insert into produtos (id_item, codigo, nome_produto, un_medida) values (@id, @cod, @nome, @un)";
            const string update = "update produtos set codigo = @codigo, nome_produto = @nome_produto, un_medida = @un_medida where id_item = @id_item";

            //conexao com o banco
            ConexaoPG con = new ConexaoPG();

            NpgsqlConnection conexao = con.ConectaBanco();

            //transacao
            NpgsqlTransaction transacao = conexao.BeginTransaction();

            Console.WriteLine("Código do Produto: ");
            prod.codigo = Console.ReadLine();
            Console.WriteLine("ID do Produto: ");
            prod.id_item = int.Parse(Console.ReadLine());
            Console.WriteLine("Nome do Produto: ");
            prod.nome_produto = Console.ReadLine();
            Console.WriteLine("Unidade de medida do Produto: ");
            prod.un_medida = Console.ReadLine();

            if (InserirProduto(prod.id_item))
            {
                try
                {
                    using (var command = new NpgsqlCommand(insert, conexao, transacao))
                    {
                        command.Parameters.AddWithValue("id", prod.id_item);
                        command.Parameters.AddWithValue("cod", prod.codigo);
                        command.Parameters.AddWithValue("nome", prod.nome_produto);
                        command.Parameters.AddWithValue("un", prod.un_medida);

                        int nRows = command.ExecuteNonQuery();
                        transacao.Commit();

                        Console.WriteLine(String.Format("Números de inserts = {0}", nRows));
                    }
                }
                catch (Exception e)
                {
                    transacao.Rollback();
                    MessageBox.Show("Erro ao inserir os dados do produto " + prod.nome_produto, e.Message);
                    throw;
                }
            }
            else
            {
                try
                {
                    using (var comando = new NpgsqlCommand(update, conexao, transacao))
                    {
                        comando.Parameters.AddWithValue("codigo", prod.codigo);
                        comando.Parameters.AddWithValue("nome_produto", prod.nome_produto);
                        comando.Parameters.AddWithValue("un_medida", prod.un_medida);
                        comando.Parameters.AddWithValue("id_item", prod.id_item);

                        int nRows = comando.ExecuteNonQuery();
                        transacao.Commit();
                    }

                }
                catch (Exception e)
                {
                    transacao.Rollback();
                    MessageBox.Show(e.Message, "Erro ao atualizar os dados do item " + prod.nome_produto);
                    throw;
                }
            }

            con.FecharConexao(conexao);

        }


        public void ListaProdutos()
        {
            ConexaoPG con = new ConexaoPG();
            NpgsqlConnection conexao = con.ConectaBanco();

            using (var s = new NpgsqlCommand("select id_item, codigo, nome_produto, un_medida from produtos", conexao))
            {
                NpgsqlDataReader query = s.ExecuteReader();

                //vai para o proximo registro
                while (query.Read())
                {
                    Console.WriteLine(string.Format("ID: {0}", query.GetInt32(0).ToString()));
                    Console.WriteLine(string.Format("Código: {1}", query.GetString(1)));
                    Console.WriteLine(string.Format("Nome Produto: {2}", query.GetString(2)));
                    Console.WriteLine(string.Format("UN Medida: {3}", query.GetString(3)));
                }

            }
            con.FecharConexao(conexao);
        }
    }
}
