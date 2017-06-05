using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using static XMLData.XMLData;

namespace XMLMain
{
    class XMLMain
    {
        static void Main(string[] args)
        {
            // Salary data
            List<SalaryData> data = new List<SalaryData>();

            // Read the XML data and output the data
            var reader = new StreamReader("data.xml");
            var serializer = new XmlSerializer(data.GetType());
            data = serializer.Deserialize(reader) as List<SalaryData>;
            reader.Close();
            foreach (var datum in data)
                Console.WriteLine(datum);
        }
    }
}
