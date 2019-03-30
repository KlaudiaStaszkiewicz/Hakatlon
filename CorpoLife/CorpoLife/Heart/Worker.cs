using System;
using MessagesPack;
using Grpc.Core;
namespace CorpoLife
{
    class Worker
    {
        public int workerID, teamID, level, status;
        public string name, teamName;
        enum Status { absent = 0, working, onCoffee }
        enum Level { casual = 1, leader, head, admin };
        public Worker()
        {
            level = 1;
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

