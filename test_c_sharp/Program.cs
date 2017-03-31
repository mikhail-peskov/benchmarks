using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace test_c_sharp
{
    class Program
    {
        //---------------- Infrastructure ----------------------
        const int testRepeatCount = 1;

        const long testAccessArraySize_ = 100000000;
        const long testAllocationClassSize_ = 50000000;
        const long testAllocationArraySize_ = 1000000;

        static Stopwatch stopwatch_ = new Stopwatch();

        private static StreamWriter file_ = new System.IO.StreamWriter(@"c_sharp_report.txt");

        static void Start()
        {
            stopwatch_.Restart();
        }

        static double GetTime()
        {
            var result = stopwatch_.ElapsedMilliseconds;
            return result;
        }

        static void WriteString(string outString)
        {
            Console.Write(outString);
            file_.Write(outString);
        }

        static void WriteDouble(double value)
        {
            Console.Write(value);
            file_.Write(value);
        }

        //---------------------------- Benchmarks ---------------------------------------

        static void TestArrayAccess()
        {
            const int arraySize_ = 100000000;

            var array = new int[arraySize_];


            // ------------------- Fill -----------------------------------------------

            double summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < arraySize_; i++)
                {
                    array[i] = i;
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            var avgTime = summTime / testRepeatCount;
            WriteString("Array Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new int[arraySize_];
            summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < arraySize_; i++)
                {
                    destinationArray[i] = array[i];
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            avgTime = summTime / testRepeatCount;
            WriteString("Array Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        class TestInlineMethodsClass
        {
            int InlineMethod(int param1)
            {
                return param1;
            }


            public void test()
            {
                // прогреваем метод
                InlineMethod(0);

                double summTime = 0;
                int summResult = 0;
                for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
                {
                    Start();

                    //--------------------------------------------------
                    for (int i = 0; i < testAccessArraySize_; i++)
                    {
                        summResult = InlineMethod(i);
                    }
                    //--------------------------------------------------

                    var time = GetTime();
                    summTime += time;
                }
                var avgTime = summTime / testRepeatCount;
                WriteString("Inline Method = ");
                WriteDouble(avgTime);
                WriteString(" ms\r\n");
                WriteString("Inline result = ");
                WriteDouble(summResult);
                WriteString("\r\n");

                // ------------------- Copy -----------------------------------------------		
            }
        }

        public class EmptyClass
        {
        };

        public static void DoSomethingWidthEmptyClassArray(EmptyClass[] emptyObjArray) { }

        static void TestEmptyClassMemoryAllocation()
        {
            var array = new EmptyClass[testAllocationClassSize_];

            // --------------------- New Operator Test ---------------------------------

            double summAllocationTime = 0;
            double summDeleteTime = 0;



            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new EmptyClass();
                }
                //--------------------------------------------------

                var time = GetTime();
                summAllocationTime += time;

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Console.WriteLine("Memory before = {0}", memoryBefore);

                Start();

                DoSomethingWidthEmptyClassArray(array);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;

                Console.WriteLine("Memory after = {0}", memoryAfter);
                Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 10 байт: SyncBlock + ReferenceTypePointer
                // плюс 4 байта - ячейка в массиве
                var mustCollectBytes = testAllocationClassSize_ * (16 + 8);
                if (collected < mustCollectBytes) 
                    Console.WriteLine("!!! GC.Collect Wrong");

                //Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                //Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            if (null != array)
                Console.WriteLine("Something Wrong");

            var avgAllocationTime = summAllocationTime / testRepeatCount;
            WriteString("New Class Test = ");
            WriteDouble(avgAllocationTime);
            WriteString(" ms\r\n");

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        public class OneRefClass
        {
            public OneRefClass Reference1;
        };

        public static void DoSomethingWidthOneRefObjArray(OneRefClass[] emptyObj)
        {
        }


        static void TestOneRefClassMemoryAllocation()
        {
            double summDeleteTime = 0;



            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                var array = new OneRefClass[testAllocationClassSize_];

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new OneRefClass();
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Reference1 = array[refIndex];
                }

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthOneRefObjArray(array);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                
                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 10 байт: SyncBlock + MethodTablePointer + ReferenceTypePointer
                // плюс 4 байта - ячейка в массиве, плюс 4 байта - указатель
                var mustCollectBytes = testAllocationClassSize_ * (16 + 8 + 8);
                Console.WriteLine(collected);
                Console.WriteLine(mustCollectBytes);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                //Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                //Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Class with 1 ref class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }
        
        public class FiveRefClass
        {
            public FiveRefClass Reference1;
            public FiveRefClass Reference2;
            public FiveRefClass Reference3;
            public FiveRefClass Reference4;
            public FiveRefClass Reference5;
        };

        public static void DoSomethingWidthFiveRefObjArray(FiveRefClass[] emptyObj)
        {
        }


        static void TestFiveRefClassMemoryAllocation()
        {
            double summDeleteTime = 0;
            
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                var array = new FiveRefClass[testAllocationClassSize_];

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new FiveRefClass();
                    long refIndex = (i + 5) % testAllocationClassSize_;
                    array[i].Reference1 = array[refIndex];
                    array[i].Reference2 = array[refIndex - 1];
                    array[i].Reference3 = array[refIndex - 2];
                    array[i].Reference4 = array[refIndex = 3];
                    array[i].Reference5 = array[refIndex - 4];
                }

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthFiveRefObjArray(array);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 10 байт: SyncBlock + MethodTablePointer + ReferenceTypePointer
                // плюс 4 байта - ячейка в массиве, плюс 4 байта - указатель
                var mustCollectBytes = testAllocationClassSize_ * (16 + 8 + 8 * 5);
                Console.WriteLine(collected);
                Console.WriteLine(mustCollectBytes);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                //Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                //Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Class with 1 ref class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }





        public static void DoSomethingWithArray(int[] array) { }

        static void TestArraysMemoryAllocation()
        {
            var array = new int[testAccessArraySize_][];


            double summAllocationTime = 0;
            double summDeleteTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {

                // --------------------- New Operator Test ---------------------------------
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAllocationArraySize_; i++)
                {
                    array[i] = new int[100];
                }
                //--------------------------------------------------

                var time = GetTime();
                summAllocationTime += time;

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWithArray(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 10 байт: SyncBlock + MethodTablePointer + ReferenceTypePointer
                // плюс 4 байта - ячейка в массиве
                // плюс 100 4-байтных чисел в самом массиве
                var mustCollectBytes = testAllocationArraySize_ * (16 + 8 + 100 * 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                //Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                //Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgAllocationTime = summAllocationTime / testRepeatCount;
            WriteString("New Array Test = ");
            WriteDouble(avgAllocationTime);
            WriteString(" ms\r\n");

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Array Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        static void TestClassMemoryAllocationMT()
        {
            var array = new EmptyClass[testAllocationClassSize_];

            // --------------------- New Operator Test ---------------------------------

            double summAllocTime = 0;
            double summDeleteTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                var allocThread1 = new Thread(() =>
                    {
                        for (int i = 0; i < testAllocationClassSize_ / 4; i++)
                        {
                            array[i] = new EmptyClass();
                        }
                    });

                var allocThread2 = new Thread(() =>
                {
                    for (long i = testAllocationClassSize_ / 4; i < testAllocationClassSize_ / 2; i++)
                    {
                        array[i] = new EmptyClass();
                    }
                });


                var allocThread3 = new Thread(() =>
                {
                    for (long i = testAllocationClassSize_ / 2; i < testAllocationClassSize_ * 3 / 4; i++)
                    {
                        array[i] = new EmptyClass();
                    }
                });

                var allocThread4 = new Thread(() =>
                {
                    for (long i = testAllocationClassSize_ * 3 / 4; i < testAllocationClassSize_; i++)
                    {
                        array[i] = new EmptyClass();
                    }
                });

                allocThread1.Start();
                allocThread2.Start();
                allocThread3.Start();
                allocThread4.Start();
                
                allocThread1.Join();
                allocThread2.Join();
                allocThread3.Join();
                allocThread4.Join();


                var time = GetTime();
                summAllocTime += time;


                // ---------------------- Delete Operator Test ------------------------


                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthEmptyClassArray(array);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 10 байт: SyncBlock + MethodTablePointer + ReferenceTypePointer
                // плюс 4 байта - ячейка в массиве
                var mustCollectBytes = testAllocationClassSize_ * (16 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                //Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                //Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

        var avgTime = summAllocTime / testRepeatCount;

        WriteString("New Class Test MT = ");

        WriteDouble(avgTime);

        WriteString(" ms\r\n");

        avgTime = summDeleteTime / testRepeatCount;

        WriteString("Delete Class Test MT = ");

        WriteDouble(avgTime);

        WriteString(" ms\r\n");
            
    }


    static void Main(string[] args)
        {
            //TestInlineMethodsClass testInlineMethodsObject = new TestInlineMethodsClass();
            //testInlineMethodsObject.test();

            //TestArrayAccess();
            //TODO: попробовать unmanaged-доступ

            TestEmptyClassMemoryAllocation();
            TestOneRefClassMemoryAllocation();
            //TestArraysMemoryAllocation();
            //TestClassMemoryAllocationMT();

            Console.ReadLine();
        }
    }
}
