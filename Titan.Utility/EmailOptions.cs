﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Titan.Utility
{
    public class EmailOptions
    {
        public string Host { get; set; }
        public string From { get; set; }
        public string Alias { get; set; }
        public int Port { get; set; }

        public string Pvalue { get; set; }
    }
}
