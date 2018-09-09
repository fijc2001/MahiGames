using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlotMachinesSim2013;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace SlotMachineSim2013Test
{
    [TestClass]
    public class UnitTestSlotMachine
    {


        class SlotEngineStub : ISlotEngine
        {
            ReportDTO dto;
            public SlotEngineStub(ReportDTO dto)
            {
                this.dto = dto;
            }

            ReportDTO ISlotEngine.Calculate(List<List<SlotSymbolOffset>> reelsOuput)
            {
                if(reelsOuput==null || reelsOuput.Count==0)
                {
                    throw new Exception("Reel collection not passed to SlotEngine");
                }
                return dto;
            }
        }
        
        
        
        [TestMethod]
        public void SpinIsOk()
        {
            bool test = false;
            var slotMachine = new SlotMachine(new List<IReel> { new Reel(3, new List<ESlotSymbol> { ESlotSymbol.Ace, ESlotSymbol.Jack, ESlotSymbol.King }) },
                                                new SlotEngineStub(new ReportDTO{
                                                                                HitTotal = 1,
                                                                                PayoffAmount = 300,
                                                                                PaylineHitTotal =3,
                                                                                SpinTotal=1
                                                                            }));

            var dto = slotMachine.Spin();
            if(dto.HitTotal==1 &&  dto.PayoffAmount==300 && 
               dto.PaylineHitTotal==3 && dto.SpinTotal==1)
            { test = true; }


            Assert.IsTrue(test);
        }
    }
}
