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
        const int testRepeatCount = 100;

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
            var array = new int[testAccessArraySize_];


            // ------------------- Fill -----------------------------------------------

            double summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAccessArraySize_; i++)
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


            var destinationArray = new int[testAccessArraySize_];
            summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAccessArraySize_; i++)
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

        static void TestListAccess()
        {
            var array = new List<int>(new int[testAccessArraySize_]);
            
            // ------------------- Fill -----------------------------------------------

            double summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAccessArraySize_; i++)
                {
                    array[i] = i;
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            var avgTime = summTime / testRepeatCount;
            WriteString("List Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new List<int>(new int[testAccessArraySize_]);
            summTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < testAccessArraySize_; i++)
                {
                    destinationArray[i] = array[i];
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            avgTime = summTime / testRepeatCount;
            WriteString("List Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }


        static unsafe void TestArrayUnsafeAccess()
        {
            var array = new int[testAccessArraySize_];


            // ------------------- Fill -----------------------------------------------
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pArray = array)
                {
                    //--------------------------------------------------
                    for (int i = 0; i < testAccessArraySize_; i++)
                    {
                        pArray[i] = i;
                    }
                    //--------------------------------------------------
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / testRepeatCount;
            WriteString("Array Unsafe Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new int[testAccessArraySize_];
            summTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pArray = array)
                fixed (int* pDestinationArray = destinationArray)
                {
                    //--------------------------------------------------
                    for (int i = 0; i < testAccessArraySize_; i++)
                    {
                        pDestinationArray[i] = pArray[i];
                    }
                    //--------------------------------------------------
                }

                var time = GetTime();
                summTime += time;
            }

            avgTime = summTime / testRepeatCount;
            WriteString("Array Unsafe Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static void TestArrayRandomAccess()
        {
            var indexArray = new int[testAccessArraySize_];
            var indexRandom = new Random();
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                indexArray[i] = indexRandom.Next(testAccessArraySize_);
            }

            var sourceArray = new int[testAccessArraySize_];
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[testAccessArraySize_];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < testAccessArraySize_; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / testRepeatCount;
            WriteString("Array Random Access = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }
        
        static unsafe void TestArrayRandomAccessUnsafe()
        {
            var indexArray = new int[testAccessArraySize_];
            var indexRandom = new Random();
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                indexArray[i] = indexRandom.Next(testAccessArraySize_);
            }

            var sourceArray = new int[testAccessArraySize_];
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[testAccessArraySize_];
            double summTime = 0;

            fixed (int* pIndexArray = indexArray)
            fixed (int* pSourceArray = sourceArray)
            fixed (int* pDestinationArray = destinationArray)
            {
                for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
                {
                    Start();

                    for (int i = 0; i < testAccessArraySize_; i++)
                    {
                        int index = pIndexArray[i];
                        pDestinationArray[index] = pSourceArray[index];
                    }

                    var time = GetTime();
                    summTime += time;
                }
            }

            var avgTime = summTime / testRepeatCount;
            WriteString("Array Random Access Unsafe = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static unsafe void TestArrayRandomAccessUnsafePointerArythmetic()
        {
            var indexArray = new int[testAccessArraySize_];
            var indexRandom = new Random();
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                indexArray[i] = indexRandom.Next(testAccessArraySize_);
            }

            var sourceArray = new int[testAccessArraySize_];
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[testAccessArraySize_];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                fixed (int* pIndexArrayFixed = indexArray)
                fixed (int* pSourceArrayFixed = sourceArray)
                fixed (int* pDestinationArrayFixed = destinationArray)
                {
                    int* pIndexArray = pIndexArrayFixed;
                    int* pSourceArray = pSourceArrayFixed;
                    int* pDestinationArray = pDestinationArrayFixed;
                    Start();

                    int* pEndIndex = pIndexArray + testAccessArraySize_;
                    for (; pIndexArray < pEndIndex; pIndexArray++)
                    {
                        int index = *pIndexArray;
                        *(pDestinationArray + index) = *(pSourceArray + index);
                    }

                    var time = GetTime();
                    summTime += time;
                }
            }

            var avgTime = summTime / testRepeatCount;
            WriteString("Array Random Access Unsafe Pointers = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static void TestListRandomAccess()
        {
            var indexArray = new List<int>(new int[testAccessArraySize_]);
            var indexRandom = new Random();
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                indexArray[i] = indexRandom.Next(testAccessArraySize_);
            }

            var sourceArray = new List<int>(new int[testAccessArraySize_]);
            for (int i = 0; i < testAccessArraySize_; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new List<int>(new int[testAccessArraySize_]);
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < testAccessArraySize_; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / testRepeatCount;
            WriteString("List Random Access = ");
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

        #region -----------------------   Empty classs ----------------------------------

        public class EmptyClass
        {
        };

        public static void DoSomethingWidthEmptyObject(EmptyClass emptyObj) { }

        static void TestEmptyClassMemoryAllocation()
        {


            // --------------------- New Operator Test ---------------------------------

            double summAllocationTime = 0;
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthEmptyObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new EmptyClass[testAllocationClassSize_];

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

                Start();

                DoSomethingWidthEmptyObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgAllocationTime = summAllocationTime / testRepeatCount;
            WriteString("New Class Test = ");
            WriteDouble(avgAllocationTime);
            WriteString(" ms\r\n");

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Empty Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }


        #endregion

        #region -----------------------   One Ref classs ----------------------------------

        public class OneRefClass
        {
            public OneRefClass Ref1;
        };

        public static void DoSomethingWidthOneRefObject(OneRefClass emptyObj) { }


        static void TestOneRefClassMemoryAllocation()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthOneRefObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new OneRefClass[testAllocationClassSize_];

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new OneRefClass();
                }

                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Ref1 = array[refIndex];
                }
                //--------------------------------------------------

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthOneRefObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete One Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion

        #region -----------------------   Five Ref classs ----------------------------------

        public class FiveRefClass
        {
            public FiveRefClass Ref1;
            public FiveRefClass Ref2;
            public FiveRefClass Ref3;
            public FiveRefClass Ref4;
            public FiveRefClass Ref5;
        };

        public static void DoSomethingWidthFiveRefObject(FiveRefClass emptyObj) { }


        static void TestFiveRefClassMemoryAllocation()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthFiveRefObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FiveRefClass[testAllocationClassSize_];

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new FiveRefClass();
                }

                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % testAllocationClassSize_;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % testAllocationClassSize_;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % testAllocationClassSize_;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % testAllocationClassSize_;
                    array[i].Ref5 = array[refIndex];
                }
                //--------------------------------------------------

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthFiveRefObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8 + 8 * 5);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Five Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Ten Ref classs ----------------------------------

        public class TenRefClass
        {
            public TenRefClass Ref1;
            public TenRefClass Ref2;
            public TenRefClass Ref3;
            public TenRefClass Ref4;
            public TenRefClass Ref5;
            public TenRefClass Ref6;
            public TenRefClass Ref7;
            public TenRefClass Ref8;
            public TenRefClass Ref9;
            public TenRefClass Ref10;
        };

        public static void DoSomethingWidthTenRefObject(TenRefClass emptyObj) { }


        static void TestTenRefClassMemoryAllocation()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            DoSomethingWidthTenRefObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TenRefClass[testAllocationClassSize_];

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new TenRefClass();
                }

                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % testAllocationClassSize_;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % testAllocationClassSize_;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % testAllocationClassSize_;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % testAllocationClassSize_;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % testAllocationClassSize_;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % testAllocationClassSize_;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % testAllocationClassSize_;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % testAllocationClassSize_;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % testAllocationClassSize_;
                    array[i].Ref10 = array[refIndex];
                }
                //--------------------------------------------------

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthTenRefObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8 + 8 * 10);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Ten Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Fifteen Ref classs ----------------------------------

        public class FifteenRefClass
        {
            public FifteenRefClass Ref1;
            public FifteenRefClass Ref2;
            public FifteenRefClass Ref3;
            public FifteenRefClass Ref4;
            public FifteenRefClass Ref5;
            public FifteenRefClass Ref6;
            public FifteenRefClass Ref7;
            public FifteenRefClass Ref8;
            public FifteenRefClass Ref9;
            public FifteenRefClass Ref10;
            public FifteenRefClass Ref11;
            public FifteenRefClass Ref12;
            public FifteenRefClass Ref13;
            public FifteenRefClass Ref14;
            public FifteenRefClass Ref15;
        };

        public static void DoSomethingWidthFifteenRefObject(FifteenRefClass emptyObj) { }


        static void TestFifteenRefClassMemoryAllocation()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthFifteenRefObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FifteenRefClass[testAllocationClassSize_];

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new FifteenRefClass();
                }

                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % testAllocationClassSize_;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % testAllocationClassSize_;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % testAllocationClassSize_;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % testAllocationClassSize_;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % testAllocationClassSize_;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % testAllocationClassSize_;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % testAllocationClassSize_;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % testAllocationClassSize_;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % testAllocationClassSize_;
                    array[i].Ref10 = array[refIndex];
                    refIndex = (i + 11) % testAllocationClassSize_;
                    array[i].Ref11 = array[refIndex];
                    refIndex = (i + 12) % testAllocationClassSize_;
                    array[i].Ref12 = array[refIndex];
                    refIndex = (i + 13) % testAllocationClassSize_;
                    array[i].Ref13 = array[refIndex];
                    refIndex = (i + 14) % testAllocationClassSize_;
                    array[i].Ref14 = array[refIndex];
                    refIndex = (i + 15) % testAllocationClassSize_;
                    array[i].Ref15 = array[refIndex];
                }
                //--------------------------------------------------

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthFifteenRefObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8 + 8 * 15);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Fifteen Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Twenty Ref classs ----------------------------------

        public class TwentyRefClass
        {
            public TwentyRefClass Ref1;
            public TwentyRefClass Ref2;
            public TwentyRefClass Ref3;
            public TwentyRefClass Ref4;
            public TwentyRefClass Ref5;
            public TwentyRefClass Ref6;
            public TwentyRefClass Ref7;
            public TwentyRefClass Ref8;
            public TwentyRefClass Ref9;
            public TwentyRefClass Ref10;
            public TwentyRefClass Ref11;
            public TwentyRefClass Ref12;
            public TwentyRefClass Ref13;
            public TwentyRefClass Ref14;
            public TwentyRefClass Ref15;
            public TwentyRefClass Ref16;
            public TwentyRefClass Ref17;
            public TwentyRefClass Ref18;
            public TwentyRefClass Ref19;
            public TwentyRefClass Ref20;
        };

        public static void DoSomethingWidthTwentyRefObject(TwentyRefClass emptyObj) { }


        static void TestTwentyRefClassMemoryAllocation()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            DoSomethingWidthTwentyRefObject(null);

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TwentyRefClass[testAllocationClassSize_];

                //--------------------------------------------------
                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    array[i] = new TwentyRefClass();
                }

                for (int i = 0; i < testAllocationClassSize_; i++)
                {
                    long refIndex = (i + 1) % testAllocationClassSize_;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % testAllocationClassSize_;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % testAllocationClassSize_;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % testAllocationClassSize_;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % testAllocationClassSize_;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % testAllocationClassSize_;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % testAllocationClassSize_;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % testAllocationClassSize_;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % testAllocationClassSize_;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % testAllocationClassSize_;
                    array[i].Ref10 = array[refIndex];

                    refIndex = (i + 11) % testAllocationClassSize_;
                    array[i].Ref11 = array[refIndex];
                    refIndex = (i + 12) % testAllocationClassSize_;
                    array[i].Ref12 = array[refIndex];
                    refIndex = (i + 13) % testAllocationClassSize_;
                    array[i].Ref13 = array[refIndex];
                    refIndex = (i + 14) % testAllocationClassSize_;
                    array[i].Ref14 = array[refIndex];
                    refIndex = (i + 15) % testAllocationClassSize_;
                    array[i].Ref15 = array[refIndex];
                    refIndex = (i + 16) % testAllocationClassSize_;
                    array[i].Ref16 = array[refIndex];
                    refIndex = (i + 17) % testAllocationClassSize_;
                    array[i].Ref17 = array[refIndex];
                    refIndex = (i + 18) % testAllocationClassSize_;
                    array[i].Ref18 = array[refIndex];
                    refIndex = (i + 19) % testAllocationClassSize_;
                    array[i].Ref19 = array[refIndex];
                    refIndex = (i + 20) % testAllocationClassSize_;
                    array[i].Ref20 = array[refIndex];
                }
                //--------------------------------------------------

                // ---------------------- Delete Operator Test ------------------------

                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthTwentyRefObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                var time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock + ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = (double)testAllocationClassSize_ * (16 + 8 + 8 * 20);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / testRepeatCount;
            WriteString("Delete Twenty Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion



        public static void DoSomethingWithArray(int[] array) { }

        static void TestArraysMemoryAllocation()
        {
            double summAllocationTime = 0;
            double summDeleteTime = 0;
            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                // --------------------- New Operator Test ---------------------------------
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new int[testAccessArraySize_][];

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

                // объукт пустого класса  CLR = 16 байт: SyncBlock +  ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                // плюс 100 4-байтных чисел в самом массиве
                var mustCollectBytes = testAllocationArraySize_ * (16 + 8 + 100 * 4);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
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


            // --------------------- New Operator Test ---------------------------------

            double summAllocTime = 0;
            double summDeleteTime = 0;

            for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
            {
                Start();

                var array = new EmptyClass[testAllocationClassSize_];

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


                double memoryBefore = GC.GetTotalMemory(true);

                Start();

                DoSomethingWidthEmptyObject(array[0]);
                array = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);


                time = GetTime();
                summDeleteTime += time;
                double memoryAfter = GC.GetTotalMemory(true);
                double collected = memoryBefore - memoryAfter;
                //Console.WriteLine("Collected = {0}", collected);

                // объукт пустого класса  CLR = 16 байт: SyncBlock +  ReferenceTypePointer
                // плюс 8 байт - ячейка в массиве
                var mustCollectBytes = testAllocationClassSize_ * (16 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
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
            var totalStopwatch = new Stopwatch();
            totalStopwatch.Restart();

            //TestInlineMethodsClass testInlineMethodsObject = new TestInlineMethodsClass();
            //testInlineMethodsObject.test();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            //TestArrayAccess();
            //TestArrayUnsafeAccess();
            //TestListAccess();

            //TestArrayRandomAccess();
            //TestArrayRandomAccessUnsafe();
            TestArrayRandomAccessUnsafePointerArythmetic();
            //TestListRandomAccess();
            Console.WriteLine("---------------------");
            file_.Flush();

            //TestEmptyClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestOneRefClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestFiveRefClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestTenRefClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestFifteenRefClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestTwentyRefClassMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestArraysMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestClassMemoryAllocationMT();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            Console.WriteLine("--- Complete ---");

            Console.WriteLine("Total Time = {0}", totalStopwatch.Elapsed.ToString());

            file_.Close();

            Console.ReadLine();
        }
    }
}
    