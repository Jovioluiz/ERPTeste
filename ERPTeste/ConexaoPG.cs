using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using System.Xml.Linq;

namespace ERPTeste
{
    public class ConexaoPG
    {
        private static string host;
        private static string user;
        private static string dbName;
        private static string password;
        private static string port;

        public NpgsqlConnection ConectaBanco()
        {
            try
            {
                //pega as configurações da conexão do arquivo xml
                XElement xml = XElement.Load("config.xml");

                foreach (XElement config in xml.Elements())
                {
                    host = config.Attribute("host").Value;
                    user = config.Attribute("user").Value;
                    dbName = config.Attribute("dbname").Value;
                    password = config.Attribute("password").Value;
                    port = config.Attribute("port").Value;
                }

                string conexao = "Server=" + host + ";Username=" + user + ";Database=" + dbName + ";Port=" + port + ";Password=" + password + ";";

                NpgsqlConnection conex = new NpgsqlConnection(conexao);
                conex.Open();

                return conex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao conectar no banco");
                return null;
            }
        }

        public void FecharConexao(NpgsqlConnection conexao)
        {
            if (conexao.State == System.Data.ConnectionState.Open)
                conexao.Close();
        } 
        
    }
}
