#include "stdafx.h"



//---------------- Infrastructure ----------------------

const int testRepeatCount = 1;

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


void TestArrayAccess()
{
	const int arraySize = 100000000;
	int* array = new int[arraySize];

	Test("Array Fill", [array, arraySize]()
	{
		for (int i = 0; i < arraySize; i++)
		{
			array[i] = i;
		}
	});
	
	int* destinationArray = new int[arraySize];

	Test("Array Copy", [array, destinationArray, arraySize]()
	{
		for (int i = 0; i < arraySize; i++)
		{
			destinationArray[i] = array[i];
		}

	});

	delete[] array;
	delete[] destinationArray;
}

void TestVectorAccess()
{
	const int arraySize = 100000000;

	std::vector<int> vector(arraySize);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < arraySize; i++)
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


	std::vector<int> destinationVector(arraySize);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < arraySize; i++)
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


int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);

	TestVectorAccess();
	TestArrayAccess();



	getchar();
    return 0;
}

