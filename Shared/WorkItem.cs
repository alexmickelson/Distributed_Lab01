using System;

namespace Shared
{
    public class WorkItem
    {
        public WorkItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }
        public string Value { get; }
        public string Result { get; set; }
    }
}
