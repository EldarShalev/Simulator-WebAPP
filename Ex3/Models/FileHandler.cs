using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FileHandler
    {
        private static FileHandler instance = null;
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";
        private  List<string> dataFromFile = null;
        private IEnumerator reader = null;

        public static FileHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileHandler();
                }
                return instance;
            }
        }


        public string Next
        {
            get
            {
                if (dataFromFile == null) return "";
                if (reader.MoveNext())
                {
                    return Convert.ToString(reader.Current);
                }
                else
                {
                    return "END";
                }
            }
        }


        public void WriteToFile(string fileName, string lon, string lat,
           string throttle, string rudder)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(lon + "," + lat + "," + throttle + "," + rudder);
            }
        }



        public void ReadData(string filename)
        {
            dataFromFile = new List<string>();
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, filename));
            using (System.IO.StreamReader fileReader = new System.IO.StreamReader(path, true))
            {
                string line;
                while ((line = fileReader.ReadLine()) != null)
                {
                    string [] parsed = line.Split(',');
                    dataFromFile.Add(line);
                }
            }
            reader = dataFromFile.GetEnumerator();
        }
    }


}
