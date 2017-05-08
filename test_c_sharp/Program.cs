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
        const int _threadCount = 8;

        const int _testRepeatCount = 100;

        const int _testArrayAccessSize = 100000000;
        const int _testClassAllocationSize = 10000000;
        

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
            var array = new int[_testArrayAccessSize];


            // ------------------- Fill -----------------------------------------------

            double summTime = 0;
            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    array[i] = i;
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new int[_testArrayAccessSize];
            summTime = 0;
            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    destinationArray[i] = array[i];
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            avgTime = summTime / _testRepeatCount;
            WriteString("Array Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static void TestListAccess()
        {
            var array = new List<int>(new int[_testArrayAccessSize]);
            
            // ------------------- Fill -----------------------------------------------

            double summTime = 0;
            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    array[i] = i;
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            var avgTime = summTime / _testRepeatCount;
            WriteString("List Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new List<int>(new int[_testArrayAccessSize]);
            summTime = 0;
            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                //--------------------------------------------------
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    destinationArray[i] = array[i];
                }
                //--------------------------------------------------

                var time = GetTime();
                summTime += time;
            }
            avgTime = summTime / _testRepeatCount;
            WriteString("List Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }


        static unsafe void TestArrayUnsafeAccess()
        {
            var array = new int[_testArrayAccessSize];


            // ------------------- Fill -----------------------------------------------
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pArray = array)
                {
                    //--------------------------------------------------
                    for (int i = 0; i < _testArrayAccessSize; i++)
                    {
                        pArray[i] = i;
                    }
                    //--------------------------------------------------
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Unsafe Fill = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");

            // ------------------- Copy -----------------------------------------------


            var destinationArray = new int[_testArrayAccessSize];
            summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pArray = array)
                fixed (int* pDestinationArray = destinationArray)
                {
                    //--------------------------------------------------
                    for (int i = 0; i < _testArrayAccessSize; i++)
                    {
                        pDestinationArray[i] = pArray[i];
                    }
                    //--------------------------------------------------
                }

                var time = GetTime();
                summTime += time;
            }

            avgTime = summTime / _testRepeatCount;
            WriteString("Array Unsafe Copy = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        // --------------------------- Full Random Access -----------------------------

        static void TestArrayRandomAccess()
        {
            var indexArray = new int[_testArrayAccessSize];

            // --------------------------------------------------------------------
            //using (var randValuesFile = new System.IO.StreamWriter(@"random_values.txt"))
            //{
            //    var indexRandom = new Random();
            //    for (int i = 0; i < _testArrayAccessSize; i++)
            //    {
            //        var nextValue = indexRandom.Next(_testArrayAccessSize);
            //        randValuesFile.WriteLine(nextValue);
            //    }
            //}
            //Console.WriteLine("Random File Generated");
            //Console.ReadLine();
            // --------------------------------------------------------------------


            using (StreamReader sr = File.OpenText("random_values.txt"))
            {
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    String line = sr.ReadLine();
                    var nextValue = int.Parse(line);
                    indexArray[i] = nextValue;
                }
            }

            Console.WriteLine("Random Files added");

            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }
        
        static unsafe void TestArrayRandomAccessUnsafe()
        {
            var indexArray = new int[_testArrayAccessSize];

            using (StreamReader sr = File.OpenText("random_values.txt"))
            {
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    String line = sr.ReadLine();
                    var nextValue = int.Parse(line);
                    indexArray[i] = nextValue;
                }
            }

            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;

            fixed (int* pIndexArray = indexArray)
            fixed (int* pSourceArray = sourceArray)
            fixed (int* pDestinationArray = destinationArray)
            {
                for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
                {
                    Start();

                    for (int i = 0; i < _testArrayAccessSize; i++)
                    {
                        int index = pIndexArray[i];
                        pDestinationArray[index] = pSourceArray[index];
                    }

                    var time = GetTime();
                    summTime += time;
                }
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access Unsafe = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static unsafe void TestArrayRandomAccessUnsafePointerArythmetic()
        {
            var indexArray = new int[_testArrayAccessSize];
            //var indexRandom = new Random();
            //for (int i = 0; i < _testArrayAccessSize; i++)
            //{
            //    indexArray[i] = indexRandom.Next(_testArrayAccessSize);
            //}

            using (StreamReader sr = File.OpenText("random_values.txt"))
            {
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    String line = sr.ReadLine();
                    var nextValue = int.Parse(line);
                    indexArray[i] = nextValue;
                }
            }

            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                fixed (int* pIndexArrayFixed = indexArray)
                fixed (int* pSourceArrayFixed = sourceArray)
                fixed (int* pDestinationArrayFixed = destinationArray)
                {
                    int* pIndexArray = pIndexArrayFixed;
                    int* pSourceArray = pSourceArrayFixed;
                    int* pDestinationArray = pDestinationArrayFixed;
                    Start();

                    int* pEndIndex = pIndexArray + _testArrayAccessSize;
                    for (; pIndexArray < pEndIndex; pIndexArray++)
                    {
                        int index = *pIndexArray;
                        *(pDestinationArray + index) = *(pSourceArray + index);
                    }

                    var time = GetTime();
                    summTime += time;
                }
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access Unsafe Pointers = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static void TestListRandomAccess()
        {
            var indexArray = new List<int>(new int[_testArrayAccessSize]);
            //var indexRandom = new Random();
            //for (int i = 0; i < _testArrayAccessSize; i++)
            //{
            //    indexArray[i] = indexRandom.Next(_testArrayAccessSize);
            //}

            using (StreamReader sr = File.OpenText("random_values.txt"))
            {
                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    String line = sr.ReadLine();
                    var nextValue = int.Parse(line);
                    indexArray[i] = nextValue;
                }
            }

            var sourceArray = new List<int>(new int[_testArrayAccessSize]);
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new List<int>(new int[_testArrayAccessSize]);
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("List Random Access = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        // ----------------------------------------------------------------------------

        // --------------------------- Sequence Random Access -----------------------------

        static void TestArrayRandomAccessNoRandom()
        {
            var indexArray = new int[_testArrayAccessSize];

            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                indexArray[i] = i;
            }


            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access No Random = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static unsafe void TestArrayRandomAccessUnsafeNoRandom()
        {
            var indexArray = new int[_testArrayAccessSize];

            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                indexArray[i] = i;
            }

            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;


            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pIndexArray = indexArray)
                fixed (int* pSourceArray = sourceArray)
                fixed (int* pDestinationArray = destinationArray)
                {
                    for (int i = 0; i < _testArrayAccessSize; i++)
                    {
                        int index = pIndexArray[i];
                        pDestinationArray[index] = pSourceArray[index];
                    }
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access Unsafe No Random = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static unsafe void TestArrayRandomAccessUnsafePointerArythmeticNoRandom()
        {
            var indexArray = new int[_testArrayAccessSize];

            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                indexArray[i] = i;
            }

            var sourceArray = new int[_testArrayAccessSize];
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new int[_testArrayAccessSize];
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                fixed (int* pIndexArrayFixed = indexArray)
                fixed (int* pSourceArrayFixed = sourceArray)
                fixed (int* pDestinationArrayFixed = destinationArray)
                {
                    int* pIndexArray = pIndexArrayFixed;
                    int* pSourceArray = pSourceArrayFixed;
                    int* pDestinationArray = pDestinationArrayFixed;
                    
                    int* pEndIndex = pIndexArray + _testArrayAccessSize;
                    for (; pIndexArray < pEndIndex; pIndexArray++)
                    {
                        int index = *pIndexArray;
                        *(pDestinationArray + index) = *(pSourceArray + index);
                    }
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("Array Random Access Unsafe Pointers No Random = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        static void TestListRandomAccessNoRandom()
        {
            var indexArray = new List<int>(new int[_testArrayAccessSize]);

            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                indexArray[i] = i;
            }

            var sourceArray = new List<int>(new int[_testArrayAccessSize]);
            for (int i = 0; i < _testArrayAccessSize; i++)
            {
                sourceArray[i] = i;
            }

            var destinationArray = new List<int>(new int[_testArrayAccessSize]);
            double summTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                for (int i = 0; i < _testArrayAccessSize; i++)
                {
                    int index = indexArray[i];
                    destinationArray[index] = sourceArray[index];
                }

                var time = GetTime();
                summTime += time;
            }

            var avgTime = summTime / _testRepeatCount;
            WriteString("List Random Access No Random = ");
            WriteDouble(avgTime);
            WriteString(" ms\r\n");
        }

        // ----------------------------------------------------------------------------

        class TestInlineMethodsClass
        {
            int Method(int param)
            {
                return param;
            }


            public void test()
            {
                // прогреваем метод
                Method(0);

                double summTime = 0;
                int summResult = 0;
                for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
                {
                    Start();

                    //--------------------------------------------------
                    for (int i = 0; i < _testArrayAccessSize; i++)
                    {
                        summResult = Method(i);
                    }
                    //--------------------------------------------------

                    var time = GetTime();
                    summTime += time;
                }
                var avgTime = summTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new EmptyClass[_testClassAllocationSize];

                Start();

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgAllocationTime = summAllocationTime / _testRepeatCount;
            WriteString("New Class Test = ");
            WriteDouble(avgAllocationTime);
            WriteString(" ms\r\n");

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new OneRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new OneRefClass();
                }

                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    long refIndex = (i + 1) % _testClassAllocationSize;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FiveRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new FiveRefClass();
                }

                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    long refIndex = (i + 1) % _testClassAllocationSize;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % _testClassAllocationSize;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % _testClassAllocationSize;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % _testClassAllocationSize;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % _testClassAllocationSize;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 5);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TenRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new TenRefClass();
                }

                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    long refIndex = (i + 1) % _testClassAllocationSize;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % _testClassAllocationSize;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % _testClassAllocationSize;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % _testClassAllocationSize;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % _testClassAllocationSize;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % _testClassAllocationSize;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % _testClassAllocationSize;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % _testClassAllocationSize;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % _testClassAllocationSize;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % _testClassAllocationSize;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 10);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FifteenRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new FifteenRefClass();
                }

                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    long refIndex = (i + 1) % _testClassAllocationSize;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % _testClassAllocationSize;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % _testClassAllocationSize;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % _testClassAllocationSize;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % _testClassAllocationSize;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % _testClassAllocationSize;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % _testClassAllocationSize;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % _testClassAllocationSize;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % _testClassAllocationSize;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % _testClassAllocationSize;
                    array[i].Ref10 = array[refIndex];
                    refIndex = (i + 11) % _testClassAllocationSize;
                    array[i].Ref11 = array[refIndex];
                    refIndex = (i + 12) % _testClassAllocationSize;
                    array[i].Ref12 = array[refIndex];
                    refIndex = (i + 13) % _testClassAllocationSize;
                    array[i].Ref13 = array[refIndex];
                    refIndex = (i + 14) % _testClassAllocationSize;
                    array[i].Ref14 = array[refIndex];
                    refIndex = (i + 15) % _testClassAllocationSize;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 15);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
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

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TwentyRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new TwentyRefClass();
                }

                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    long refIndex = (i + 1) % _testClassAllocationSize;
                    array[i].Ref1 = array[refIndex];
                    refIndex = (i + 2) % _testClassAllocationSize;
                    array[i].Ref2 = array[refIndex];
                    refIndex = (i + 3) % _testClassAllocationSize;
                    array[i].Ref3 = array[refIndex];
                    refIndex = (i + 4) % _testClassAllocationSize;
                    array[i].Ref4 = array[refIndex];
                    refIndex = (i + 5) % _testClassAllocationSize;
                    array[i].Ref5 = array[refIndex];
                    refIndex = (i + 6) % _testClassAllocationSize;
                    array[i].Ref6 = array[refIndex];
                    refIndex = (i + 7) % _testClassAllocationSize;
                    array[i].Ref7 = array[refIndex];
                    refIndex = (i + 8) % _testClassAllocationSize;
                    array[i].Ref8 = array[refIndex];
                    refIndex = (i + 9) % _testClassAllocationSize;
                    array[i].Ref9 = array[refIndex];
                    refIndex = (i + 10) % _testClassAllocationSize;
                    array[i].Ref10 = array[refIndex];

                    refIndex = (i + 11) % _testClassAllocationSize;
                    array[i].Ref11 = array[refIndex];
                    refIndex = (i + 12) % _testClassAllocationSize;
                    array[i].Ref12 = array[refIndex];
                    refIndex = (i + 13) % _testClassAllocationSize;
                    array[i].Ref13 = array[refIndex];
                    refIndex = (i + 14) % _testClassAllocationSize;
                    array[i].Ref14 = array[refIndex];
                    refIndex = (i + 15) % _testClassAllocationSize;
                    array[i].Ref15 = array[refIndex];
                    refIndex = (i + 16) % _testClassAllocationSize;
                    array[i].Ref16 = array[refIndex];
                    refIndex = (i + 17) % _testClassAllocationSize;
                    array[i].Ref17 = array[refIndex];
                    refIndex = (i + 18) % _testClassAllocationSize;
                    array[i].Ref18 = array[refIndex];
                    refIndex = (i + 19) % _testClassAllocationSize;
                    array[i].Ref19 = array[refIndex];
                    refIndex = (i + 20) % _testClassAllocationSize;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 20);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete Twenty Ref Class Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion










        #region -----------------------   One Ref classs NoRecursionPtr ----------------------------------

        static void TestOneRefClassMemoryAllocationNoRecursionPtr()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthOneRefObject(null);

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new OneRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new OneRefClass();
                }

                var sharedObj = new OneRefClass();
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i].Ref1 = sharedObj;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete One Ref Class NoRecursionPtr Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion

        #region -----------------------   Five Ref classs NoRecursionPtr ----------------------------------

        static void TestFiveRefClassMemoryAllocationNoRecursionPtr()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthFiveRefObject(null);

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FiveRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new FiveRefClass();
                }

                var sharedObj = new FiveRefClass();
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i].Ref1 = sharedObj;
                    array[i].Ref2 = sharedObj;
                    array[i].Ref3 = sharedObj;
                    array[i].Ref4 = sharedObj;
                    array[i].Ref5 = sharedObj;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 5);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete Five Ref Class NoRecursionPtr Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Ten Ref classs NoRecursionPtr ----------------------------------


        static void TestTenRefClassMemoryAllocationNoRecursionPtr()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            DoSomethingWidthTenRefObject(null);

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TenRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new TenRefClass();
                }

                var sharedObj = new TenRefClass();
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i].Ref1 = sharedObj;
                    array[i].Ref2 = sharedObj;
                    array[i].Ref3 = sharedObj;
                    array[i].Ref4 = sharedObj;
                    array[i].Ref5 = sharedObj;
                    array[i].Ref6 = sharedObj;
                    array[i].Ref7 = sharedObj;
                    array[i].Ref8 = sharedObj;
                    array[i].Ref9 = sharedObj;
                    array[i].Ref10 = sharedObj;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 10);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete Ten Ref Class NoRecursionPtr Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Fifteen Ref classs NoRecursionPtr ----------------------------------

        static void TestFifteenRefClassMemoryAllocationNoRecursionPtr()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            // прогреваем метод
            DoSomethingWidthFifteenRefObject(null);

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new FifteenRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new FifteenRefClass();
                }

                var sharedObj = new FifteenRefClass();
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i].Ref1 = sharedObj;
                    array[i].Ref2 = sharedObj;
                    array[i].Ref3 = sharedObj;
                    array[i].Ref4 = sharedObj;
                    array[i].Ref5 = sharedObj;
                    array[i].Ref6 = sharedObj;
                    array[i].Ref7 = sharedObj;
                    array[i].Ref8 = sharedObj;
                    array[i].Ref9 = sharedObj;
                    array[i].Ref10 = sharedObj;
                    array[i].Ref11 = sharedObj;
                    array[i].Ref12 = sharedObj;
                    array[i].Ref13 = sharedObj;
                    array[i].Ref14 = sharedObj;
                    array[i].Ref15 = sharedObj;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 15);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete Fifteen Ref Class NoRecursionPtr Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion


        #region -----------------------   Twenty Ref classs NoRecursionPtr ----------------------------------

        static void TestTwentyRefClassMemoryAllocationNoRecursionPtr()
        {
            // --------------------- New Operator Test ---------------------------------
            double summDeleteTime = 0;

            DoSomethingWidthTwentyRefObject(null);

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                var array = new TwentyRefClass[_testClassAllocationSize];

                //--------------------------------------------------
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i] = new TwentyRefClass();
                }

                var sharedObj = new TwentyRefClass();
                for (int i = 0; i < _testClassAllocationSize; i++)
                {
                    array[i].Ref1 = sharedObj;
                    array[i].Ref2 = sharedObj;
                    array[i].Ref3 = sharedObj;
                    array[i].Ref4 = sharedObj;
                    array[i].Ref5 = sharedObj;
                    array[i].Ref6 = sharedObj;
                    array[i].Ref7 = sharedObj;
                    array[i].Ref8 = sharedObj;
                    array[i].Ref9 = sharedObj;
                    array[i].Ref10 = sharedObj;

                    array[i].Ref11 = sharedObj;
                    array[i].Ref12 = sharedObj;
                    array[i].Ref13 = sharedObj;
                    array[i].Ref14 = sharedObj;
                    array[i].Ref15 = sharedObj;
                    array[i].Ref16 = sharedObj;
                    array[i].Ref17 = sharedObj;
                    array[i].Ref18 = sharedObj;
                    array[i].Ref19 = sharedObj;
                    array[i].Ref20 = sharedObj;
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
                var mustCollectBytes = (double)_testClassAllocationSize * (16 + 8 + 8 * 20);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgDeleteTime = summDeleteTime / _testRepeatCount;
            WriteString("Delete Twenty Ref Class NoRecursionPtr Test = ");
            WriteDouble(avgDeleteTime);
            WriteString(" ms\r\n");
        }

        #endregion



        



        public static void DoSomethingWithArray(int[] array) { }

        static void TestArraysMemoryAllocation()
        {
            int subArraySize = 1;

            int arrayCount = 1000000000;

            for (int onderNumber = 0; onderNumber < 6; onderNumber++)
            {
                subArraySize *= 10;

                arrayCount /= 10;

                double summAllocationTime = 0;
                double summDeleteTime = 0;
                for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
                {

                    // --------------------- New Operator Test ---------------------------------
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    var array = new int[arrayCount][];

                    Start();

                    //--------------------------------------------------
                    for (int i = 0; i < arrayCount; i++)
                    {
                        array[i] = new int[subArraySize];
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
                    var mustCollectBytes = (double)arrayCount * (16 + 8 + subArraySize * 4);
                    if (collected < mustCollectBytes)
                        Console.WriteLine("!!! GC.Collect Wrong");

                    Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                    Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
                }

                WriteString("------------------------------ \r\n");
                WriteString("Array Count = ");
                WriteDouble(arrayCount);
                WriteString("\r\n");

                var avgAllocationTime = summAllocationTime / _testRepeatCount;
                WriteDouble(subArraySize);
                WriteString(" Size New Array Test = ");
                WriteDouble(avgAllocationTime);
                WriteString(" ms\r\n");

                var avgDeleteTime = summDeleteTime / _testRepeatCount;
                WriteDouble(subArraySize);
                WriteString(" Size Delete Array Test = ");
                WriteDouble(avgDeleteTime);
                WriteString(" ms\r\n");
            }
        }


        static void TestArraysMemoryAllocationMT(int threadCount)
        {
            int subArraySize = 1;

            int arrayCount = 100000000;

            for (int onderNumber = 0; onderNumber < 6; onderNumber++)
            {
                subArraySize *= 10;

                arrayCount /= 10;

                if (100000 == subArraySize)
                    arrayCount *= 10;

                double summAllocationTime = 0;
                double summDeleteTime = 0;
                for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
                {

                    // --------------------- New Operator Test ---------------------------------
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                    var array = new int[arrayCount][];
                    Console.WriteLine("Allocation start");

                    Start();

                    var threads = new Thread[threadCount];
                    for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                    {
                        int threadIndexCopy = threadIndex;
                        threads[threadIndex] = new Thread(() =>
                        {
                            long beginIndexLong = (long)arrayCount * threadIndexCopy / threadCount;
                            long endIndexLong = (long)arrayCount * (threadIndexCopy + 1) / threadCount;

                            int beginIndex = (int)beginIndexLong;
                            int endIndex = (int)endIndexLong;

                            for (int i = beginIndex
                                 ; i < endIndex
                                 ; i++)
                            {
                                array[i] = new int[subArraySize];
                            }
                        });
                    }

                    for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                    {
                        threads[threadIndex].Start();
                    }

                    for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                    {
                        threads[threadIndex].Join();
                    }

                    Console.WriteLine("Allocation end");
      
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
                    var mustCollectBytes = (double)arrayCount * (16 + 8 + subArraySize * 4);
                    if (collected < mustCollectBytes)
                        Console.WriteLine("!!! GC.Collect Wrong");

                    Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                    Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
                }

                WriteString("------------------------------ \r\n");
                WriteString("Array Count = ");
                WriteDouble(arrayCount);
                WriteString("\r\n");

                var avgAllocationTime = summAllocationTime / _testRepeatCount;
                WriteDouble(subArraySize);
                WriteString(" Size New Array Test MT = ");
                WriteDouble(avgAllocationTime);
                WriteString(" ms\r\n");

                var avgDeleteTime = summDeleteTime / _testRepeatCount;
                WriteDouble(subArraySize);
                WriteString(" Size Delete Array Test MT = ");
                WriteDouble(avgDeleteTime);
                WriteString(" ms\r\n");
            }
        }




        static void TestEmptyClassMemoryAllocationMT()
        {
            // --------------------- New Operator Test ---------------------------------

            double summAllocTime = 0;
            double summDeleteTime = 0;

            for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
            {
                Start();

                var array = new EmptyClass[_testClassAllocationSize];

                const int threadCount = 4;
                var threads = new Thread[threadCount];
                for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                {
                    int threadIndexCopy = threadIndex;
                    threads[threadIndex] = new Thread(() =>
                    {
                        long beginIndexLong = (long)_testClassAllocationSize * threadIndexCopy / threadCount;
                        long endIndexLong = (long)_testClassAllocationSize * (threadIndexCopy + 1) / threadCount;

                        int beginIndex = (int)beginIndexLong;
                        int endIndex = (int)endIndexLong;

                        for (int i = beginIndex
                             ; i < endIndex
                             ; i++)
                        {
                            array[i] = new EmptyClass();
                        }
                    });
                }


                for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                {
                    threads[threadIndex].Start();
                }

                for (int threadIndex = 0; threadIndex < threadCount; threadIndex++)
                {
                    threads[threadIndex].Join();
                }

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
                var mustCollectBytes = _testClassAllocationSize * (16 + 8);
                if (collected < mustCollectBytes)
                    Console.WriteLine("!!! GC.Collect Wrong");

                Console.WriteLine("Collected Difference = {0}", collected - mustCollectBytes);
                Console.WriteLine("Collected Proportion = {0}", collected / mustCollectBytes);
            }

            var avgTime = summAllocTime / _testRepeatCount;

            WriteString("New Class Test MT = ");

            WriteDouble(avgTime);

            WriteString(" ms\r\n");

            avgTime = summDeleteTime / _testRepeatCount;

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

            //Console.WriteLine("---------------------");
            //file_.Flush();


            //TestArrayRandomAccess();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestArrayRandomAccessUnsafe();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestArrayRandomAccessUnsafePointerArythmetic();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestListRandomAccess();
            //Console.WriteLine("---------------------");
            //file_.Flush();


            //TestArrayRandomAccessNoRandom();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestArrayRandomAccessUnsafeNoRandom();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestArrayRandomAccessUnsafePointerArythmeticNoRandom();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestListRandomAccessNoRandom();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            //TestEmptyClassMemoryAllocationMT();
            //Console.WriteLine("---------------------");
            //file_.Flush();

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



            //// -------------- No Recursion Ptr Methods Calling -----------------------

            //TestOneRefClassMemoryAllocationNoRecursionPtr();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestFiveRefClassMemoryAllocationNoRecursionPtr();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestTenRefClassMemoryAllocationNoRecursionPtr();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestFifteenRefClassMemoryAllocationNoRecursionPtr();
            //Console.WriteLine("---------------------");
            //file_.Flush();
            //TestTwentyRefClassMemoryAllocationNoRecursionPtr();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            //TestArraysMemoryAllocation();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            WriteString(_threadCount + " threads:\r\n");

            TestArraysMemoryAllocationMT(_threadCount);
            Console.WriteLine("---------------------");
            file_.Flush();

            WriteString("single thread:\r\n");

            TestArraysMemoryAllocationMT(1);
            Console.WriteLine("---------------------");
            file_.Flush();


            //TestEmptyClassMemoryAllocationMT();
            //Console.WriteLine("---------------------");
            //file_.Flush();

            Console.WriteLine("--- Complete ---");

            Console.WriteLine("Total Time = {0}", totalStopwatch.Elapsed.ToString());

            file_.Close();

            //Console.ReadLine();
        }
    }
}
    