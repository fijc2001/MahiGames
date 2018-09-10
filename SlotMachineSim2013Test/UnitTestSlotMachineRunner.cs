using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotMachinesSim2013;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Configuration;

namespace SlotMachineSim2013Test
{
    [TestClass]
    public class UnitTestSlotMachineRunner
    {
        static string filePath;
        static SlotMachineRunner slotMachineRunner;
        
        [ClassInitialize]
        public static void Initialize(TestContext ct)
        {
            ct.WriteLine("Class Initialize");
            filePath = ConfigurationManager.AppSettings["ConfigSlotMachine"];
            slotMachineRunner = new SlotMachineRunner(filePath, 100000, 3);
        }
        
        [TestMethod]
        public void CreationIsOK()
        {
            var test = false;
           
           if (slotMachineRunner != null)
           {
               test =true;
           }
           Assert.IsTrue(test);
        }

        [TestMethod]
        public void SpinAllCombinationsIsOk()
        {
            bool test = false;
            
            var dto=slotMachineRunner.SpinAllCombinations();
            
            if (dto.HitTotal == 30 && dto.PayoffAmount == -6086300 &&
               dto.PaylineHitTotal == 35 && dto.SpinTotal == 252)
            { test = true; }

            Assert.IsTrue(test);
        }
        [TestMethod]
        public void SpinManytimesIsOk()
        {
            /*Note : This test should pass if the ramdom generation is consistent and the number of iterations is big enough*/
            bool test = false;

            var dto = slotMachineRunner.SpinManyTimes();
            var hitRate=(dto.HitTotal*100.0)/dto.SpinTotal;
            if ( hitRate<14  && hitRate>10 && dto.SpinTotal==300000)
            { test = true; }

            Assert.IsTrue(test);
        }
    
    }
}
