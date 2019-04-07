using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grpc.Core;
using MessagesPack;
using System.Collections.Generic;
using System.Data.Common;

namespace RealTimeService
{
    class RTServer
    {
        private const int Port = 50051;
        static void Main(string[] args)
        {
            var server = new Server
            {
                Services = { MessagesPack.ServerEvents.BindService(new TalkImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();
            Console.WriteLine("Server listening on port " + Port);

            Console.WriteLine("Press any key to stop the server.");
            Console.ReadKey();
            server.ShutdownAsync().Wait();
        }
    }

    class ActiveMember
    {
        public int ID;

        public string Name;

        //public GeoCoordinate coords;
        public ActiveMember(int id, string name)
        {
            ID = id;
            Name = name;
        }

        //void UpdateCoords(GeoCoordinate cords)
        //{
        //    coords = cords;
        //}
    }

    class EmergencyCaller
    {
        public int InviterId;
        public int TeamId;

        public EmergencyCaller()
        {
            InviterId = -1;
            TeamId = -1;
        }
    }

    class CoffieInviter
    {
        public string InviterName;
        public bool Inviting;

        public CoffieInviter()
        {
            InviterName = "";
            Inviting = false;
        }
    }

    class TalkImpl : ServerEvents.ServerEventsBase
    {
        EmergencyCaller _emergencyCaller = new EmergencyCaller();
        CoffieInviter _coffieInviter = new CoffieInviter();
        SqlConnection _DBConnection =
            new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase2");
        private List<ActiveMember> _activeMembers = new List<ActiveMember>();

        private int GetWorkerTeamIdFromId(int id)
        {
            var command = "SELECT TeamID FROM Worker WHERE WorkerID =" + id;
            var newCommand = new SqlCommand(command, _DBConnection);
            var teamId = -1;
            var dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
                teamId = Convert.ToInt32(dataReader1.GetString(0));
            }
            dataReader1.Close();
            return teamId;
        }
        private string GetWorkerTeamFromId(int id)
        {
            var command = "SELECT TeamName FROM Worker WHERE WorkerID =" + id;
            var newCommand = new SqlCommand(command, _DBConnection);
            var name = "";
            var dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
                name = dataReader1.GetString(0);
            }
            dataReader1.Close();
            return name;
        }
        private string GetWorkerNameFromId(int id)
        {
            var command = "SELECT Name FROM Worker WHERE WorkerID =" + id;
            var newCommand = new SqlCommand(command, _DBConnection);
            var name = "";
            var dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
            name = dataReader1.GetString(0);
            }
            dataReader1.Close();
            return name;
        }
        private int GetWorkerLevelFromId(int id)
        {
            var command = "SELECT Level FROM Worker WHERE WorkerID =" + id;
            var newCommand = new SqlCommand(command, _DBConnection);
            var lvl = -1;
            var dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
                lvl = Convert.ToInt32(dataReader1.GetString(0));
            }
            dataReader1.Close();
            return lvl;
        }
        public override Task<LogInResponse> LogIn(LoginRequest request, ServerCallContext context)
        {
            _DBConnection.Open();
            var command = "SELECT Password FROM Worker WHERE WorkerID =" + request.Id;
            var newCommand = new SqlCommand(command, _DBConnection);
            var dataReader = newCommand.ExecuteReader();
            var correct = false;
            var lvl = -1;
            if (dataReader.Read())
            {
                if (dataReader.GetString(0) == request.Password)
                {
                    correct = true;
                    dataReader.Close();
                    _activeMembers.Add(new ActiveMember(request.Id, GetWorkerNameFromId(request.Id)));
                    lvl = GetWorkerLevelFromId(request.Id);
                }
            }
            _DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(correct
                ? new LogInResponse { State = true, Msg = "Worker logged in successfully.", Level = lvl, Name = GetWorkerNameFromId(request.Id),
                    TeamId = GetWorkerTeamIdFromId(request.Id), Team = GetWorkerTeamFromId(request.Id)}
                : new LogInResponse { State = false, Msg = "Unable to log in."});
        }
        public override Task<WorkerEventResponse> LogOut(LogOutRequest request, ServerCallContext context)
        {
            var loggedOut = false;
            for (var i = 0; i < _activeMembers.Count; i++)
            {
                if (_activeMembers[i].ID != request.Id) continue;
                loggedOut = true;
                _activeMembers.RemoveAt(i);
            }

            return System.Threading.Tasks.Task.FromResult(loggedOut
                ? new WorkerEventResponse {State = true, Msg = "Worker logged out successfully."}
                : new WorkerEventResponse {State = false, Msg = "Cannot log out."});
        }
        public override Task<WorkerEventResponse> PullCoffeeBrake(CoffeBreakRequest request, ServerCallContext context)
        {
            return _coffieInviter.Inviting && _coffieInviter.InviterName != request.Name
                ? System.Threading.Tasks.Task.FromResult(new WorkerEventResponse
                    {Msg = "Read status of coffee.", State = true})
                : System.Threading.Tasks.Task.FromResult(new WorkerEventResponse
                    {Msg = "Read status of coffee", State = false});
        }
        public override Task<WorkerEventResponse> PullEmergency(IntIntRequest request, ServerCallContext context)
        {
            return _emergencyCaller.TeamId != -1 && _emergencyCaller.TeamId == request.TeamID
                ? System.Threading.Tasks.Task.FromResult(new WorkerEventResponse
                    {Msg = "Read status of emergency.", State = true})
                : System.Threading.Tasks.Task.FromResult(new WorkerEventResponse
                    {Msg = "Read status of emergency", State = false});
        }
        //TODO reforge it so that more people can call emergency (also refer to PullEmergency after change)
        public override Task<WorkerEventResponse> CallEmergency(IntIntRequest request, ServerCallContext context)
        {
            _emergencyCaller.InviterId = request.WorkerID;
            _emergencyCaller.TeamId = request.TeamID;
            System.Threading.Tasks.Task.Delay(50000).ContinueWith(_ =>
            {
                _emergencyCaller.InviterId = -1;
                _emergencyCaller.TeamId = -1;
            });
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Emergency called!", State = true });
        }
        public override Task<WorkerEventResponse> CallCoffeeBreak(CoffeBreakRequest request, ServerCallContext context)
        {
            _coffieInviter.InviterName = request.Name;
            _coffieInviter.Inviting = true;
            System.Threading.Tasks.Task.Delay(50000).ContinueWith(_ =>
            {
                _coffieInviter.InviterName = "";
                _coffieInviter.Inviting = false;
            }
            );
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Smacznego!" });
        }
    }
}
