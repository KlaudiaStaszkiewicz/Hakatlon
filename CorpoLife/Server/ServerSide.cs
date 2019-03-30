using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grpc.Core;
using MessagesPack;
using System.Device.Location;
using System.Collections.Generic;

namespace Server
{
    class ActiveMember
    {
        int ID;
        string Name;
        GeoCoordinate coords;
        public ActiveMember(int id, string name)
        {
            ID = id;
            Name = name;
        }
        void UpdateCoords(GeoCoordinate cords)
        {
            coords = cords;
        }
    }
    class TalkImpl : ServerEvents.ServerEventsBase
    {
        SqlConnection DBConnection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase");
        List<ActiveMember> activeMembers = new List<ActiveMember>();

        int GetNewWorkerID()
        {
            DBConnection.Open();
            String command = "SELECT COUNT(*) FROM Worker";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int number = (int)newCommand.ExecuteScalar();
            DBConnection.Close();
            return number;
        }
        int GetTeamIdFromName(string name)
        {
            return 0;
        }
        public override Task<WorkerEventResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "INSERT INTO Worker (WorkerID, Password, Name, TeamName, TeamID, DepartmentID, Level) VALUES (@Val1, @val2, @val3, @val4, @val5, @val6, @val7)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int newWorkerID = GetNewWorkerID();
            newCommand.Parameters.AddWithValue("@val1", newWorkerID);
            newCommand.Parameters.AddWithValue("@val2", request.Password);
            newCommand.Parameters.AddWithValue("@val3", request.Name);
            newCommand.Parameters.AddWithValue("@val4", request.Team);
            newCommand.Parameters.AddWithValue("@val5", 1); //TODO get teamID from name
            newCommand.Parameters.AddWithValue("@val6", 2); //TODO get dep ID from name
            newCommand.Parameters.AddWithValue("@val7", request.Level);
            WorkerEventResponse resp = new WorkerEventResponse { State = false, Msg = "Worker cannot be created." };
            try { newCommand.ExecuteNonQuery(); } 
            catch(Exception ex) { Console.WriteLine(ex.Message); }
            resp.State = true;
            resp.Msg = "Worker added with ID: " + newWorkerID;
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(resp);
        }

        public override Task<WorkerEventResponse> LogIn(LoginRequest request, ServerCallContext context)
        {//TODO add to active members list
            DBConnection.Open();
            String command = "SELECT Password FROM Worker WHERE WorkerID =" + request.Id;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            bool correct = false;
            if (dataReader.HasRows)
            {
                if (dataReader.GetString(0) == request.Password)
                {
                    correct = true;
                    DBConnection.Open();
                    command = "";
                    newCommand = new SqlCommand(command, DBConnection);
                    // activeMembers.Add(new ActiveMember(request.Id, ""));
                }
            }
            dataReader.Close();
            DBConnection.Close();
            if (correct)
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker logged in succesfuly." });
            }
            else
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = false, Msg = "Unable to log in." });
            }
        }

        public override Task<WorkerEventResponse> LogOut(LogOutRequest request, ServerCallContext context)
        {//TODO remove from active members list
            bool loggedOut = false;
            //find in List of active memnvers
            //set loggedOut to true
            //delete from that list
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker logged out succesfuly." });
        }

        public override Task<WorkerEventResponse> CoffeBreak(CoffeBreakRequest request, ServerCallContext context)
        {//TODO
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Coffe break ready to be annouced" });
        }

        public override Task<TaskListResponse> GetTaskList(TaskListRequest request, ServerCallContext context)
        {//TODO
            TaskListResponse taskList = new TaskListResponse();
            DBConnection.Open();
            String command = "";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                
            }
            dataReader.Close();
            DBConnection.Close();
            taskList.Tasks.Add(new MessagesPack.Task {Team = "Cool team", Status = 1, TeamID = 0, Text = "w/e"});
            return System.Threading.Tasks.Task.FromResult(taskList);
        }

        public override Task<DepartmentsListResp> GetDepartments(BlankMsg request, ServerCallContext context)
        {//TODO
            DepartmentsListResp tmp = new DepartmentsListResp();
            DBConnection.Open();
            String command = "SELECT * FROM Department";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.DepsDesc.Add(new DepartmentDescription {Index = dataReader.GetInt32(0), Name = dataReader.GetString(1) });
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
    }
    class Server
    {
        const int Port = 50051;
        static void Main(string[] args)
        {
            Grpc.Core.Server server = new Grpc.Core.Server
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
}
