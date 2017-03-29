using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace test_c_sharp
{
    class Program
    {
        //---------------- Infrastructure ----------------------
        const int testRepeatCount = 1;

        const int testAccessArraySize_ = 100000000;
        const int testAllocationClassSize_ = 50000000;
        const int testAllocationArraySize_ = 1000000;

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
            WriteString("ms\r\n");

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
            WriteString("ms\r\n");
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
                WriteString("ms\r\n");
                WriteString("Inline result = ");
                WriteDouble(summResult);
                WriteString("\r\n");

                // ------------------- Copy -----------------------------------------------		
            }
        }

        class EmptyClass
        {
        };

        static void TestClassMemoryAllocation()
        {
            var array = new EmptyClass[testAllocationClassSize_];

            // --------------------- New Operator Test ---------------------------------

            double summAllocationTime = 0;
            double summDeleteTime = 0;

            GC.Collect();

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
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

                Start();

                array = null;
                GC.Collect();
                

                time = GetTime();
                summDeleteTime += time;
            }

            var avgAllocationTime = summAllocationTime / testRepeatCount;
            WriteString("New Class Test = ");
            WriteDouble(avgAllocationTime);
            WriteString("ms\r\n");

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString("ms\r\n");
        }

        static void TestArraysMemoryAllocation()
        {
            var array = new int[testAccessArraySize_][];


            double summAllocationTime = 0;
            double summDeleteTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {

                // --------------------- New Operator Test ---------------------------------

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

                Start();

                //--------------------------------------------------
                array = null;
                GC.Collect();
                //--------------------------------------------------

                time = GetTime();
                summDeleteTime += time;
            }

            var avgAllocationTime = summAllocationTime / testRepeatCount;
            WriteString("New Array Test = ");
            WriteDouble(avgAllocationTime);
            WriteString("ms\r\n");

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Array Test = ");
            WriteDouble(avgDeleteTime);
            WriteString("ms\r\n");
        }

        void TestClassMemoryAllocationMT()
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
                    for (int i = testAllocationClassSize_ / 4; i < testAllocationClassSize_ / 2; i++)
                    {
                        array[i] = new EmptyClass();
                    }
                });


            var allocThread3 = new Thread(() =>
            {
                for (int i = testAllocationClassSize_ / 2; i < testAllocationClassSize_ * 3 / 4; i++)
                {
                    array[i] = new EmptyClass();
                }
            });

            var allocThread4 = new Thread(() =>
            {
                for (int i = testAllocationClassSize_ * 3 / 4; i < testAllocationClassSize_; i++)
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

            Start();

            array = null;
            GC.Collect();
            time = GetTime();
            summDeleteTime += time;
        }

        var avgTime = summAllocTime / testRepeatCount;

        WriteString("New Class Test MT = ");

        WriteDouble(avgTime);

        WriteString("ms\r\n");

        avgTime = summDeleteTime / testRepeatCount;

        WriteString("Delete Class Test MT = ");

        WriteDouble(avgTime);

        WriteString("ms\r\n");
            
    }


    static void Main(string[] args)
        {
            TestInlineMethodsClass testInlineMethodsObject = new TestInlineMethodsClass();
            testInlineMethodsObject.test();

            TestArrayAccess();
            //TODO: попробовать unmanaged-доступ
            
            TestClassMemoryAllocation();
            TestArraysMemoryAllocation();
            TestArraysMemoryAllocation();

            Console.ReadLine();
        }
    }
}
