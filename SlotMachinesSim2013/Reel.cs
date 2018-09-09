using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlotMachinesSim2013
{
    static class RandomGen
    {
        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) //this is to make it thread safe
            {
                return getrandom.Next(min, max);
            }
        }
        public static int GetRandomNumber(int max)
        {
            lock (getrandom) //this is to make it thread safe
            {
                return getrandom.Next(max);
            }
        }

   
    }
    static class EnumExt
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

    public class Reel :IReel
    {
        int reelWindow;
        List<ESlotSymbol> strip;

        
        int rndLenght;
        string nameReel;
        public Reel(int reelWindow,List<ESlotSymbol> stripParam, string nameReel="NA")
        {
            this.reelWindow = reelWindow;
            strip = stripParam;
            this.nameReel = nameReel;
            this.rndLenght = strip.Count;
            /*adding the needed elements at the end to make it  circular as per requirement*/
            var excess = strip.Take<ESlotSymbol>(reelWindow - 1).ToList<ESlotSymbol>();
            strip.AddRange(excess);
        }
        public Reel(int reelWindow, XElement config)
        {
            this.reelWindow = reelWindow;
            this.nameReel=config.Attribute("name").Value;
            this.strip = config.Attribute("Symbols").Value.Split(',')
                                                                .ToList<string>()
                                                                .Select( s => s.ToEnum<ESlotSymbol>())
                                                                .ToList<ESlotSymbol>();
            this.rndLenght = strip.Count;
            /*adding the needed elements at the end to make it  circular as per requirement*/
            var excess = strip.Take<ESlotSymbol>(reelWindow - 1).ToList<ESlotSymbol>();
            strip.AddRange(excess);


        }
        public List<ESlotSymbol> Strip
        {
            get { return strip; }
          
        }
      public   List<SlotSymbolOffset> Spin()
        {          
            List<ESlotSymbol> slice;
            List<SlotSymbolOffset> result=null;
            int index=RandomGen.GetRandomNumber(rndLenght);
            slice = strip.Skip(index).Take(reelWindow).ToList<ESlotSymbol>();
            var offsets = Enum.GetValues(typeof(ESlotOffset)).Cast<ESlotOffset>();
            result=slice.Zip(offsets, (s, o) => new SlotSymbolOffset(o, s)).ToList<SlotSymbolOffset>();
            return result;
        }
    }
}
