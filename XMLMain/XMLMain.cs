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
    public class XMLMain
    {
        public static List<SalaryData> loadData()
        {
            List<SalaryData> data = new List<SalaryData>();

            // Read the XML data and output the data
            var reader = new StreamReader("data.xml");
            var serializer = new XmlSerializer(data.GetType());
            data = serializer.Deserialize(reader) as List<SalaryData>;
            reader.Close();

            return data;
        }

        static void Main(string[] args)
        {
            // Load the salary data and display it
            List<SalaryData> data = loadData();

            foreach (var datum in data)
                Console.WriteLine(datum);
        }
    }
}
