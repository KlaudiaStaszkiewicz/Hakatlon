using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagesPack;
using Grpc.Core;

namespace Heart
{
    class GlobalUsage
    {
        public static ServerEvents.ServerEventsClient Client()
        {
            Channel channel = new Channel("192.168.43.241:50051", ChannelCredentials.Insecure);
            var client = new ServerEvents.ServerEventsClient(channel);
            return client;
        }
    }
}
