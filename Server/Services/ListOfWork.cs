using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ListOfWork : IListOfWork
    {
        private List<MessageDto> workToDo { get; set; }

        public ListOfWork()
        {
            workToDo = new List<MessageDto>();
            workToDo.Add(new MessageDto("Row 1", "First Value"));
            workToDo.Add(new MessageDto("Row 2", "First Value"));
            workToDo.Add(new MessageDto("Row 2", "Second Value"));
            workToDo.Add(new MessageDto("Row 1", "Second Value"));
        }

        public List<MessageDto> getWork()
        {
            var res = new List<MessageDto>();

            var work = workToDo.First();
            workToDo.Remove(work);
            res.Add(work);

            work = workToDo.First();
            workToDo.Remove(work);
            res.Add(work);

            return res;
        }
    }
}
