using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Weitrest
    {
        public TcpClient Client { get; }
        public User User { get; }

        public Weitrest(TcpClient client, User user)
        {
            User = user;
            Client = client;
        }
    }
}
