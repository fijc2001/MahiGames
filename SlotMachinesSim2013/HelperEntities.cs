using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlotMachinesSim2013
{
   public enum ESlotSymbol
    {
        Ace = 1, King, Queen, Jack, Ten, Nine
    }
   public  enum ESlotOffset
    {
        Fisrt=0, Second, Third
    }

  public  class SlotSymbolOffset
    {
        private ESlotOffset offset ;
        private ESlotSymbol symbol;
        public  SlotSymbolOffset(ESlotOffset offset,ESlotSymbol symbol)
        {
            this.offset = offset;
            this.symbol = symbol;
        }
        public ESlotOffset Offset
        {
            get { return  offset; }
            
        }

        public ESlotSymbol Symbol
        {
            get { return symbol; }

        }
        

    }

    public struct ReportDTO{

        public float PayoffAmount;
        public int HitTotal;
        public int PaylineHitTotal;
        public int SpinTotal;
    }

   public  class PayMatchAmount
    {
       private List<ESlotSymbol> exactMatch;

       private float amount;
       public PayMatchAmount(List<ESlotSymbol> exactMatch, float amount)
       {
           this.exactMatch=exactMatch;
           this.amount=amount;
       }

       public PayMatchAmount(XElement config)
       {
           this.amount = float.Parse(config.Attribute("Amount").Value);
           this.exactMatch = config.Attribute("ExactMatch").Value.Split(',')
                                                                .ToList<string>()
                                                                .Select(s => s.ToEnum<ESlotSymbol>())
                                                                .ToList<ESlotSymbol>();
         
       }

       public float Amount
        {
          get { return amount; }
        }
        
       public List<ESlotSymbol> ExactMatch
        {
          get { return exactMatch; }
          //set { exactMatch = value; }
        }

    } 
 
}
