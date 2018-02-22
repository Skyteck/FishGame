using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishGame
{
    public static class ArmadaRandom
    {
        static Random myRan;
        
        public static int Next()
        {
            if(myRan == null)
            {
                myRan = new Random();
            }

            return myRan.Next();
        }

        public static int Next(int min)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }

            return myRan.Next(min);
        }

        public static int Next(int min, int max)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }

            return myRan.Next(min, max);
        }

        public static float NextFloat(float precision)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }

            
            return myRan.Next() / precision;

        }

        public static float NextFloat(float precision, float min)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }
            int i = (int)(min * precision);
            return myRan.Next(i) / precision;

        }

        public static float NextFloat(float precision, int min, int max)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }
            //int i = (int)(min * precision);
            //int j = (int)(max * precision);
            return myRan.Next(min, max) / precision;

        }

        public static double NextDouble(double precision)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }

            return myRan.Next() / precision;

        }

        public static double NextDouble(double precision, float min)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }
            int i = (int)(min * precision);
            return myRan.Next(i) / precision;

        }

        public static double NextDouble(double precision, float min, float max)
        {
            if (myRan == null)
            {
                myRan = new Random();
            }
            int i = (int)(min * precision);
            int j = (int)(max * precision);
            return myRan.Next(i, j) / precision;

        }
    }
}
