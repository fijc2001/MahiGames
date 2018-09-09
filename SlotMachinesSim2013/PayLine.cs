using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlotMachinesSim2013
{
    public class Payline : IPayline
    {
        string name;
        List<ESlotOffset> verticalOffsets;


        public Payline(string name, List<ESlotOffset> verticalOffsets)
        {
            this.name = name;
            this.verticalOffsets = verticalOffsets;
        }

        public Payline(XElement config)
        {
            this.name = config.Attribute("name").Value;
            this.verticalOffsets = config.Attribute("VerticalOffsets").Value.Split(',')
                                                                 .ToList<string>()
                                                                 .Select(s => s.ToEnum<ESlotOffset>())
                                                                 .ToList<ESlotOffset>();

        }

        public List<ESlotOffset> VerticalOffsets
        {
            get { return verticalOffsets; }

        }

        public ReportDTO Calculate(PayMatchAmount match, List<List<SlotSymbolOffset>> reelOutputs)
        {
            ReportDTO dto = new ReportDTO
            {
                HitTotal = 0,
                PayoffAmount = 0,
                PaylineHitTotal = 0,
                SpinTotal = 0
            };
            bool wasHit = true;
            List<SlotSymbolOffset> matchesOffset = new List<SlotSymbolOffset>();
            List<bool> isMatchList = new List<bool>();

            matchesOffset = match.ExactMatch.Zip(verticalOffsets, (me, v) => new SlotSymbolOffset(v, me)).ToList<SlotSymbolOffset>();
            isMatchList = matchesOffset.Zip(reelOutputs, (m, r) =>
            {
                bool wasHitReel = false;
                foreach (SlotSymbolOffset so in r)
                {
                    if (so.Offset == m.Offset && so.Symbol == m.Symbol)
                    {
                        wasHitReel = true;
                        break;
                    }
                }

                return wasHitReel;

            }).ToList<bool>();
            foreach (bool m in isMatchList)
            {

                wasHit = m && wasHit;
            }
            if (wasHit == true)
            {
                dto.PayoffAmount = match.Amount;
                dto.PaylineHitTotal = 1;
            }
            else
            {
                dto.PayoffAmount = -match.Amount;

            }


            return dto;
        }


    }

}
