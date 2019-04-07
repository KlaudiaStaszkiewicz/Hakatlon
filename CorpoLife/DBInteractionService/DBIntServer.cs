using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grpc.Core;
using MessagesPack;
using System.Collections.Generic;

namespace DBInteractionService
{
    class DBIntServer
    {
        const int Port = 50053;
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

    class TalkImpl : ServerEvents.ServerEventsBase
    {
        SqlConnection _DbConnection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase2");
        int GetNewDepId()
        {
            var command = "SELECT COUNT(*) FROM Department";
            var newCommand = new SqlCommand(command, _DbConnection);
            var number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewTeamId()
        {
            var command = "SELECT COUNT(*) FROM Team";
            var newCommand = new SqlCommand(command, _DbConnection);
            var number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewWorkerId()
        {
            var command = "SELECT COUNT(*) FROM Worker";
            var newCommand = new SqlCommand(command, _DbConnection);
            var number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewTaskId()
        {
            var command = "SELECT COUNT(*) FROM ScheduleItem";
            var newCommand = new SqlCommand(command, _DbConnection);
            var number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetTeamIdFromName(string name)
        {
            var command = "SELECT TeamID FROM Team WHERE TeamName =" + name;
            var newCommand = new SqlCommand(command, _DbConnection);
            var dataReader = newCommand.ExecuteReader();
            var number = dataReader.GetInt32(0);
            dataReader.Close();
            return number;
        }
        int GetDepIdFromName(string name)
        {
            var command = "SELECT DepartmentID FROM Department WHERE DepName =" + name;
            var newCommand = new SqlCommand(command, _DbConnection);
            var dataReader = newCommand.ExecuteReader();
            var number = dataReader.GetInt32(0);
            dataReader.Close();
            return number;
        }
        public override Task<WorkerEventResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            _DbConnection.Open();
            var command =
                "INSERT INTO Worker (WorkerID, Password, Name, TeamName, TeamID, DepartmentID, Level, DepartmentName) VALUES (@Val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8)";
            var newCommand = new SqlCommand(command, _DbConnection);
            var newWorkerId = GetNewWorkerId();
            newCommand.Parameters.AddWithValue("@val1", newWorkerId);
            newCommand.Parameters.AddWithValue("@val2", request.Password);
            newCommand.Parameters.AddWithValue("@val3", request.Name);
            newCommand.Parameters.AddWithValue("@val4", request.TeamName);
            newCommand.Parameters.AddWithValue("@val5", GetTeamIdFromName(request.TeamName));
            newCommand.Parameters.AddWithValue("@val6", GetDepIdFromName(request.DepName));
            newCommand.Parameters.AddWithValue("@val7", request.Level);
            newCommand.Parameters.AddWithValue("@val8", request.DepName);
            WorkerEventResponse resp = new WorkerEventResponse {State = false, Msg = "Worker cannot be created."};
            try
            {
                newCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            resp.State = true;
            resp.Msg = "Worker added with ID: " + newWorkerId;
            _DbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(resp);
        }
        public override Task<WorkerEventResponse> AddTeam(AddTeamRequest request, ServerCallContext context)
        {
            _DbConnection.Open();
            String command = "INSERT INTO Team (TeamID, TeamName, DepartmentID, DepartmentName) VALUES (@val1, @val2, @val3, @val4)";
            SqlCommand newCommand = new SqlCommand(command, _DbConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewTeamId());
            newCommand.Parameters.AddWithValue("@val2", request.TeamName);
            newCommand.Parameters.AddWithValue("@val3", GetDepIdFromName(request.DepartmentName));
            newCommand.Parameters.AddWithValue("@val4", request.DepartmentName);
            newCommand.ExecuteNonQuery();
            _DbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "New team added!" });
        }
        public override Task<WorkerEventResponse> RemoveTeam(TeamDescription request, ServerCallContext context)
        {
            _DbConnection.Open();
            String command = "DELETE FROM Team WHERE TeamName =" + request.Name;
            SqlCommand newCommand = new SqlCommand(command, _DbConnection);
            newCommand.ExecuteNonQuery();
            _DbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Team deleted sucesfully!" });
        }
        public override Task<WorkerEventResponse> AddDepartment(AddDepRequest request, ServerCallContext context)
        {
            _DbConnection.Open();
            String command = "INSERT INTO Department (DepartmentID, DepName) VALUES (@val1, @val2)";
            SqlCommand newCommand = new SqlCommand(command, _DbConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewDepId());
            newCommand.Parameters.AddWithValue("@val2", request.DepName);
            newCommand.ExecuteNonQuery();
            _DbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "New department added!" });
        }
        public override Task<WorkerEventResponse> AddTask(MessagesPack.Task request, ServerCallContext context)
        {
            var resp = new WorkerEventResponse();
            _DbConnection.Open();
            String command = "INSERT INTO ScheduleItem (Id, Team, TeamID, Status, Text) VALUES (@val1, @val2, @val3,@val4, @val5)";
            SqlCommand newCommand = new SqlCommand(command, _DbConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewTaskId());
            newCommand.Parameters.AddWithValue("@val2", request.Team);
            newCommand.Parameters.AddWithValue("@val3", request.TeamID);
            newCommand.Parameters.AddWithValue("@val4", request.Status);
            newCommand.Parameters.AddWithValue("@val5", request.Text);
            try
            {
                newCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _DbConnection.Close();
            }
            _DbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Task added successfully.", State = true });
        }
    }
}
