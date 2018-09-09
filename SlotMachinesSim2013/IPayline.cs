using System;
namespace SlotMachinesSim2013
{
   public interface IPayline
    {
        ReportDTO Calculate(PayMatchAmount match, System.Collections.Generic.List<System.Collections.Generic.List<SlotSymbolOffset>> reelOutputs);
        System.Collections.Generic.List<ESlotOffset> VerticalOffsets { get; }
    }
}
