using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ListOfWork : IListOfWork
    {
        private static List<MessageDto> workToDo { get; set; }
        private static int batchNums { get; set; } = 0;

        public ListOfWork()
        {
            workToDo = new List<MessageDto>();
            workToDo.Add(new MessageDto("Row 1", "First Value"));
            workToDo.Add(new MessageDto("Row 2", "First Value"));
            workToDo.Add(new MessageDto("Row 3", "First Value"));
            workToDo.Add(new MessageDto("Row 4", "First Value"));
            workToDo.Add(new MessageDto("Row 5", "First Value"));
            workToDo.Add(new MessageDto("Row 6", "First Value"));
            workToDo.Add(new MessageDto("Row 6", "Second Value"));
            workToDo.Add(new MessageDto("Row 5", "Second Value"));
            workToDo.Add(new MessageDto("Row 4", "Second Value"));
            workToDo.Add(new MessageDto("Row 3", "Second Value"));
            workToDo.Add(new MessageDto("Row 2", "Second Value"));
            workToDo.Add(new MessageDto("Row 1", "Second Value"));
        }

        public List<MessageDto> getWork()
        {
            var b = batchNums++;
            var res = new List<MessageDto>();

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
