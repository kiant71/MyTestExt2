﻿using System;

namespace MyTestExt.ConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
            

                new ThumbnailTest().Do();

            //new ZipArchiveCoreTest().Do();
            



            }
            catch (Exception e)
            {
                //
            }

            while (true)
                System.Threading.Thread.Sleep(1000);
        }
    }
}
