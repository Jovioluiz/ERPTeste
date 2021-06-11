using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPTeste
{
    class Program
    {
        enum Opcao { Adicionar = 1, Visualizar, Atualizar, Deletar, Sair };
        static void Main(string[] args)
        {
            int index;
            Produtos p = new Produtos();

            do
            {
                Console.WriteLine("Selecione uma opção: ");
                Console.WriteLine("1-Adicionar\n2-Visualizar\n3-Atualizar\n4-Deletar\n5-Sair");
                index = int.Parse(Console.ReadLine());
                Opcao opcaoSelecionada = (Opcao)index; //cast para o enum
                
                switch (opcaoSelecionada)
                {
                    case Opcao.Adicionar:
                        p.AdicionaProdutos();                       
                        break;
                    case Opcao.Visualizar:
                        p.ListaProdutos();
                        break;
                    case Opcao.Atualizar:
                        break;
                    case Opcao.Deletar:
                        break;
                    case Opcao.Sair:
                        break;
                    default:
                        Console.WriteLine("Opção Inválida!");
                        break;
                }

            } while ((Opcao)index != Opcao.Sair);
            
        }
    }
}
