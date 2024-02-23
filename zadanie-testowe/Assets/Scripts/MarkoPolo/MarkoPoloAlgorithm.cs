using System;
using System.Collections.Generic;
using System.Text;

namespace TestTask.MarkoPolo
{
    public class MarkoPoloAlgorithm
    {
         private readonly int startNumber;
         private readonly int endNumber;

        public MarkoPoloAlgorithm(int startNumber, int endNumber)
        {
            this.startNumber = startNumber;
            this.endNumber = endNumber;
        }

        public string Execute()
        {
            StringBuilder text = new StringBuilder();

            for (int i = startNumber; i <= endNumber; i++)
            {
               text.AppendLine(DetectWord(i));
            }

            return text.ToString();
        }

        private string DetectWord(int number)
        {
            if (number % 3 == 0 && number % 5 == 0) return "MarkoPolo";
            else if (number % 3 == 0) return "Marko";
            else if (number % 5 == 0) return "Polo";
            
           return number.ToString();
        }
        
    }
}
