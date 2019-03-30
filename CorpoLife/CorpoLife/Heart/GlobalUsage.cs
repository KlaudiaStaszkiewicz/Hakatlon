using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagesPack;
using Grpc.Core;

namespace CorpoLife
{
    static class GlobalUsage
    {
        static public Worker currentUser = new Worker();
        
        public static ServerEvents.ServerEventsClient Client()
        {
            Channel channel = new Channel("192.168.52.55:50051", ChannelCredentials.Insecure);
            return new ServerEvents.ServerEventsClient(channel);
        }
    }
}
