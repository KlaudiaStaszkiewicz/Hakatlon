using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpoLife
{
    class Team
    {
        int teamID, leaderID;
        string teamName, leaderName;
        List<Worker> Workers = new List<Worker>();
        public Team()
        {

        }
    }
}
