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
        public int ID;
        public string Name;
        public GeoCoordinate coords;
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
        EmergencyCaller emergencyCaller = new EmergencyCaller();
        CoffieInviter coffieInviter = new CoffieInviter();
        SqlConnection DBConnection = new SqlConnection("Server=localhost;Integrated security=SSPI;database=ProjectDatabase2");
        List<ActiveMember> activeMembers = new List<ActiveMember>();

        int GetNewDepID()
        {
            String command = "SELECT COUNT(*) FROM Department";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewTeamID()
        {
            String command = "SELECT COUNT(*) FROM Team";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewWorkerID()
        {
            String command = "SELECT COUNT(*) FROM Worker";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetNewTaskID()
        {
            String command = "SELECT COUNT(*) FROM ScheduleItem";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int number = (int)newCommand.ExecuteScalar();
            return number;
        }
        int GetTeamIdFromName(string name)
        {
            String command = "SELECT TeamID FROM Team WHERE TeamName =" + name;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            int number = dataReader.GetInt32(0);
            dataReader.Close();
            return number;
        }
        int GetDepIdFromName(string name)
        {
            String command = "SELECT DepartmentID FROM Department WHERE DepName =" + name;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            int number = dataReader.GetInt32(0);
            dataReader.Close();
            return number;
        }
        string GetWorkerNameFromID(int id)
        {
            String command = "SELECT Name FROM Worker WHERE WorkerID =" + id;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            string name = "";
            SqlDataReader dataReader1 = newCommand.ExecuteReader();
            if (dataReader1.Read())
            {
                name = dataReader1.GetString(0);
            }
            dataReader1.Close();
            return name;
        }
        public override Task<WorkerEventResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "INSERT INTO Worker (WorkerID, Password, Name, TeamName, TeamID, DepartmentID, Level, DepartmentName) VALUES (@Val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            int newWorkerID = GetNewWorkerID();
            newCommand.Parameters.AddWithValue("@val1", newWorkerID);
            newCommand.Parameters.AddWithValue("@val2", request.Password);
            newCommand.Parameters.AddWithValue("@val3", request.Name);
            newCommand.Parameters.AddWithValue("@val4", request.TeamName);
            newCommand.Parameters.AddWithValue("@val5", GetTeamIdFromName(request.TeamName));
            newCommand.Parameters.AddWithValue("@val6", GetDepIdFromName(request.DepName));
            newCommand.Parameters.AddWithValue("@val7", request.Level);
            newCommand.Parameters.AddWithValue("@val8", request.DepName);
            WorkerEventResponse resp = new WorkerEventResponse { State = false, Msg = "Worker cannot be created." };
            try { newCommand.ExecuteNonQuery(); } 
            catch(Exception ex) { Console.WriteLine(ex.Message); }
            resp.State = true;
            resp.Msg = "Worker added with ID: " + newWorkerID;
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(resp);
        }

        public override Task<LogInResponse> LogIn(LoginRequest request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "SELECT Password FROM Worker WHERE WorkerID =" + request.Id;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            bool correct = false;
            if (dataReader.Read())
            {
                if (dataReader.GetString(0) == request.Password)
                {
                    correct = true;
                    dataReader.Close();
                    activeMembers.Add(new ActiveMember(request.Id, GetWorkerNameFromID(request.Id)));
                }
            }
            DBConnection.Close();
            if (correct)
            {
                return System.Threading.Tasks.Task.FromResult(new LogInResponse { State = true, Msg = "Worker logged in succesfuly." });
            }
            else
            {
                return System.Threading.Tasks.Task.FromResult(new LogInResponse { State = false, Msg = "Unable to log in." });
            }
        }

        public override Task<WorkerEventResponse> LogOut(LogOutRequest request, ServerCallContext context)
        {
            bool loggedOut = false;
            for(int i = 0; i < activeMembers.Count; i++)
            {
                if(activeMembers[i].ID == request.Id)
                {
                    loggedOut = true;
                    activeMembers.RemoveAt(i);
                }
            }
            if (loggedOut)
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Worker logged out succesfuly." });
            }
            else
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = false, Msg = "Cannot log out." });
            }
        }
        public override Task<TaskListResponse> GetTaskList(TaskListRequest request, ServerCallContext context)
        {
            TaskListResponse taskList = new TaskListResponse();
            DBConnection.Open();
            String command = "SELECT * FROM ScheduleItem WHERE Team =" + request.TeamName;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                taskList.Tasks.Add(new MessagesPack.Task {Status = dataReader.GetString(3), Team = dataReader.GetString(1), TeamID = dataReader.GetInt32(0), Text = dataReader.GetString(4)});
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(taskList);
        }

        public override Task<DepartmentsListResp> GetDepartments(BlankMsg request, ServerCallContext context)
        {
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
        public override Task<WorkerEventResponse> AddTeam(AddTeamRequest request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "INSERT INTO Team (TeamID, TeamName, DepartmentID, DepartmentName) VALUES (@val1, @val2, @val3, @val4)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewTeamID());
            newCommand.Parameters.AddWithValue("@val2", request.TeamName);
            newCommand.Parameters.AddWithValue("@val3", GetDepIdFromName(request.DepartmentName));
            newCommand.Parameters.AddWithValue("@val4", request.DepartmentName);
            newCommand.ExecuteNonQuery();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "New team added!" });
        }
        public override Task<WorkerEventResponse> RemoveTeam(TeamDescription request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "DELETE FROM Team WHERE TeamName ="+request.Name;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            newCommand.ExecuteNonQuery();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Team deleted sucesfully!" });
        }
        public override Task<WorkerEventResponse> AddDepartment(AddDepRequest request, ServerCallContext context)
        {
            DBConnection.Open();
            String command = "INSERT INTO Department (DepartmentID, DepName) VALUES (@val1, @val2)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewDepID());
            newCommand.Parameters.AddWithValue("@val2", request.DepName);
            newCommand.ExecuteNonQuery();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "New department added!" });
        }
        public override Task<TeamListResp> GetDepartmetTeams(NameRequest request, ServerCallContext context)
        {
            TeamListResp tmp = new TeamListResp();
            DBConnection.Open();
            String command = "SELECT * FROM Team WHERE DepartmentName=" + request.TeamName;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.TeamDesc.Add(new TeamDescription { Index = dataReader.GetInt32(0), Name = dataReader.GetString(1)});
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
        public override Task<IntegerResponse> GetTeamID(NameRequest request, ServerCallContext context)
        {
            IntegerResponse tmp = new IntegerResponse();
            DBConnection.Open();
            tmp.Number = GetTeamIdFromName(request.TeamName);
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
        public override Task<WorkerEventResponse> GetWorkerName(IntegerRequest request, ServerCallContext context)
        {
            WorkerEventResponse tmp = new WorkerEventResponse();
            DBConnection.Open();
            tmp.Msg = GetWorkerNameFromID(request.Number);
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
        public override Task<TaskListResponse> GetTeamSpecificTasks(TeamDescription request, ServerCallContext context)
        {
            TaskListResponse respList = new TaskListResponse();
            int teamID = -1;
            DBConnection.Open();
            String command = "SELECT TeamID FROM Worker WHERE WorkerID =" + request.Index;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            if(dataReader.Read())
            {
                teamID = dataReader.GetInt32(0);            
                command = "SELECT Team, TeamID, Status, Text FROM ScheduleItem WHERE Status=" + request.Name + " AND TeamID =" + teamID;
                newCommand = new SqlCommand(command, DBConnection);
                dataReader = newCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    respList.Tasks.Add(new MessagesPack.Task { Team = dataReader.GetString(0), TeamID = dataReader.GetInt32(1), Status = dataReader.GetString(2), Text = dataReader.GetString(3) });
                }
            }            
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(respList);
        }
        public override Task<TeamDescription> GetDepHead(DepartmentDescription request, ServerCallContext context)
        {
            TeamDescription headDesc = new TeamDescription();
            DBConnection.Open();
            String command = "SELECT WorkerID, Name FROM Worker WHERE DepartmentID =" + request.Index;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            if (dataReader.Read())
            {
                headDesc.Index = dataReader.GetInt32(0);
                headDesc.Name = dataReader.GetString(1);
            }
            else
            {
                headDesc.Index = -1;
                headDesc.Name = "None"; 
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(headDesc);
        }
        public override Task<TaskListResponse> GetAllDepTasks(NameRequest request, ServerCallContext context) //TODO this throws error when invoked
        {
            //get all department teams
            TeamListResp teams = new TeamListResp();
            DBConnection.Open();
            String command = "SELECT TeamID, TeamName FROM Team WHERE DepartmentName =" + request.TeamName;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                teams.TeamDesc.Add(new TeamDescription { Index = dataReader.GetInt32(0), Name = dataReader.GetString(1) });
            }
            //get all tasks for these teams
            TaskListResponse taskList = new TaskListResponse();
            foreach (var team in teams.TeamDesc)
            {                
                command = "SELECT * FROM ScheduleItem WHERE Team =" + team.Name;
                newCommand = new SqlCommand(command, DBConnection);
                dataReader = newCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    taskList.Tasks.Add(new MessagesPack.Task { Status = dataReader.GetString(3), Team = dataReader.GetString(1), TeamID = dataReader.GetInt32(0), Text = dataReader.GetString(4) });
                }
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(taskList);
        }
        public override Task<WorkerEventResponse> GetDepFromUser(IntegerRequest request, ServerCallContext context)
        { 
            var resp = new WorkerEventResponse();
            DBConnection.Open();
            String command = "SELECT DepartmentName FROM Worker WHERE DepartmentID =" + request.Number;
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
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
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(resp);
        }
        public override Task<WorkerEventResponse> AddTask(MessagesPack.Task request, ServerCallContext context)
        {
            var resp = new WorkerEventResponse();
            DBConnection.Open();
            String command = "INSERT INTO ScheduleItem (Id, Team, TeamID, Status, Text) VALUES (@val1, @val2, @val3,@val4, @val5)";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            newCommand.Parameters.AddWithValue("@val1", GetNewTaskID());
            newCommand.Parameters.AddWithValue("@val2", request.Team);
            newCommand.Parameters.AddWithValue("@val3", request.TeamID);
            newCommand.Parameters.AddWithValue("@val4", request.Status);
            newCommand.Parameters.AddWithValue("@val5", request.Text);
            try
            {
                newCommand.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                DBConnection.Close();
            }
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse {Msg = "Task added sucessfully.", State = true});
        }
        public override Task<TeamListResp> GetAllWorkers(BlankMsg request, ServerCallContext context)
        {
            TeamListResp tmp = new TeamListResp();
            DBConnection.Open();
            String command = "SELECT WorkerID, Name FROM Worker";
            SqlCommand newCommand = new SqlCommand(command, DBConnection);
            SqlDataReader dataReader = newCommand.ExecuteReader();
            while (dataReader.Read())
            {
                tmp.TeamDesc.Add(new TeamDescription { Index = dataReader.GetInt32(0), Name = dataReader.GetString(1) });
            }
            dataReader.Close();
            DBConnection.Close();
            return System.Threading.Tasks.Task.FromResult(tmp);
        }
        public override Task<WorkerEventResponse> PullCoffeeBrake(CoffeBreakRequest request, ServerCallContext context)
        {
            if (coffieInviter.Inviting && coffieInviter.InviterName != request.Name)
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Read status of coffee.", State = true });
            }
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Read status of coffee", State = false });
        }
        public override Task<WorkerEventResponse> PullEmergency(IntIntRequest request, ServerCallContext context)
        {
            if(emergencyCaller.TeamId != -1 && emergencyCaller.TeamId == request.TeamID)
            {
                return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Read status of emergency.", State = true });
            }
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Read status of emergency", State = false });
        }
        public override Task<WorkerEventResponse> CallEmergency(IntIntRequest request, ServerCallContext context)
        {
            //emergencyCaller.InviterId = request.WorkerID;
            //emergencyCaller.TeamId = request.TeamID;
            //System.Threading.Tasks.Task.Delay(50000).ContinueWith(_ =>
            //{
            //    emergencyCaller.InviterId = -1;
            //    emergencyCaller.TeamId = -1;
            //});
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { Msg = "Emergency called!", State = true });
        }
        public override Task<WorkerEventResponse> CallCoffeeBreak(CoffeBreakRequest request, ServerCallContext context)
        { 
            //coffieInviter.InviterName = request.Name;
            //coffieInviter.Inviting = true;
            //System.Threading.Tasks.Task.Delay(50000).ContinueWith(_ =>
            //{
            //    coffieInviter.InviterName = "";
            //    coffieInviter.Inviting = false;
            //}
            //);
            return System.Threading.Tasks.Task.FromResult(new WorkerEventResponse { State = true, Msg = "Smacznego!" });
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
