﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.TestReportDomain.UnitTest
{
    public class TestClassSampleCompose
    {
        public string TestHeader { get; set; } = "Hello Compose";
        public TestClassSample TestClassSample { get; set; } = new();
        public TestClassSample ABC {get;set;} = new();
    }
}
