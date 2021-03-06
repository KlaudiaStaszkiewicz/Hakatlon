﻿using MessagesPack;
using Grpc.Core;

namespace CorpoLife
{
    static class GlobalUsage
    {
        static public Worker CurrentUser = new Worker();

        public static ServerEvents.ServerEventsClient GetRtClient()
        {
            return new ServerEvents.ServerEventsClient(new Channel("192.168.1.41:50051", ChannelCredentials.Insecure));
        }
        public static ServerEvents.ServerEventsClient GetInfClient()
        {
            return new ServerEvents.ServerEventsClient(new Channel("192.168.1.41:50052", ChannelCredentials.Insecure));
        }
        public static ServerEvents.ServerEventsClient GetIntClient()
        {
            return new ServerEvents.ServerEventsClient(new Channel("192.168.1.41:50053", ChannelCredentials.Insecure));
        }
    }
}
