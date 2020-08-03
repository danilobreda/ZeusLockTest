using System;
using System.IO;
using System.Threading;

namespace TesteCacheZeus
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<teste>();
            string xml = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "xml.xml"));

            Console.WriteLine("Digite 1 para teste sem lock e 2 para teste com lock => ");

            string saida = Console.ReadLine();
            if (saida == "1")
            {
                Console.WriteLine("Iniciando processo SEM lock");
                Thread threadA = new Thread(() => FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml));
                Thread threadB = new Thread(() => FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml));
                Thread threadC = new Thread(() => FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(xml));

                threadA.Start();
                threadB.Start();
                threadC.Start();
            }
            else if (saida == "2")
            {
                Console.WriteLine("Iniciando processo COM lock");
                Thread threadA = new Thread(() => FuncoesXmlLock.XmlStringParaClasse<NFe.Classes.NFe>(xml));
                Thread threadB = new Thread(() => FuncoesXmlLock.XmlStringParaClasse<NFe.Classes.NFe>(xml));
                Thread threadC = new Thread(() => FuncoesXmlLock.XmlStringParaClasse<NFe.Classes.NFe>(xml));

                threadA.Start();
                threadB.Start();
                threadC.Start();
            }

            Console.WriteLine("Processo Finalizado");
        }
    }
}
