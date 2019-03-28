using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Server.Services;
using System.Threading;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IListOfWork _listOfWork;

        public ValuesController(IListOfWork listOfWork)
        {
            _listOfWork = listOfWork;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string[]>> Get()
        {
            var res = new List<string[]>();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var file in files)
            {
                if (file.Contains("Row"))
                {
                    string contents;
                    contents = System.IO.File.ReadAllText(file);

                    var str = new string[] { file, contents };
                    res.Add(str);
                }
            }
            return res;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string key)
        {
            if(System.IO.File.Exists(key))
                return System.IO.File.ReadAllText(key);
            return null;
        }

        // POST api/values
        [HttpPost]
        public ActionResult<List<MessageDto>> Post([FromBody]List<MessageDto> messages)
        {
            foreach(var msg in messages)
            {
                System.IO.File.WriteAllText(msg.Key, msg.Value);
                msg.Result = $"Saved on server at {DateTime.Now}, Key: {msg.Key}, Value: {msg.Value}";
                Thread.Sleep(500);
            }
            return messages;
        }
        // GET api/values/getwork
        [HttpGet]
        public IEnumerable<MessageDto> GetWork()
        {
            return _listOfWork.getWork();
        }

    }
}
