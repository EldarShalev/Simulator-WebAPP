using Ex3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    
    public class WebController : Controller
    {
        private TimeHandler timer = TimeHandler.Instance;

        // GET: Web
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult saveToFile(string ip, int port, int rate, int time, string fileName)
        {
            Server ser = Models.Server.instance;
            NetworkStream netStream;
            TcpClient client = new TcpClient(ip, port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);
            Session["file"] = fileName;
            ser.Ip = ip;
            ser.Port = port;
            ViewBag.time = rate;
            ViewBag.file = fileName;
            timer.Interval = time;
            return View();
        }

        [HttpPost]
        public string RouteSaver()
        {
            FileHandler fh = Models.FileHandler.Instance;
            timer.StartTimer();
            Server ser = Models.Server.instance;
            NetworkStream netStream;
            TcpClient client = new TcpClient(ser.Ip, ser.Port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);
            SendGetCommands(ser);
            if (timer.isRunning) // need to save to file 
            {
                ser.Lon += 20;
                ser.Lat += 20;
                float throttle = getValue("get /controls/engines/current-engine/throttle\r\n", netStream, streamReader);
                float rudder = getValue("get /controls/flight/rudder\r\n", netStream, streamReader);
                fh.WriteToFile(Convert.ToString(Session["file"]), ser.Lon.ToString(), ser.Lat.ToString(), throttle.ToString(), rudder.ToString());

            }



            return ToXml(ser);
        }




        [HttpGet]
        public ActionResult timesPerSecond(string ip, int port, int times)
        {
            NetworkStream netStream;
            Server ser = Models.Server.instance;
            ser.Ip = ip;
            ser.Port = port;
            TcpClient client = new TcpClient(ip, port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);
            SendGetCommands(ser);
           

            Session["time"] = times;
            return View();
            
        }

        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            // Check whether or not the string ip is IP format, or a filename.
            bool flg = checkIfIP(ip);
            if (flg)
            {//if its a file-> get the lon and lat
                Server ser = Models.Server.instance;
                ser.Ip = ip;
                ser.Port = port;
                SendGetCommands(ser);
                ViewBag.lon = ser.Lon;
                ViewBag.lat = ser.Lat;
                return View();
            }
            else {// Open file

                return LoadFile(ip, port);
            }
                

            
         
        }

        [HttpPost]
        public string GetServer()
        {
            Server ser = Models.Server.instance;
            NetworkStream netStream;
            TcpClient client = new TcpClient(ser.Ip, ser.Port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);
            SendGetCommands(ser);
            
            return ToXml(ser);
        }

        /**
         * Parse the data into XML
         */ 
        private string ToXml(Server server)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Parameters");

            server.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        /**
         * Check if the given text is valid IP format.
         */
        bool checkIfIP(string text)
        {
            foreach (char i in text)
            {
                if (!(Char.IsDigit(i) || i == '.')){
                    return false ;
                }
                    
            }
            return true; 
        }

        float getValue(string line, NetworkStream nt,StreamReader sr )
        {
            nt.Flush();
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(line);
            nt.Write(data, 0, data.Length);
            string data1 = sr.ReadLine();
            string resultString = string.Empty;
            bool charWasFound = false;
            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] == '=')
                {
                    charWasFound = true;
                }
                if (charWasFound)
                {
                    if (Char.IsDigit(data1[i]) || data1[i] == '-' || data1[i]=='.')
                        resultString += data1[i];
                }
                
            }
            float parsedFloat = float.Parse(resultString);
            return parsedFloat;

        }



        public void SendGetCommands(Server ser)
        {
            NetworkStream netStream;
           
            TcpClient client = new TcpClient(ser.Ip, ser.Port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);


            float lon = getValue("get /position/longitude-deg\r\n", netStream, streamReader);
            float lat = getValue("get /position/latitude-deg\r\n", netStream, streamReader);
            ser.Lon = lon;
            ser.Lat = lat;

        }

        public ActionResult LoadFile(string fileName, int times)
        {
            FileHandler.Instance.ReadData(fileName);
            ViewBag.time = times;
            return View("displayFlight");

        }



        [HttpPost]
        public string GetState()
        {
            Server ser = Models.Server.instance;
            string input = FileHandler.Instance.Next;
            List<string> parsedValues = new List<string>();
            if (input == "") parsedValues.Add("error");
            else if (input == "END")
            {
                parsedValues.Add("END");
            }else
            {
                parsedValues = input.Split(',').ToList();
                ser.Lon = float.Parse(parsedValues[0]);
                ser.Lat = float.Parse(parsedValues[1]);
            }
            return ToXml(ser);
        }

    }
}