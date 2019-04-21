using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grpc.Core;
using MessagesPack;

namespace DBInformationService
{
    class DBInfServer
    {
        const int Port = 50052;
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
        private readonly SqlConnection _dbConnection =
            new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase2");

        int GetTeamIdFromName(string name)
        {
            _dbConnection.Open();
            var command = "SELECT TeamID FROM Team WHERE TeamName =" + name;
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            var number = dataReader.GetInt32(0);
            dataReader.Close();
            _dbConnection.Close();
            return number;
        }

        string GetWorkerNameFromId(int id)
        {
            _dbConnection.Open();
            String command = "SELECT Name FROM Worker WHERE WorkerID =" + id;
            SqlCommand newCommand = new SqlCommand(command, _dbConnection);
            string name = "";
            SqlDataReader dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
                name = dataReader1.GetString(0);
            }

            dataReader1.Close();
            _dbConnection.Close();
            return name;
        }

        public override Task<TaskListResponse> GetTaskList(TaskListRequest request, ServerCallContext context)
        {
            TaskListResponse taskList = new TaskListResponse();
            _dbConnection.Open();
            String command = "SELECT * FROM ScheduleItem WHERE Team =" + request.TeamName;
            SqlCommand newCommand = new SqlCommand(command, _dbConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                taskList.Tasks.Add(new MessagesPack.Task
                {
                    Status = dataReader.GetString(3), Team = dataReader.GetString(1), TeamID = dataReader.GetInt32(0),
                    Text = dataReader.GetString(4)
                });
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(taskList);
        }

        public override Task<DepartmentsListResp> GetDepartments(BlankMsg request, ServerCallContext context)
        {
            var tmp = new DepartmentsListResp();
            _dbConnection.Open();
            String command = "SELECT * FROM Department;";
            SqlCommand newCommand = new SqlCommand(command, _dbConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.DepsDesc.Add(new DepartmentDescription
                    {Index = dataReader.GetInt32(0), Name = dataReader.GetString(1)});
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<TeamListResp> GetDepartmentWorkers(NameRequest request, ServerCallContext context)
        {
            var tmp = new TeamListResp();
            _dbConnection.Open();
            var command = "SELECT * FROM Worker WHERE DepartmentName = '" + request.TeamName + "'";
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.TeamDesc.Add(new TeamDescription {Index = dataReader.GetInt32(0), Name = dataReader.GetString(2)});
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<TeamListResp> GetDepartmetTeams(NameRequest request, ServerCallContext context)
        {
            TeamListResp tmp = new TeamListResp();
            _dbConnection.Open();
            String command = "SELECT * FROM Team WHERE DepartmentName = '" + request.TeamName + "'";
            SqlCommand newCommand = new SqlCommand(command, _dbConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.TeamDesc.Add(new TeamDescription {Index = dataReader.GetInt32(0), Name = dataReader.GetString(1)});
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<IntegerResponse> GetTeamID(NameRequest request, ServerCallContext context)
        {
            var tmp = new IntegerResponse
            {
                Number = GetTeamIdFromName(request.TeamName)
            };
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<WorkerEventResponse> GetWorkerName(IntegerRequest request, ServerCallContext context)
        {
            var tmp = new WorkerEventResponse
            {
                Msg = GetWorkerNameFromId(request.Number)
            };
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<TaskListResponse> GetTeamSpecificTasks(TeamDescription request, ServerCallContext context)
        {
            var respList = new TaskListResponse();
            var teamID = -1;
            _dbConnection.Open();
            var command = "SELECT TeamID FROM Worker WHERE WorkerID =" + request.Index;
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            if (dataReader.Read())
            {
                teamID = dataReader.GetInt32(0);
                command = "SELECT Team, TeamID, Status, Text FROM ScheduleItem WHERE Status = '" + request.Name +
                          "' AND TeamID =" + teamID;
                newCommand = new SqlCommand(command, _dbConnection);
                dataReader = newCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    respList.Tasks.Add(new MessagesPack.Task
                    {
                        Team = dataReader.GetString(0), TeamID = dataReader.GetInt32(1),
                        Status = dataReader.GetString(2), Text = dataReader.GetString(3)
                    });
                }
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(respList);
        }

        public override Task<TeamDescription> GetDepHead(DepartmentDescription request, ServerCallContext context)
        {
            var headDesc = new TeamDescription();
            _dbConnection.Open();
            var command = "SELECT WorkerID, Name FROM Worker WHERE DepartmentID = " + request.Index;
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            if (dataReader.Read())
            {
                headDesc.Index = dataReader.GetInt32(0);
                headDesc.Name = dataReader.GetString(1);
            }
            else
            {
                headDesc.Index = -1;
                headDesc.Name = "";
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(headDesc);
        }

        public override Task<TaskListResponse>
            GetAllDepTasks(NameRequest request, ServerCallContext context) //TODO this throws error when invoked
        {
            var teams = new TeamListResp();
            _dbConnection.Open();
            var command = "SELECT TeamID, TeamName FROM Team WHERE DepartmentName = '" + request.TeamName + "'";
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                teams.TeamDesc.Add(new TeamDescription
                    {Index = dataReader.GetInt32(0), Name = dataReader.GetString(1)});
            }

            dataReader.Close();
            _dbConnection.Close();
            //get all tasks for these teams
            var taskList = new TaskListResponse();
            foreach (var team in teams.TeamDesc)
            {
                command = "SELECT * FROM ScheduleItem WHERE Team =" + team.Name;
                newCommand = new SqlCommand(command, _dbConnection);
                dataReader = newCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    taskList.Tasks.Add(new MessagesPack.Task
                    {
                        Status = dataReader.GetString(3), Team = dataReader.GetString(1),
                        TeamID = dataReader.GetInt32(0), Text = dataReader.GetString(4)
                    });
                }
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(taskList);
        }

        public override Task<WorkerEventResponse> GetDepFromUser(IntegerRequest request, ServerCallContext context)
        {
            var resp = new WorkerEventResponse();
            _dbConnection.Open();
            var command = "SELECT DepartmentName FROM Worker WHERE DepartmentID =" + request.Number;
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            if (dataReader.Read())
            {
                resp.Msg = dataReader.GetString(0);
                resp.State = true;
            }
            else
            {
                resp.Msg = "Empty";
                resp.State = false;
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(resp);
        }

        public override Task<TeamListResp> GetAllWorkers(BlankMsg request, ServerCallContext context)
        {
            var tmp = new TeamListResp();
            _dbConnection.Open();
            var command = "SELECT WorkerID, Name FROM Worker";
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.TeamDesc.Add(new TeamDescription {Index = dataReader.GetInt32(0), Name = dataReader.GetString(1)});
            }

            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }

        public override Task<GetLeadersResponse> GetLeaders(BlankMsg request, ServerCallContext context)
        {
            var tmp = new GetLeadersResponse();
            _dbConnection.Open();
            var command = "SELECT WorkerID, Name, TeamName, TeamID FROM Worker WHERE Level = 1;";
            var newCommand = new SqlCommand(command, _dbConnection);
            var dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.Leaders.Add(new leaderItem { LeaderId = dataReader.GetInt32(0), LeaderName = dataReader.GetString(1), TeamName = dataReader.GetString(2), TeamId = dataReader.GetInt32(3)});
            }
            dataReader.Close();
            _dbConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
    }
}

