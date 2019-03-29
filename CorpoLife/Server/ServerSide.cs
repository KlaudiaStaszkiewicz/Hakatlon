using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grpc.Core;
using MessagesPack;

namespace Server
{
    class TalkImpl : ServerEvents.ServerEventsBase
    {
        SqlConnection DBConnection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase");
        public TalkImpl()
        {
            try
            {
                DBConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        int GetNetWorkerID()
        {
            String command = "SELECT COUNT(*) FROM Worker";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            Int32 number = (Int32)newCommand.ExecuteScalar();
            return number;
        }
        public override Task<WorkerEventResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            String command = "INSERT INTO Worker (WorkerID, Password, Name, TeamName, TeamID, Status, Level) VALUES (@Val1, @val2, @val3, @val4, @val5, @val6, @val7)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int newWorkerID = GetNetWorkerID();
            newCommand.Parameters.AddWithValue("@val1", newWorkerID);
            newCommand.Parameters.AddWithValue("@val2", request.Password);
            newCommand.Parameters.AddWithValue("@val3", request.Name);
            newCommand.Parameters.AddWithValue("@val4", request.Team);
            newCommand.Parameters.AddWithValue("@val5", request.TeamID);
            newCommand.Parameters.AddWithValue("@val6", request.Status);
            newCommand.Parameters.AddWithValue("@val7", request.Level);
            try { newCommand.ExecuteNonQuery(); } 
            catch(Exception ex) { Console.WriteLine(ex.Message); }
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker registered without any specific errors that I can send You right now." });
        }

        public override Task<WorkerEventResponse> LogIn(LoginRequest request, ServerCallContext context)
        {
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker logged in succesfuly." });
        }

        public override Task<WorkerEventResponse> LogOut(LogOutRequest request, ServerCallContext context)
        {
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker logged out succesfuly." });
        }

        public override Task<WorkerEventResponse> CoffeBreak(CoffeBreakRequest request, ServerCallContext context)
        {
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Coffe break ready to be annouced" });
        }

        public override Task<TaskListResponse> GetTaskList(TaskListRequest request, ServerCallContext context)
        {
            TaskListResponse taskList = new TaskListResponse();
            taskList.Tasks.Add(new MessagesPack.Task {Team = "Cool team", Status = 1, TeamID = 0, Text = "w/e"});
            return System.Threading.Tasks.Task.FromResult(taskList);
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
