using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachinesSim2013
{
    public class SlotMachine 
    {
        private List<IReel> reels;
        private ISlotEngine engine;
        float amount;
        int numberHits;
        int numberofMatches;

        public  SlotMachine(List<IReel> reels, ISlotEngine engine)
         {
             this.reels = reels;
             this.engine = engine;
             numberHits=0;
             numberofMatches=0;
         }

        public  ReportDTO Spin()
         {
             ReportDTO dto;
            List<List<SlotSymbolOffset>> outReels = new List<List<SlotSymbolOffset>>();
             foreach (IReel r in reels){

                 outReels.Add(r.Spin());
             }

             dto=engine.Calculate(outReels);
             return dto;
         }


    }
}
