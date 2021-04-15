using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Logger.Service.Data
{
    public class LogPoco
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public string ChainId { get; set; }

        public int Severity { get; set; }

        public string Host { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
