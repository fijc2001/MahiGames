using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlotMachinesSim2013
{
    public class SlotMachineRunner
    {
        List<SlotMachine> slotMachineList;
        SlotEngine slotMachineEngine;
        List<IReel>  reelStripList;
        int visibleAreaHeight;
        int visibleAreaWidth;
        int iterationsTotal;
        int slotMachineTotal;
     
       public SlotMachineRunner(string slotMachineConfigFile, int iterationsTotal , int slotMachineTotal)
        {
            this.iterationsTotal=iterationsTotal;
            this.slotMachineTotal=slotMachineTotal;
            init(slotMachineConfigFile);
        }

       protected  virtual void init(string slotMachineConfigFile)
        {
            XDocument xdoc = XDocument.Load("assessment_game.xml");
            var betInfo = int.Parse(xdoc.Descendants("BetInfo").ToArray<XElement>()[0].Attribute("Amount").Value);
            visibleAreaHeight = int.Parse(xdoc.Descendants("VisibleArea").ToArray<XElement>()[0].Attribute("Rows").Value);
            visibleAreaWidth = int.Parse(xdoc.Descendants("VisibleArea").ToArray<XElement>()[0].Attribute("Columns").Value);
            reelStripList = xdoc.Descendants("Reel").ToList<XElement>()
                                                        .Select(xe => new Reel(visibleAreaHeight, xe))
                                                        .ToList<IReel>();
            var payMatchAmountList= xdoc.Descendants("Pay").ToList<XElement>()
                                                               .Select(xe => new PayMatchAmount(xe))
                                                               .ToList<PayMatchAmount>();
            var payLineList= xdoc.Descendants("Payline").ToList<XElement>()
                                                            .Select(xe => new Payline(xe))
                                                             .ToList<IPayline>();

            slotMachineEngine = new SlotEngine(payLineList, payMatchAmountList);

            slotMachineList = new List<SlotMachine>();
            for (int i = 0; i < slotMachineTotal; i++)
            {
                slotMachineList.Add(new SlotMachine(reelStripList, slotMachineEngine));
            }
           
           

        }

       
       ReportDTO SpinInstanceManyTimes(SlotMachine slotMachineInstance)
       {

           ReportDTO dto = new ReportDTO
            {
                HitTotal = 0,
                PayoffAmount = 0,
                PaylineHitTotal = 0
            };
             for (int i = 0; i < iterationsTotal; i++)
               {

                   ReportDTO dtoM = slotMachineInstance.Spin();
                   dto.PayoffAmount += dtoM.PayoffAmount;
                   dto.HitTotal += dtoM.HitTotal;
                   dto.PaylineHitTotal += dtoM.PaylineHitTotal;
                   dto.SpinTotal += dtoM.SpinTotal;
               }
           
           return dto;

       }

       async Task<ReportDTO> SpinInstanceManyTimesAsync(SlotMachine slotMachineInstance)
       {
          
           var result = await Task.Run(() => SpinInstanceManyTimes(slotMachineInstance));
           return result;
       }
       public ReportDTO SpinManyTimes()
         {
            ReportDTO dto = new ReportDTO
             {
                 HitTotal = 0,
                 PayoffAmount = 0,
                 PaylineHitTotal = 0,
                 SpinTotal=0
             };

            List <Task<ReportDTO>> dtoList= new List<Task<ReportDTO>>();
            slotMachineList.ToArray();
            foreach (var  m in slotMachineList){
                dtoList.Add(SpinInstanceManyTimesAsync(m));
            }

            Task.WaitAll(dtoList.ToArray());
            foreach (var dtoI in dtoList)
            {
                dto.PayoffAmount += dtoI.Result.PayoffAmount;
                dto.HitTotal += dtoI.Result.HitTotal;
                dto.PaylineHitTotal += dtoI.Result.PaylineHitTotal;
                dto.SpinTotal += dtoI.Result.SpinTotal;

            }
            return dto;
         }
    
       public ReportDTO SpinAllCombinations()
       {
           ReportDTO dto = new ReportDTO
           {
               HitTotal = 0,
               PayoffAmount = 0,
               PaylineHitTotal = 0,
               SpinTotal = 0
           };
           ReportDTO dtoI;

           List<List<ESlotSymbol>> stripList = reelStripList
                                            .Select(r => r.Strip)
                                            .ToList<List<ESlotSymbol>>();
          

           var offsets = Enum.GetValues(typeof(ESlotOffset)).Cast<ESlotOffset>();
           var stripList0=stripList[0].ToArray<ESlotSymbol>();
           var stripList1=stripList[1].ToArray<ESlotSymbol>();
           var stripList2=stripList[2].ToArray<ESlotSymbol>();
           var stripList0Wind = new List<SlotSymbolOffset>();
           var stripList1Wind = new List<SlotSymbolOffset>();
           var stripList2Wind = new List<SlotSymbolOffset>();
           var Count0 = stripList0.Count<ESlotSymbol>() - (visibleAreaHeight - 1);
           var Count1 = stripList1.Count<ESlotSymbol>() - (visibleAreaHeight - 1);
           var Count2 = stripList2.Count<ESlotSymbol>() - (visibleAreaHeight - 1);

           var reelsOutput=new List<List<SlotSymbolOffset>>();
           var slice=new List<ESlotSymbol>();
           for(int i0 =0 ;i0<Count0;i0++)
           {
               slice = stripList0.Skip(i0).Take(visibleAreaHeight).ToList<ESlotSymbol>();
               stripList0Wind=slice.Zip(offsets, (s, o) => new SlotSymbolOffset(o, s)).ToList<SlotSymbolOffset>();
               for(int i1 =0 ;i1<Count1;i1++)
                {
                    slice = stripList1.Skip(i1).Take(visibleAreaHeight).ToList<ESlotSymbol>();
                    stripList1Wind = slice.Zip(offsets, (s, o) => new SlotSymbolOffset(o, s)).ToList<SlotSymbolOffset>();
                   for(int i2 =0 ;i2<Count2;i2++)
                    {
                       slice = stripList2.Skip(i2).Take(visibleAreaHeight).ToList<ESlotSymbol>();
                       stripList2Wind = slice.Zip(offsets, (s, o) => new SlotSymbolOffset(o, s)).ToList<SlotSymbolOffset>();
                       reelsOutput.Add(stripList0Wind);
                       reelsOutput.Add(stripList1Wind);
                       reelsOutput.Add(stripList2Wind);
                       dtoI=slotMachineEngine.Calculate(reelsOutput);
                       dto.PayoffAmount += dtoI.PayoffAmount;
                       dto.HitTotal += dtoI.HitTotal;
                       dto.PaylineHitTotal += dtoI.PaylineHitTotal;
                       dto.SpinTotal += dtoI.SpinTotal;
                       reelsOutput.RemoveAll(e => true);
                    }

                }
              
           }
          
           return dto;
       }
    }
}
