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

namespace Ex3.Controllers
{
    
    public class WebController : Controller
    {
        // GET: Web
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult timesPerSecond(string ip, int port, int times)
        {   
            Server ser= Models.Server.getInstance();
            Session["time"] = times;
            return View();
            
        }

        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            // Check whether or not the string ip is IP format, or a filename.
            bool flg = checkIfIP(ip); 
            if (flg) //if its a file-> get the lon and lat
                SendGetCommands(ip,port);
            else // Open file
                LoadFile(ip,port);
           
            return View();
        }

        
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



        void SendGetCommands(string ip, int port)
        {
            NetworkStream netStream;
            Server ser;
            ser = Models.Server.getInstance();
            ser.Ip = ip;
            ser.Port = port;
            TcpClient client = new TcpClient(ip, port);
            netStream = client.GetStream();
            StreamReader streamReader = new StreamReader(netStream);


            float lon = getValue("get /position/longitude-deg\r\n", netStream, streamReader);
            float lat = getValue("get /position/latitude-deg\r\n", netStream, streamReader);


            ViewBag.lon = lon;
            ViewBag.lat = lat;
        }

        void LoadFile(string fileName, int times)
        {

        }
    }
}