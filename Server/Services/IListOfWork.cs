using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IListOfWork
    {
        List<WorkItem> getWork();
    }
}
