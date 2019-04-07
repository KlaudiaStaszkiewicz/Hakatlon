using System;
using MessagesPack;
using Grpc.Core;
using System.Timers;
namespace CorpoLife
{
    class Worker
    {
        public bool coffee, emergency;
        public int workerID, teamID, level, status;
        public string name, teamName;
        enum Status { absent = 0, working, onCoffee }
        enum Level { casual = 1, leader, head, admin };
        Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(40).TotalMilliseconds);
        void Init()
        {
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckEventsStatus);
            timer.Start();
        }

        public static void CheckEventsStatus(object sender, ElapsedEventArgs e)
        {
            var responseEm = GlobalUsage.GetRtClient().PullEmergency(new IntIntRequest { TeamID = GlobalUsage.CurrentUser.teamID, WorkerID = GlobalUsage.CurrentUser.workerID });
            var responseCo = GlobalUsage.GetRtClient().PullCoffeeBrake(new CoffeBreakRequest { Name = GlobalUsage.CurrentUser.name });
            if(responseCo.State)
            {
                var coffee = new CoffeeBreak();
                coffee.Show();
            }
            if (responseEm.State)
            {
                var emer = new Emergency();
                emer.Show();
            }
        }

    
        public void LogIn()
        {

        }

        public void LogOut()
        {

        }

        public void MakeCoffeeBreak()
        {

        }

        public void CheckCoffeeBreak()
        {

        }

        public void UpdatePosition()
        {

        }
        
    }
}

