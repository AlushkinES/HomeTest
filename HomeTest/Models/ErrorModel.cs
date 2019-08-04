using System.Collections.Generic;
using Newtonsoft.Json;

namespace HomeTest.DTO
{
    public class ErrorModel
    {
        public string name { get; set; }
        public string message { get; set; }
        public int code { get; set; }
        public string className { get; set; }
        
        public dynamic errors { get; set; }
    }
}