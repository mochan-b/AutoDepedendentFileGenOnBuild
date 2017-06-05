using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLData
{
    /// <summary>
    /// Contains the data that we will use to create the XML file
    /// </summary>    
    public class XMLData
    {
        /// <summary>
        /// Class that holds the salary information
        /// </summary>
        [Serializable]
        public class SalaryData
        {
            [XmlAttribute]
            public string FirstName, LastName;
            [XmlAttribute("Salary")]
            public int Salary;

            public override string ToString()
            {
                return FirstName + " " + LastName + " \t\t: $" + Salary + ",000";
            }
        }

        // Data that we will write to the XML file
        List<SalaryData> data = new List<SalaryData>();

        /// <summary>
        /// Read the input file that is comma separated data and output as XML file
        /// </summary>
        /// <param name="inputfile">Name of input file</param>
        public XMLData(string inputfile)
        {
            // Read the comma separated data of 5 items and split them by the names
            var lines = System.IO.File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var splits = line.Split(new char[] { ',' });
                data.Add(new SalaryData() { FirstName = splits[0], LastName = splits[1], Salary = Convert.ToInt32(splits[2]) });
            }

            // Save the file as data.xml
            var writer = new StreamWriter("data.xml");
            var serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(writer, data);
            writer.Close();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var data = new XMLData("");
        }
    }
}
