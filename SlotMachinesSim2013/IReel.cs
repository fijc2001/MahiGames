using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachinesSim2013
{
  public interface  IReel 
    {
       List<SlotSymbolOffset> Spin();
       List<ESlotSymbol> Strip
       {
           get;
       }

    }
}
