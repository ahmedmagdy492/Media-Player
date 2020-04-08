using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Models
{
    internal class Time
    {
        public int Seconds { get; set; }
        public int Mintues { get; set; }
        public int Hours { get; set; }

        public void TimeFunction()
        {
            if (this.Seconds < 59)
            {
                this.Seconds++;
            }
            else
            {
                Seconds = 0;
                if (Mintues < 59)
                {
                    Mintues++;
                }
                else
                {
                    Mintues = 0;
                    Hours++;
                }
            }
        }

        public void CountDown()
        {
            if(Seconds > 0)
            {
                Seconds--;
            }
            else
            {
                if(Mintues > 0)
                {
                    Mintues--;
                    Seconds = 59;
                }
                else
                {
                    if(Hours > 0)
                    {
                        Hours--;
                        Mintues = 59;
                    }
                }
            }
        }
    }
}
