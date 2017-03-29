#include "stdafx.h"

//---------------- Infrastructure ----------------------

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

//---------------------------- Benchmarks ---------------------------------------

void TestArrayAccess()
{
	const int arraySize_ = 10000000;
	int* array = new int[arraySize_];


	Start();
	for (int i = 0; i < arraySize_; i++)
	{
		array[i] = i;
	}

	auto time = GetTime();
	WriteString("Write access time = ");
	WriteDouble(time);
	WriteString("\r\n");
	
	delete[] array;
}

int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);

	TestArrayAccess();


	getchar();
    return 0;
}

