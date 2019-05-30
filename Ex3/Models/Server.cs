using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class Server
    {
        private static Server server;

        private Server() { }
        public static Server getInstance() {
            if (server == null) {
                    server = new Server();
            }
            return server;
        }
        public string Ip { get; set; }
        public int Port { get; set; }
        public float Lan { get; set; }
        public float Lon { get; set; }
    }
}