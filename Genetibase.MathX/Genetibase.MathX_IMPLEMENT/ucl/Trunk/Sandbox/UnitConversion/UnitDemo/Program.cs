using System;
using System.Collections.Generic;
using System.Text;

using UnitConversion;

namespace UnitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Quantity d1 = Factory.u(12, x.m);
            Quantity d2 = Factory.u(5, x.km);
            Quantity distance = d1 + d2;

            Quantity time = Factory.u(100, x.s) + d1;
            Console.WriteLine("Distance is " + distance.shortString());
            Console.WriteLine("Time is " + time.shortString());

            CompoundUnit mps = x.m / x.s;
            CompoundUnit kph = x.km / x.hr;
            Quantity windspeed = Factory.u(123, mps);
            Quantity carspeed = Factory.u(10, kph);
            Quantity total = windspeed + carspeed;

            Console.WriteLine("Speed is " + total.shortString());

            Console.ReadLine();
        }
    }
}
