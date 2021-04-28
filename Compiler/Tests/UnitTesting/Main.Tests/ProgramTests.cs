using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Main.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void Run_SmokeTest_DoNotCrash()
        {
            string[] args = new string[] { "project" };

            try
            {
                new Program(args).Run();
            }
            catch (NotImplementedException)
            {
            }
        }
    }
}
