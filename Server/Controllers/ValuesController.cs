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
using System.Text;

namespace Server.Controllers
{
    //docker-compose down; docker rm -f $(docker ps --all -q); docker-compose up --build --scale worker=2 --scale server=1
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
        public IEnumerable<string[]> Get()
        {
            Console.WriteLine("Getting current key values");
            var res = new List<string[]>();
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (var file in files)
            {
                if (file.Contains("Row"))
                {
                    string contents = "";
                    try
                    {
                        contents = System.IO.File.ReadAllText(file);

                    } catch (IOException e)
                    {
                        Console.WriteLine("Cannot open file" + file);
                    }

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
            Console.WriteLine("Getting current value of key");
            if (System.IO.File.Exists(key))
                return System.IO.File.ReadAllText(key);
            return null;
        }

        // POST api/values
        [HttpPost]
        public List<MessageDto> Post([FromBody]List<MessageDto> messages)
        {
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            var rand = new Random();
            var timeOut = rand.Next(5, 15);

            Console.WriteLine("attempting to do a batch of work");
            var openFiles = new List<FileStream>();
            foreach (var msg in messages)
            {
                FileStream stream = null;
                while(stream == null)
                {
                    stream = IsFileLocked(msg.Key);
                    if ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds - epoch > timeOut)
                    {
                        Console.WriteLine($"Batched Timed Out After {timeOut} Seconds, returning null");

                        foreach(var f in openFiles)
                        {
                            Console.WriteLine("     Closing File: " + f.Name);
                            f.Close();
                        }

                        return null;
                    }
                    Thread.Sleep(100);
                }
                openFiles.Add(stream);
            }
            var res = new List<string[]>();
            for (int i = 0; i < openFiles.Count; i++)
            {
                openFiles[i].Write(Encoding.ASCII.GetBytes(messages[i].Value));
                var str = new string[] { openFiles[i].Name, messages[i].Value };
                res.Add(str);
                messages[i].Result = $"Saved on server at {DateTime.Now}, Key: {messages[i].Key}, Value: {messages[i].Value}";
            }
            for (int i = 0; i < openFiles.Count; i++)
            {
                openFiles[i].Close();
                Console.WriteLine("Closed file " + openFiles[i].Name);
            }
            Console.WriteLine("Write Complete, printing current state");
            foreach (var s in res)
            {
                foreach (var s2 in s)
                {
                    Console.Write(s2 + ", ");
                }
            }
            Console.WriteLine("");

            return messages;
        }
        
        private FileStream IsFileLocked(string path)
        {
            Thread.Sleep(300); // to allow for more deadlocks
            Console.WriteLine("checking if " + path + " is locked");
            FileStream stream = null;
            try
            {
                stream = System.IO.File.OpenWrite(path);
            }
            catch (IOException)
            {
                //the file is unavailable 
                Console.WriteLine(path + " is locked");
                return null;
            }
            Console.WriteLine(path + " is not locked");
            //file is not locked
            return stream;

        }

        // GET api/values/getwork
        [HttpGet]
        public IEnumerable<MessageDto> GetWork()
        {
            Console.WriteLine("sending work to worker");
            return _listOfWork.getWork();
        }


    }
}
