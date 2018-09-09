using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotMachinesSim2013;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace SlotMachineSim2013Test
{
    [TestClass]
    public class UnitTestReel
    {
        static List<ESlotSymbol> strip;
        static Reel reelObj;
        static int reelWindow;
        [ClassInitialize]
        public static void Initialize(TestContext ct)
        {
            ct.WriteLine("Class Initialize");
            strip = new List<ESlotSymbol>{
                                        ESlotSymbol.Ace , ESlotSymbol.King, ESlotSymbol.Queen, ESlotSymbol.Jack, ESlotSymbol.Ten, ESlotSymbol.Nine
                                       };
            reelWindow=3;
            reelObj = new Reel(reelWindow, strip);

        }
        [ClassCleanup]
        public static void CleanUp()
        {
          
        }
        
        
        [TestMethod]
        public void SpinReturnNotNull ()
        {
            Assert.IsNotNull(reelObj.Spin());
            
        }
        [TestMethod]
        public void SpinReturnChangingValues()
        {
            var listReelOutput =new List<List<SlotSymbolOffset>>();
            var TotalI = 50;
            List<SlotSymbolOffset> nextL;
            for(int i=0 ; i<TotalI; i++) {

                listReelOutput.Add(reelObj.Spin());
            }
            for(int i=0 ; i<(TotalI-1) ; i++)
            {
                nextL = listReelOutput[i + 1];
                if(!Enumerable.SequenceEqual(listReelOutput[i], nextL))
                {
                    Assert.IsTrue(true);
                    return;
                }
           
            }

            Assert.IsTrue(false);
        }

        [TestMethod]
        public void CreationIsOK()
        {
            bool test1=false;
            bool test2 = false;
            XDocument xdoc = XDocument.Parse("<Root><Reel name='reel1' Symbols='Nine,King,Ten,Queen,Jack,Ace'/> <Reel name='reel2' Symbols='Nine,King,Ten,Queen,Jack,Ace'/> </Root>");
            var reelL =xdoc.Descendants("Reel").ToList<XElement>()
                                                        .Select(xe => new Reel(reelWindow, xe))
                                                        .ToList<IReel>();
            if (reelObj != null && reelObj.Strip != null && reelObj.Strip.Count == strip.Count)
                test1 = true;

            if (reelL != null && reelL.Count == 2 &&
                reelL[0].Strip.Count == 8 &&
                reelL[1].Strip.Count == 8 &&
                reelL[0].Spin().Count == reelWindow
                )
            {
                test2 = true;
            }
            Assert.IsTrue(test1 && test2);

        }
    
    }
}
