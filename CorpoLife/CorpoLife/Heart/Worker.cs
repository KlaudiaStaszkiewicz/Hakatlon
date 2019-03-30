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
        public Worker()
        {
            level = 2;
            workerID = 1;
            name = "Brajan";
            teamID = 1;
        }
        Timer timer = new System.Timers.Timer(TimeSpan.FromSeconds(40).TotalMilliseconds);
        void Init()
        {
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(MyMethod);
            timer.Start();
        }

        public static void MyMethod(object sender, ElapsedEventArgs e)
        {
            var responceEm = GlobalUsage.Client().PullEmergency(new IntIntRequest { TeamID = GlobalUsage.currentUser.teamID, WorkerID = GlobalUsage.currentUser.workerID });
            var responceCo = GlobalUsage.Client().PullCoffeBrake(new CoffeBreakRequest { Name = GlobalUsage.currentUser.name });
            if(responceCo.State)
            {
                CoffeeBreak coffee = new CoffeeBreak();
                coffee.Show();
            }
            if (responceEm.State)
            {
                Emergency emer = new Emergency();
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

