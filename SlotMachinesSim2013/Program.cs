using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlotMachinesSim2013
{
    class Program
    {
    

        static void Main(string[] args)
        {

            
            int iterationsTotal ;
            int slotMachineTotal;
           if (args.Length == 2)
            {
                
                try
                {
                    iterationsTotal = int.Parse(args[0]);
                    slotMachineTotal = int.Parse(args[1]);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid arguments type..both parameters should be integers");
                    Console.ReadKey();
                    return;
                }
                
            }
            else
            {
                Console.WriteLine("Need to pass only two parameters : iterationsTotal and slotMachineTotal");
                Console.ReadKey();
                return;
            }
            

            
            Console.WriteLine("***Simulating of {0} slot machines with {1} iterations each***",slotMachineTotal,iterationsTotal);
            SlotMachineRunner runner = new SlotMachineRunner("assessment_game.xml", iterationsTotal, slotMachineTotal);
            var result=runner.SpinManyTimes();

            Console.WriteLine("Total PayoffAmount={0}",result.PayoffAmount);
            Console.WriteLine("Total HitTotal={0}", result.HitTotal);
            Console.WriteLine("Total PayLineHitTotal={0}", result.PaylineHitTotal);
            Console.WriteLine("Total SpinTotal={0}", result.SpinTotal);
            if (result.SpinTotal != 0)
            {
                Console.WriteLine("Total HitRate={0}%", (result.HitTotal * 100.00) / result.SpinTotal);
            }
            Console.WriteLine("***Spining All possible Combinations to determine hit rate***");
            runner = new SlotMachineRunner("assessment_game.xml", iterationsTotal, slotMachineTotal);
            result=runner.SpinAllCombinations();
            Console.WriteLine("Total PayoffAmount={0}",result.PayoffAmount);
            Console.WriteLine("Total HitTotal={0}", result.HitTotal);
            Console.WriteLine("Total PayLineHitTotal={0}", result.PaylineHitTotal);
            Console.WriteLine("Total SpinTotal={0}", result.SpinTotal);
            if (result.SpinTotal != 0)
            {
                Console.WriteLine("Total HitRate={0}%", (result.HitTotal * 100.00) / result.SpinTotal);
            }
            Console.ReadKey();
        }
    }

   
}
