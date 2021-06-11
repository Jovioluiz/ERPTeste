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
                    using (var command = new NpgsqlCommand("insert into produtos (id_item, codigo, nome_produto, un_medida) values (@id, @cod, @nome, @un)", conexao, transacao))
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
                    MessageBox.Show(e.Message, "Erro ao inserir os dados");
                    throw;
                }
            }
            else
            {
                //update
            }

            con.FecharConexao(conexao);

        }

        public void ListaProdutos()
        {
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine(prod.codigo + "\n");
                Console.WriteLine(prod.id_item + "\n");
                Console.WriteLine(prod.nome_produto + "\n");
                Console.WriteLine(prod.un_medida + "\n");
            }
        }
    }
}
