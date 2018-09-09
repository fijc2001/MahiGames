using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachinesSim2013
{
   public  class SlotEngine :ISlotEngine 
    {
        private List<IPayline> paylineList;

   
        private List<PayMatchAmount> payMatchAmountList;

     
        
        public SlotEngine(List<IPayline> paylineList, List<PayMatchAmount> payMatchAmountList)
        {
            this.paylineList = paylineList;
            this.payMatchAmountList = payMatchAmountList;
        }

        public List<PayMatchAmount> PayMatchAmountList
        {
            get { return payMatchAmountList; }

        }
        public List<IPayline> PaylineList
        {
            get { return paylineList; }

        }
        public ReportDTO Calculate(List<List<SlotSymbolOffset>>reelsOutput)
        {
            ReportDTO dto = new ReportDTO {
                                            HitTotal = 0,
                                            PayoffAmount = 0,
                                            PaylineHitTotal = 0,
                                            SpinTotal = 0
                                          };
            foreach (IPayline pL in paylineList)
            {
                foreach (PayMatchAmount pMA in payMatchAmountList)
                {
                    ReportDTO dtoPL=pL.Calculate(pMA, reelsOutput);
                    dto.PayoffAmount = dto.PayoffAmount + dtoPL.PayoffAmount;
                    dto.PaylineHitTotal = dto.PaylineHitTotal + dtoPL.PaylineHitTotal;
                }
            }
            if (dto.PaylineHitTotal!=0){
                dto.HitTotal=1;
            }
            dto.SpinTotal = 1;
            return dto;
        }
    }
}
