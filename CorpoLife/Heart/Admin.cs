﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heart
{
    class Admin : Head
    {
        public static void RegisterWorker()
        {
            var client = GlobalUsage.Client();
            var depList = client.GetDepartments(new MessagesPack.BlankMsg());
            foreach(var dep in depList.DepsDesc)
            {
                
            }
        }
        public static void RemoveWorker()
        {

        }
    }
}
