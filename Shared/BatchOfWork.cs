using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class BatchOfWork
    {
        public BatchOfWork(int id)
        {
            BatchId = id;
            WorkItems = new List<WorkItem>();
        }

        public BatchOfWork(int id, List<WorkItem> workItems)
        {
            BatchId = id;
            WorkItems = workItems;
        }

        public int BatchId { get; set; }
        public List<WorkItem> WorkItems { get; set; }
    }
}
