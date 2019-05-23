using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    
    public class WebController : Controller
    {
        bool stop = false;
        // GET: Web
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port)
        {
            Server ser = new Server();
           ConnectToServer(ip, port, ser);
            return View();
        }

        public void ConnectToServer(string ip, int port, Server s)
        {
            IPAddress iPAddress = IPAddress.Parse(ip);
            IPEndPoint localEndPoint = new IPEndPoint(iPAddress, port);
            Socket listener = new Socket(iPAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

                // Using Bind() method we associate a 
                // network address to the Server Socket 
                // All client that will connect to this  
                // Server Socket must know this network 
                // Address 
                listener.Bind(localEndPoint);

                // Using Listen() method we create  
                // the Client list that will want 
                // to connect to Server 
                listener.Listen(10);

                // listen always on new thread
                Thread t1 = new Thread(delegate ()
                {
                    Console.WriteLine("Waiting connection ... ");
                    // Suspend while waiting for 
                    // incoming connection Using  
                    // Accept() method the server  
                    // will accept connection of client 
                    Socket clientSocket = listener.Accept();
                    while (!stop)
                    {
                        // Data buffer 
                        byte[] bytes = new Byte[1024];
                        string data = null;
                        //int numByte = clientSocket.Receive(bytes);
                        //data = ASCIIEncoding.ASCII.GetString(bytes, 0, numByte);
                        //if (data != null)
                        //{
                            //ParseLonAndLat(data);
                            stop = true;
                            s.Lan = 31.1234f;
                            s.Lon = -168.1234f;
                            ViewData["Message"] = s;
                            
                        
                            RedirectToAction("display");
                        View();
                            

                        //}
                    }
                      });
                    t1.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}