using ImLazy.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImLazy.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var addinLazy in AddinHost.Instance.ConditionAddins)
            {
                //if(addinLazy is FilePropertyConditionAddin)
            }
            //foreach (var addinLazy in AddinHost.Instance.ConditionAddins)
            //{
            //    Console.WriteLine("found addin : \n\tname:{0}", addinLazy.Metadata.Name);
            //    var addin = addinLazy.Value;
            //    addin.Configurate(new Dictionary<string, object>() { { "Extensions", ".txt" }, { "Symbol", "==" } });
            //    var match = addin.IsMatch(@"C:\log.txt");
            //    Console.WriteLine("IsMatch:{0}", match);
            //    Condition c = new Condition() 
            //    {
            //        AddinName = addinLazy.Metadata.Name,
            //        Config = new SerializableDictionary<string, object>() { { "Extensions", ".txt" }, { "Symbol", "==" } }
            //    };
            //    XmlSerializer xs = new XmlSerializer(typeof(Condition));
            //    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(String.Format("d:/desktop/{0}.xml",addinLazy.Metadata.Name), false))
            //    {
            //        xs.Serialize(sw, c);
            //    }
            //}
        }
    }
}
