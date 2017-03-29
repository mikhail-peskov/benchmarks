#include "stdafx.h"



//---------------- Infrastructure ----------------------

const int testRepeatCount = 100;

const int arraySize_ = 100000000;

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
	int* array = new int[arraySize_];

	Test("Array Fill Lambda", [array]()
	{
		for (int i = 0; i < arraySize_; i++)
		{
			array[i] = i;
		}
	});
	
	int* destinationArray = new int[arraySize_];

	Test("Array Copy Lambda", [array, destinationArray]()
	{
		for (int i = 0; i < arraySize_; i++)
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
	std::vector<int> vector(arraySize_);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < arraySize_; i++)
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


	std::vector<int> destinationVector(arraySize_);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < arraySize_; i++)
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
	std::vector<int> vector(arraySize_);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int forwardIndex = 0, backwardIndex = arraySize_ - 1; forwardIndex < arraySize_ / 2; forwardIndex++, backwardIndex--)
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


	std::vector<int> destinationVector(arraySize_);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int forwardIndex = 0, backwardIndex = arraySize_ - 1; forwardIndex < arraySize_ / 2; forwardIndex++, backwardIndex--)
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
			for (int i = 0; i < arraySize_; i++)
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
			for (int i = 0; i < arraySize_; i++)
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

int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);

	TestInlineMethodsClass testInline;
	testInline.test();

	TestNotInlineMethodsClass testNoInline;
	testNoInline.test();

	TestArrayAccessLambda();
	TestArrayAccess();
	TestVectorAccess();
	TestVectorRandomAccess();



	getchar();
    return 0;
}

