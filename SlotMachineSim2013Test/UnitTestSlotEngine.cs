using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotMachinesSim2013;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace SlotMachineSim2013Test
{
    [TestClass]
    public class UnitTestSlotEngine
    {

        class PayLineStub : IPayline
        {
            ReportDTO dto;
           public  PayLineStub(ReportDTO dto)
            {
                this.dto = dto;
            }
            public ReportDTO Calculate(PayMatchAmount match, List<List<SlotSymbolOffset>> reelOutputs)
            {
                return dto;
            }

            public List<ESlotOffset> VerticalOffsets
            {
                get { throw new NotImplementedException(); }
            }
        }


        
        
        
        
        [TestMethod]
        public void CreationIsOK()
        {
            bool test = false;

            var xdoc = XDocument.Parse(@"<Root>
                                        <Paylines name='BasePayLines'>
                                        <Payline name='line1' VerticalOffsets='1,1,1'/>
                                        <Payline name='line2' VerticalOffsets='1,1,1'/>
                                        </Paylines>
                                        <PayTable name='BasePayTable'>
                                        <Pay Amount='2500' ExactMatch='Queen,Queen,Ten'/>    
                                        <Pay Amount='200' ExactMatch='Queen,Queen,King'/>         
                                        </PayTable>
                                        </Root>");
            var payMatchAmount = xdoc.Descendants("Pay").ToList<XElement>()
                                                              .Select(xe => new PayMatchAmount(xe))
                                                              .ToList<PayMatchAmount>();
            var payLineXML = xdoc.Descendants("Payline").ToList<XElement>()
                                                            .Select(xe => new Payline(xe))
                                                             .ToList<IPayline>();
            var slotEngine = new SlotEngine(payLineXML, payMatchAmount);
            
            if(slotEngine!=null && 
               slotEngine.PaylineList!=null && 
               slotEngine.PayMatchAmountList!=null&&
               slotEngine.PaylineList.Count==2&&
               slotEngine.PayMatchAmountList.Count==2)
            {
                test = true;
            }

            Assert.IsTrue(test);
        }
   
        [TestMethod]
        public void CalculateIsOk ()
        {
            bool test = false;
            var payMatchA = new List<PayMatchAmount> { new PayMatchAmount(null, 0), new PayMatchAmount(null, 0) };
            var payL = new List<IPayline>{
                                         new PayLineStub(new ReportDTO
                                                             {
                                                                 HitTotal = 10,
                                                                 PayoffAmount = 300,
                                                                 PaylineHitTotal =1,
                                                                 SpinTotal=10
                                                             }),
                                         new PayLineStub(new ReportDTO
                                                             {
                                                                 HitTotal = 10,
                                                                 PayoffAmount = 0,
                                                                 PaylineHitTotal =0,
                                                                 SpinTotal=10
                                                             }),
                                            new PayLineStub(new ReportDTO
                                                             {
                                                                 HitTotal = 10,
                                                                 PayoffAmount = 100,
                                                                 PaylineHitTotal =1,
                                                                 SpinTotal=10
                                                             })
                                        
                                       };
            var slotEngine = new SlotEngine(payL, payMatchA);
            var dto = slotEngine.Calculate(null);
            if(dto.PayoffAmount==800 && dto.SpinTotal==1 && dto.PaylineHitTotal==4&& dto.HitTotal==1)
            {
                test = true;
            }
            Assert.IsTrue(test);
        }
    }
}
