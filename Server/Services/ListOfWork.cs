using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ListOfWork : IListOfWork
    {
        private static List<BatchOfWork> Batches { get; set; }
        public int id { get; set; } = 0;

        public ListOfWork()
        {
            Batches = new List<BatchOfWork>();

            Batches.Add(GenerateWork(id++))

            workToDo.Add(new WorkItem("Row 1", "First Value"));
            workToDo.Add(new WorkItem("Row 2", "First Value"));
            workToDo.Add(new WorkItem("Row 3", "First Value"));
            workToDo.Add(new WorkItem("Row 4", "First Value"));
            workToDo.Add(new WorkItem("Row 5", "First Value"));
            workToDo.Add(new WorkItem("Row 6", "First Value"));
            workToDo.Add(new WorkItem("Row 6", "Second Value"));
            workToDo.Add(new WorkItem("Row 5", "Second Value"));
            workToDo.Add(new WorkItem("Row 4", "Second Value"));
            workToDo.Add(new WorkItem("Row 3", "Second Value"));
            workToDo.Add(new WorkItem("Row 2", "Second Value"));
            workToDo.Add(new WorkItem("Row 1", "Second Value"));

        }

        public BatchOfWork GenerateWork(int myId, int numRows)
        {

            var batch = new BatchOfWork(id++);
        }

        public List<WorkItem> getWork()
        {
            lock (workToDo)
            {
                var res = new List<WorkItem>();

                var work = workToDo.First();
                workToDo.Remove(work);
            
                res.Add(work);

                work = workToDo.First();
                workToDo.Remove(work);
                res.Add(work);

                work = workToDo.First();
                workToDo.Remove(work);
                res.Add(work);

                work = workToDo.First();
                workToDo.Remove(work);
                res.Add(work);

                work = workToDo.First();
                workToDo.Remove(work);
                res.Add(work);

                work = workToDo.First();
                workToDo.Remove(work);
                res.Add(work);


                return res;

            }
        }
    }
}
