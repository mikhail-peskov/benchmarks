#include "stdafx.h"

//---------------- Infrastructure ----------------------

const int testRepeatCount = 1;

const int testAccessArraySize_ = 100000000;
const int testAllocationClassSize_ = 100000000;
const int testAllocationArraySize_ = 1000000;

_LARGE_INTEGER startCounter_;

std::ofstream fileOut_;

void Start()
{
	QueryPerformanceCounter(&startCounter_);
}

double GetTime()
{
	_LARGE_INTEGER NowCounter;
	QueryPerformanceCounter(&NowCounter);
	__int64 CountS = startCounter_.QuadPart;
	__int64 CountE = NowCounter.QuadPart;

	_LARGE_INTEGER FreqL;

	QueryPerformanceFrequency(&FreqL);
	__int64 Freq = FreqL.QuadPart;

	return ((double)CountE - (double)CountS) / (double)Freq;
}

void WriteString(const char* outString)
{
	std::cout << outString;
	fileOut_ << outString;
}

void WriteDouble(double value)
{
	std::cout << value;
	fileOut_ << value;
}

void Test(const char* testName, std::function<void()> function, int avgIterationCount = testRepeatCount)
{
	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < avgIterationCount; iterationIndes++)
	{
		Start();

		function();

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / avgIterationCount;

	WriteString(testName);
	WriteString(" = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");
}

//---------------------------- Benchmarks ---------------------------------------


void TestArrayAccessLambda()
{
	int* array = new int[testAccessArraySize_];

	Test("Array Fill Lambda", [array]()
	{
		for (int i = 0; i < testAccessArraySize_; i++)
		{
			array[i] = i;
		}
	});
	
	int* destinationArray = new int[testAccessArraySize_];

	Test("Array Copy Lambda", [array, destinationArray]()
	{
		for (int i = 0; i < testAccessArraySize_; i++)
		{
			destinationArray[i] = array[i];
		}

	});

	delete[] array;
	delete[] destinationArray;
}

void TestArrayAccess()
{
	const int arraySize_ = 100000000;

	int* array = new int[arraySize_];


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

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("Array Fill = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ------------------- Copy -----------------------------------------------


	int* destinationArray = new int[arraySize_];
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

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Array Copy = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	delete[] array;
	delete[] destinationArray;
}

void TestVectorAccess()
{
	std::vector<int> vector(testAccessArraySize_);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAccessArraySize_; i++)
		{
			vector[i] = i;
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("Vector Fill = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ------------------- Copy -----------------------------------------------


	std::vector<int> destinationVector(testAccessArraySize_);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAccessArraySize_; i++)
		{
			destinationVector[i] = vector[i];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Vector Copy = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");
}


void TestVectorRandomAccess()
{
	std::vector<int> vector(testAccessArraySize_);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int forwardIndex = 0, backwardIndex = testAccessArraySize_ - 1; forwardIndex < testAccessArraySize_ / 2; forwardIndex++, backwardIndex--)
		{
			vector[forwardIndex] = forwardIndex;
			vector[backwardIndex] = forwardIndex;
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("Rand. Access Vector Fill = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ------------------- Copy -----------------------------------------------


	std::vector<int> destinationVector(testAccessArraySize_);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int forwardIndex = 0, backwardIndex = testAccessArraySize_ - 1; forwardIndex < testAccessArraySize_ / 2; forwardIndex++, backwardIndex--)
		{
			destinationVector[forwardIndex] = vector[backwardIndex];
			destinationVector[backwardIndex] = vector[forwardIndex];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Rand. Access Vector Copy = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");
}


class TestInlineMethodsClass
{
	int InlineMethod(const int param1) const
	{
		return param1;
	}

public:
	void test()
	{
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

			auto time = GetTime();
			time *= 1000.0;
			summTime += time;
		}
		auto avgTime = summTime / testRepeatCount;
		WriteString("Inline Method = ");
		WriteDouble(avgTime);
		WriteString("ms\r\n");
		WriteString("Inline result = ");
		WriteDouble(summResult);
		WriteString("\r\n");

		// ------------------- Copy -----------------------------------------------		
	}
};


class TestNotInlineMethodsClass
{
	int noInlineMethod(const int param1) const;
	
public:
	void test()
	{
		double summTime = 0;
		int summResult = 0;
		for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
		{
			Start();

			//--------------------------------------------------
			for (int i = 0; i < testAccessArraySize_; i++)
			{
				summResult = noInlineMethod(i);
			}
			//--------------------------------------------------

			auto time = GetTime();
			time *= 1000.0;
			summTime += time;
		}
		auto avgTime = summTime / testRepeatCount;
		WriteString("No Inline Method = ");
		WriteDouble(avgTime);
		WriteString("ms\r\n");
		WriteString("No Inline result = ");
		WriteDouble(summResult);
		WriteString("\r\n");

		// ------------------- Copy -----------------------------------------------		
	}
};

int TestNotInlineMethodsClass::noInlineMethod(const int param1) const
{
	return param1;
}

class EmptyClass
{
};

void TestClassMemoryAllocation()
{
	auto array = new EmptyClass*[testAllocationClassSize_];
			
	// --------------------- New Operator Test ---------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationClassSize_; i++)
		{
			array[i] = new EmptyClass();
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("New Class Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");
	
	// ---------------------- Delete Operator Test ------------------------


	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationClassSize_; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Delete Class Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");


	delete[] array;
}

void TestArraysMemoryAllocation()
{
	auto array = new int*[testAccessArraySize_];

	// --------------------- New Operator Test ---------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationArraySize_; i++)
		{
			array[i] = new int[100];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("New Array Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ---------------------- Delete Operator Test ------------------------


	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationArraySize_; i++)
		{
			delete[] array[i];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Delete Array Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");


	delete[] array;
}


void TestVectorMemoryAllocation()
{
	auto array = new std::vector<int>*[testAccessArraySize_];

	// --------------------- New Operator Test ---------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationArraySize_; i++)
		{
			array[i] = new std::vector<int>(100);
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("New Vector Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ---------------------- Delete Operator Test ------------------------


	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < testAllocationArraySize_; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Delete Vector Test = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");


	delete[] array;
}

void TestClassMemoryAllocationMT()
{
	auto array = new EmptyClass*[testAllocationClassSize_];

	// --------------------- New Operator Test ---------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		std::thread thread1([array]()
		{
			for (int i = 0; i < testAllocationClassSize_ / 4; i++)
			{
				array[i] = new EmptyClass();
			}
		});

		std::thread thread2([array]()
		{
			for (int i = testAllocationClassSize_ / 4; i < testAllocationClassSize_ / 2; i++)
			{
				array[i] = new EmptyClass();
			}
		});


		std::thread thread3([array]()
		{
			for (int i = testAllocationClassSize_ / 2; i < testAllocationClassSize_ * 3 / 4; i++)
			{
				array[i] = new EmptyClass();
			}
		});

		std::thread thread4([array]()
		{
			for (int i = testAllocationClassSize_ * 3 / 4; i < testAllocationClassSize_; i++)
			{
				array[i] = new EmptyClass();
			}
		});

		thread1.join();
		thread2.join();
		thread3.join();
		thread4.join();


		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	auto avgTime = summTime / testRepeatCount;
	WriteString("New Class Test MT = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ---------------------- Delete Operator Test ------------------------


	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		std::thread thread1([array]()
		{
			for (int i = 0; i < testAllocationClassSize_ / 4; i++)
			{
				delete array[i];
			}
		});

		std::thread thread2([array]()
		{
			for (int i = testAllocationClassSize_ / 4; i < testAllocationClassSize_ / 2; i++)
			{
				delete array[i];
			}
		});


		std::thread thread3([array]()
		{
			for (int i = testAllocationClassSize_ / 2; i < testAllocationClassSize_ * 3 / 4; i++)
			{
				delete array[i];
			}
		});

		std::thread thread4([array]()
		{
			for (int i = testAllocationClassSize_ * 3 / 4; i < testAllocationClassSize_; i++)
			{
				delete array[i];
			}
		});

		thread1.join();
		thread2.join();
		thread3.join();
		thread4.join();


		auto time = GetTime();
		time *= 1000.0;
		summTime += time;
	}
	avgTime = summTime / testRepeatCount;
	WriteString("Delete Class Test MT = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");


	delete[] array;
}

int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);

	//TestInlineMethodsClass testInline;
	//testInline.test();

	//TestNotInlineMethodsClass testNoInline;
	//testNoInline.test();

	//TestArrayAccessLambda();
	//TestArrayAccess();
	//TestVectorAccess();
	//TestVectorRandomAccess();

	TestClassMemoryAllocation();
	//TestArraysMemoryAllocation();
	//TestVectorMemoryAllocation();

	TestClassMemoryAllocationMT();

	getchar();
    return 0;
}

