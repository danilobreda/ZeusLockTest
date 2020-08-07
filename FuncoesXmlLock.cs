using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TesteCacheZeus
{
    public static class FuncoesXmlLock
    {
        private static readonly Hashtable CacheSerializers = new Hashtable();

        /// <summary>
        ///     Serializa a classe passada para uma string no form
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objeto"></param>
        /// <returns></returns>
        public static string ClasseParaXmlString<T>(T objeto)
        {
            XElement xml;
            var keyNomeClasseEmUso = typeof(T).FullName;
            var ser = BuscarNoCache(keyNomeClasseEmUso, typeof(T));

            using (var memory = new MemoryStream())
            {
                using (TextReader tr = new StreamReader(memory, Encoding.UTF8))
                {
                    ser.Serialize(memory, objeto);
                    memory.Position = 0;
                    xml = XElement.Load(tr);
                    xml.Attributes().Where(x => x.Name.LocalName.Equals("xsd") || x.Name.LocalName.Equals("xsi")).Remove();
                }
            }
            return XElement.Parse(xml.ToString()).ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        ///     Deserializa a classe a partir de uma String contendo a estrutura XML daquela classe
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T XmlStringParaClasse<T>(string input) where T : class
        {
            var keyNomeClasseEmUso = typeof(T).FullName;
            var ser = BuscarNoCache(keyNomeClasseEmUso, typeof(T));

            using (var sr = new StringReader(input))
                return (T)ser.Deserialize(sr);
        }

        private static XmlSerializer BuscarNoCache(string chave, Type type)
        { 
            if (CacheSerializers.Contains(chave))
            {
                return (XmlSerializer)CacheSerializers[chave];
            }

            lock (type)
            {
                if (CacheSerializers.Contains(chave))
                {
                    return (XmlSerializer)CacheSerializers[chave];
                }

                var ser = XmlSerializer.FromTypes(new[] { type })[0];
                CacheSerializers.Add(chave, ser);

                return ser;
            }
        }
    }
}