// test_cpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


_LARGE_INTEGER startCounter_;

std::ofstream fileOut_;

void Start()
{
	QueryPerformanceCounter(&startCounter_);
}
//---------------------------------------------------------------------------
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

void writeString(const char* outString)
{
	std::cout << outString;
	fileOut_ << outString;
}

void writeDouble(double value)
{
	std::cout << value;
	fileOut_ << value;
}

int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);

	writeString("Hello");
	writeDouble(3.142);


	getchar();
    return 0;
}

