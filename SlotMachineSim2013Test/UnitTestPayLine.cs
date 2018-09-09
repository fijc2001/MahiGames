using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotMachinesSim2013;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;


namespace SlotMachineSim2013Test
{
    [TestClass]
    public class UnitTestPayLine
    {
        static List<ESlotSymbol> strip;
        static Reel reelObj;
        static int reelWindow;
        static List<List<SlotSymbolOffset>> outputReel;
        [ClassInitialize]
        public static void Initialize(TestContext ct)
        {
            ct.WriteLine("Class Initialize");
            strip = new List<ESlotSymbol>{
                                        ESlotSymbol.Ace , ESlotSymbol.King, ESlotSymbol.Queen, ESlotSymbol.Jack, ESlotSymbol.Ten, ESlotSymbol.Nine
                                       };
            reelWindow = 3;
            List<ESlotOffset> VerticalOffsets = new List<ESlotOffset>
                                                        {
                                                            ESlotOffset.Fisrt,ESlotOffset.Second,ESlotOffset.Third
                                                        };

            

            outputReel = new List<List<SlotSymbolOffset>>{
                                                        new List<SlotSymbolOffset>{
                                                                                    new SlotSymbolOffset(ESlotOffset.Fisrt,ESlotSymbol.Ace),
                                                                                    new SlotSymbolOffset(ESlotOffset.Second,ESlotSymbol.Queen),
                                                                                    new SlotSymbolOffset(ESlotOffset.Third,ESlotSymbol.Jack)
                                                                                  },
                                                        new List<SlotSymbolOffset>{
                                                                                    new SlotSymbolOffset(ESlotOffset.Fisrt,ESlotSymbol.Jack),
                                                                                    new SlotSymbolOffset(ESlotOffset.Second,ESlotSymbol.Queen),
                                                                                    new SlotSymbolOffset(ESlotOffset.Third,ESlotSymbol.Ace)
                                                                                  },
                                                         new List<SlotSymbolOffset>{
                                                                                    new SlotSymbolOffset(ESlotOffset.Fisrt,ESlotSymbol.King),
                                                                                    new SlotSymbolOffset(ESlotOffset.Second,ESlotSymbol.Ten),
                                                                                    new SlotSymbolOffset(ESlotOffset.Third,ESlotSymbol.Nine)
                                                                                  },

                                                       };
        }                                                          

        [ClassCleanup]
        public static void CleanUp()
        {

        }
        
        
        [TestMethod]
        public void CreationIsOk()
        {
            bool test=false;
            var xdoc = XDocument.Parse(@"<Root>
                                        <Paylines name='BasePayLines'>
                                        <Payline name='line1' VerticalOffsets='1,1,1'/>
                                        </Paylines>
                                        </Root>");
           
            var payLineXML = xdoc.Descendants("Payline").ToList<XElement>()
                                                            .Select(xe => new Payline(xe))
                                                             .ToList<Payline>()[0];

            var payLine = new Payline("line2", new List<ESlotOffset> { ESlotOffset.Fisrt, ESlotOffset.Third, ESlotOffset.Second });

            if(payLineXML!=null && payLine!=null && 
               payLineXML.VerticalOffsets!=null && 
               payLine.VerticalOffsets!=null&&
               payLineXML.VerticalOffsets.Count==3 &&
               payLine.VerticalOffsets.Count == 3)
            {
                test = true;
            }
            Assert.IsTrue(test);
        }

        [TestMethod]
        public void CalculateDoesMatch()
        {

            bool test = false;
        
            var xdoc = XDocument.Parse(@"<Root>
                                        <Paylines name='BasePayLines'>
                                        <Payline name='line1' VerticalOffsets='1,1,1'/>
                                        </Paylines>
                                        <PayTable name='BasePayTable'>
                                        <Pay Amount='2500' ExactMatch='Queen,Queen,Ten'/>         
                                        </PayTable>
                                        </Root>");
            var payMatchAmount= xdoc.Descendants("Pay").ToList<XElement>()
                                                              .Select(xe => new PayMatchAmount(xe))
                                                              .ToList<PayMatchAmount>()[0];
            var payLineXML = xdoc.Descendants("Payline").ToList<XElement>()
                                                            .Select(xe => new Payline(xe))
                                                             .ToList<Payline>()[0];
            var dto = payLineXML.Calculate(payMatchAmount, outputReel);
            if (dto.PaylineHitTotal == 1 && dto.PayoffAmount == 2500)
            {
                test = true;
            }
            Assert.IsTrue(test);
        }

         [TestMethod]
        public void CalculateDoesNotMatch()
        {

            bool test = false;

            var xdoc = XDocument.Parse(@"<Root>
                                        <Paylines name='BasePayLines'>
                                        <Payline name='line1' VerticalOffsets='1,1,1'/>
                                        </Paylines>
                                        <PayTable name='BasePayTable'>
                                        <Pay Amount='2500' ExactMatch='Ace,Queen,Ten'/>         
                                        </PayTable>
                                        </Root>");
            var payMatchAmount = xdoc.Descendants("Pay").ToList<XElement>()
                                                              .Select(xe => new PayMatchAmount(xe))
                                                              .ToList<PayMatchAmount>()[0];
            var payLineXML = xdoc.Descendants("Payline").ToList<XElement>()
                                                            .Select(xe => new Payline(xe))
                                                             .ToList<Payline>()[0];
             var dto=payLineXML.Calculate(payMatchAmount, outputReel);
            if (dto.PaylineHitTotal != 1 && dto.PayoffAmount==-2500)
            {
                test = true;
            }
            Assert.IsTrue(test);
        }
    
    
    }
}
