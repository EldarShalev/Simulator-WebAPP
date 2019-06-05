using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class Server
    {
        private static Server server=null;

        private Server() { }
        public static Server instance {
            get
            {
                if (server == null)
                {
                    server = new Server();
                }
                return server;
            }
        }
        public string Ip { get; set; }
        public int Port { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("Parameters");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }
    }
}