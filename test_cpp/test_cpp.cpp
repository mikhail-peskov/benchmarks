#include "stdafx.h"

using namespace std;

//---------------- Infrastructure ----------------------

const int _threadCount = 8;

const int _testRepeatCount = 100;

const int _testArrayAccessSize = 100000000;
const int _testClassAllocationSize = 10000000;

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

	return (((double)CountE - (double)CountS) / (double)Freq) * 1000;
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

void Test(const char* testName, std::function<void()> function, int avgIterationCount = _testRepeatCount)
{
	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < avgIterationCount; iterationIndes++)
	{
		Start();

		function();

		auto time = GetTime();
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
	int* array = new int[_testArrayAccessSize];


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

		auto time = GetTime();
		summTime += time;
	}
	auto avgTime = summTime / _testRepeatCount;
	WriteString("Array Fill = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ------------------- Copy -----------------------------------------------


	int* destinationArray = new int[_testArrayAccessSize];
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

		auto time = GetTime();
		summTime += time;
	}
	avgTime = summTime / _testRepeatCount;
	WriteString("Array Copy = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	delete[] array;
	delete[] destinationArray;
}

void TestVectorAccess()
{
	std::vector<int> vector(_testArrayAccessSize);


	// ------------------- Fill -----------------------------------------------

	double summTime = 0;
	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			vector[i] = i;
		}
		//--------------------------------------------------

		auto time = GetTime();
		summTime += time;
	}
	auto avgTime = summTime / _testRepeatCount;
	WriteString("Vector Fill = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	// ------------------- Copy -----------------------------------------------


	std::vector<int> destinationVector(_testArrayAccessSize);
	summTime = 0;
	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			destinationVector[i] = vector[i];
		}
		//--------------------------------------------------

		auto time = GetTime();
		summTime += time;
	}
	avgTime = summTime / _testRepeatCount;
	WriteString("Vector Copy = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");
}

void TestArrayRandomAccess()
{
	int* indexArray = new int[_testArrayAccessSize];
	int* sourceArray = new int[_testArrayAccessSize];
	int* destinationArray = new int[_testArrayAccessSize];

	//srand(time(NULL));
	//for (int i = 0; i < _testArrayAccessSize; i++) {
	//	indexArray[i] = rand() % _testArrayAccessSize;
	//}

	ifstream randNumbers("random_values.txt");
	for (int i = 0; i < _testArrayAccessSize; i++) {
		int nextValue;
		randNumbers >> nextValue;
		indexArray[i] = nextValue;
	}
	randNumbers.close();
	cout << "file with random data loaded\r\n";

	for (int i = 0; i < _testArrayAccessSize; i++) {
		sourceArray[i] = i;
	}

	double summTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			int index = indexArray[i];
			destinationArray[index] = sourceArray[index];
		}

		auto time = GetTime();
		summTime += time;
	}

	auto avgTime = summTime / _testRepeatCount;
	WriteString("Test Random Access = ");
	WriteDouble(avgTime);
	WriteString(" ms\r\n");

	delete[] indexArray;
	delete[] sourceArray;
	delete[] destinationArray;
}

void TestVectorRandomAccess()
{
	std::vector<int> indexArray(_testArrayAccessSize);
	std::vector<int> sourceArray(_testArrayAccessSize);
	std::vector<int> destinationArray(_testArrayAccessSize);

	srand(time(NULL));

	//for (int i = 0; i < _testArrayAccessSize; i++) {
	//	indexArray[i] = rand() % _testArrayAccessSize;
	//}



	ifstream randNumbers("random_values.txt");
	for (int i = 0; i < _testArrayAccessSize; i++) {
		int nextValue;
		randNumbers >> nextValue;
		indexArray[i] = nextValue;
	}
	randNumbers.close();
	cout << "file with random data loaded\r\n";




	for (int i = 0; i < _testArrayAccessSize; i++) {
		sourceArray[i] = i;
	}

	double summTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			int index = indexArray[i];
			destinationArray[index] = sourceArray[index];
		}

		auto time = GetTime();
		summTime += time;
	}

	auto avgTime = summTime / _testRepeatCount;
	WriteString("Test Random Access Vector = ");
	WriteDouble(avgTime);
	WriteString(" ms\r\n");
}

void TestArrayRandomAccessNoRandom()
{
	int* indexArray = new int[_testArrayAccessSize];
	int* sourceArray = new int[_testArrayAccessSize];
	int* destinationArray = new int[_testArrayAccessSize];

	for (int i = 0; i < _testArrayAccessSize; i++) {
		indexArray[i] = i;
	}
	
	for (int i = 0; i < _testArrayAccessSize; i++) {
		sourceArray[i] = i;
	}

	double summTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			int index = indexArray[i];
			destinationArray[index] = sourceArray[index];
		}

		auto time = GetTime();
		summTime += time;
	}

	auto avgTime = summTime / _testRepeatCount;
	WriteString("Test Random Access No Random = ");
	WriteDouble(avgTime);
	WriteString(" ms\r\n");

	delete[] indexArray;
	delete[] sourceArray;
	delete[] destinationArray;
}

void TestVectorRandomAccessNoRandom()
{
	std::vector<int> indexArray(_testArrayAccessSize);
	std::vector<int> sourceArray(_testArrayAccessSize);
	std::vector<int> destinationArray(_testArrayAccessSize);

	for (int i = 0; i < _testArrayAccessSize; i++) {
		indexArray[i] = i;
	}
	
	for (int i = 0; i < _testArrayAccessSize; i++) {
		sourceArray[i] = i;
	}

	double summTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		for (int i = 0; i < _testArrayAccessSize; i++)
		{
			int index = indexArray[i];
			destinationArray[index] = sourceArray[index];
		}

		auto time = GetTime();
		summTime += time;
	}

	auto avgTime = summTime / _testRepeatCount;
	WriteString("Test Random Access Vector No Random = ");
	WriteDouble(avgTime);
	WriteString(" ms\r\n");
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
		for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
		{
			Start();

			//--------------------------------------------------
			for (int i = 0; i < _testArrayAccessSize; i++)
			{
				summResult = InlineMethod(i);
			}
			//--------------------------------------------------

			auto time = GetTime();
			summTime += time;
		}
		auto avgTime = summTime / _testRepeatCount;
		WriteString("Inline Method = ");
		WriteDouble(avgTime);
		WriteString("ms\r\n");
		WriteString("Inline result = ");
		WriteDouble(summResult);
		WriteString("\r\n");

		// ------------------- Copy -----------------------------------------------		
	}
};


class TestNoInlineMethodsClass
{
	int noInlineMethod(const int param1) const;
	
public:
	void test()
	{
		double summTime = 0;
		int summResult = 0;
		for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
		{
			Start();

			//--------------------------------------------------
			for (int i = 0; i < _testArrayAccessSize; i++)
			{
				summResult = noInlineMethod(i);
			}
			//--------------------------------------------------

			auto time = GetTime();
			summTime += time;
		}
		auto avgTime = summTime / _testRepeatCount;
		WriteString("No Inline Method = ");
		WriteDouble(avgTime);
		WriteString("ms\r\n");
		WriteString("No Inline result = ");
		WriteDouble(summResult);
		WriteString("\r\n");

		// ------------------- Copy -----------------------------------------------		
	}
};

int TestNoInlineMethodsClass::noInlineMethod(const int param1) const
{
	return param1;
}

//-----------------------   EmptyClasss ----------------------------------

class EmptyClass
{
};

void TestEmptyClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summAllocationTime = 0;
	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new EmptyClass*[_testClassAllocationSize];

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new EmptyClass();
		}
		//--------------------------------------------------

		auto time = GetTime();
		summAllocationTime += time;
	
	// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		delete[] array;

		time = GetTime();
		summDeleteTime += time;
	}

	auto avgAllocationTime = summAllocationTime / _testRepeatCount;
	WriteString("New Class Test = ");
	WriteDouble(avgAllocationTime);
	WriteString("ms\r\n");

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete Empty Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   One Ref classs ----------------------------------

class OneRefClass
{
public:
	OneRefClass* Ref1;
};

void TestOneRefClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new OneRefClass*[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new OneRefClass();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 1 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Five Ref classs ----------------------------------

class FiveRefClass
{
public:
	FiveRefClass* Ref1;
	FiveRefClass* Ref2;
	FiveRefClass* Ref3;
	FiveRefClass* Ref4;
	FiveRefClass* Ref5;
};

void TestFiveRefClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new FiveRefClass*[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new FiveRefClass();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 5 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}


//-----------------------   Ten Ref classs ----------------------------------

class TenRefClass
{
public:
	TenRefClass* Ref1;
	TenRefClass* Ref2;
	TenRefClass* Ref3;
	TenRefClass* Ref4;
	TenRefClass* Ref5;
	TenRefClass* Ref6;
	TenRefClass* Ref7;
	TenRefClass* Ref8;
	TenRefClass* Ref9;
	TenRefClass* Ref10;
};

void TestTenRefClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new TenRefClass*[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new TenRefClass();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 10 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}


//-----------------------   Fifteen Ref classs ----------------------------------


class FifteenRefClass
{
public:
	FifteenRefClass* Ref1;
	FifteenRefClass* Ref2;
	FifteenRefClass* Ref3;
	FifteenRefClass* Ref4;
	FifteenRefClass* Ref5;
	FifteenRefClass* Ref6;
	FifteenRefClass* Ref7;
	FifteenRefClass* Ref8;
	FifteenRefClass* Ref9;
	FifteenRefClass* Ref10;
	FifteenRefClass* Ref11;
	FifteenRefClass* Ref12;
	FifteenRefClass* Ref13;
	FifteenRefClass* Ref14;
	FifteenRefClass* Ref15;
};

void TestFifteenRefClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new FifteenRefClass*[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new FifteenRefClass();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
			refIndex = (i + 11) % _testClassAllocationSize;
			array[i]->Ref11 = array[refIndex];
			refIndex = (i + 12) % _testClassAllocationSize;
			array[i]->Ref12 = array[refIndex];
			refIndex = (i + 13) % _testClassAllocationSize;
			array[i]->Ref13 = array[refIndex];
			refIndex = (i + 14) % _testClassAllocationSize;
			array[i]->Ref14 = array[refIndex];
			refIndex = (i + 15) % _testClassAllocationSize;
			array[i]->Ref15 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 15 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Twenty Ref classs ----------------------------------

class TwentyRefClass
{
public:
	TwentyRefClass* Ref1;
	TwentyRefClass* Ref2;
	TwentyRefClass* Ref3;
	TwentyRefClass* Ref4;
	TwentyRefClass* Ref5;
	TwentyRefClass* Ref6;
	TwentyRefClass* Ref7;
	TwentyRefClass* Ref8;
	TwentyRefClass* Ref9;
	TwentyRefClass* Ref10;
	TwentyRefClass* Ref11;
	TwentyRefClass* Ref12;
	TwentyRefClass* Ref13;
	TwentyRefClass* Ref14;
	TwentyRefClass* Ref15;
	TwentyRefClass* Ref16;
	TwentyRefClass* Ref17;
	TwentyRefClass* Ref18;
	TwentyRefClass* Ref19;
	TwentyRefClass* Ref20;
};

void TestTwentyRefClassMemoryAllocation()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new TwentyRefClass*[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = new TwentyRefClass();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
			refIndex = (i + 11) % _testClassAllocationSize;
			array[i]->Ref11 = array[refIndex];
			refIndex = (i + 12) % _testClassAllocationSize;
			array[i]->Ref12 = array[refIndex];
			refIndex = (i + 13) % _testClassAllocationSize;
			array[i]->Ref13 = array[refIndex];
			refIndex = (i + 14) % _testClassAllocationSize;
			array[i]->Ref14 = array[refIndex];
			refIndex = (i + 15) % _testClassAllocationSize;
			array[i]->Ref15 = array[refIndex];
			refIndex = (i + 16) % _testClassAllocationSize;
			array[i]->Ref16 = array[refIndex];
			refIndex = (i + 17) % _testClassAllocationSize;
			array[i]->Ref17 = array[refIndex];
			refIndex = (i + 18) % _testClassAllocationSize;
			array[i]->Ref18 = array[refIndex];
			refIndex = (i + 19) % _testClassAllocationSize;
			array[i]->Ref19 = array[refIndex];
			refIndex = (i + 20) % _testClassAllocationSize;
			array[i]->Ref20 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			delete array[i];
		}
		//--------------------------------------------------
		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 20 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}


// ----------------------- SHARED POINTER TEST ----------------------------------------



//-----------------------   EmptyClasss Shared Pointer ----------------------------------

void TestEmptyClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summAllocationTime = 0;
	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<EmptyClass>[_testClassAllocationSize];

		Start();

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<EmptyClass>();
		}
		//--------------------------------------------------

		auto time = GetTime();
		summAllocationTime += time;

		
		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		time = GetTime();
		summDeleteTime += time;
	}

	auto avgAllocationTime = summAllocationTime / _testRepeatCount;
	WriteString("New Class Test Shared Pointers = ");
	WriteDouble(avgAllocationTime);
	WriteString("ms\r\n");

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete Empty Class Shared Pointers Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}




//-----------------------   One Ref classs Shared Pointer ----------------------------------

class OneRefClassSharedPtr
{
public:
	shared_ptr<OneRefClassSharedPtr> Ref1;
};

void TestOneRefClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<OneRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<OneRefClassSharedPtr>();
		}

		auto sharedObj = make_shared<OneRefClassSharedPtr>();
		for (int i = 0; i < _testClassAllocationSize - 1; i++)
		{
			long refIndex = i + 1;
			array[i]->Ref1 = sharedObj;
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 1 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Five Ref classs SharedPtr ----------------------------------

class FiveRefClassSharedPtr
{
public:
	shared_ptr<FiveRefClassSharedPtr> Ref1;
	shared_ptr<FiveRefClassSharedPtr> Ref2;
	shared_ptr<FiveRefClassSharedPtr> Ref3;
	shared_ptr<FiveRefClassSharedPtr> Ref4;
	shared_ptr<FiveRefClassSharedPtr> Ref5;
};

void TestFiveRefClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<FiveRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<FiveRefClassSharedPtr>();
		}

		auto sharedObj = make_shared<FiveRefClassSharedPtr>();
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i]->Ref1 = sharedObj;
			array[i]->Ref2 = sharedObj;
			array[i]->Ref3 = sharedObj;
			array[i]->Ref4 = sharedObj;
			array[i]->Ref5 = sharedObj;
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 5 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Ten Ref classs ----------------------------------

class TenRefClassSharedPtr
{
public:
	shared_ptr<TenRefClassSharedPtr> Ref1;
	shared_ptr<TenRefClassSharedPtr> Ref2;
	shared_ptr<TenRefClassSharedPtr> Ref3;
	shared_ptr<TenRefClassSharedPtr> Ref4;
	shared_ptr<TenRefClassSharedPtr> Ref5;
	shared_ptr<TenRefClassSharedPtr> Ref6;
	shared_ptr<TenRefClassSharedPtr> Ref7;
	shared_ptr<TenRefClassSharedPtr> Ref8;
	shared_ptr<TenRefClassSharedPtr> Ref9;
	shared_ptr<TenRefClassSharedPtr> Ref10;
};

void TestTenRefClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<TenRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<TenRefClassSharedPtr>();
		}

		auto sharedObj = make_shared<TenRefClassSharedPtr>();
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i]->Ref1 = sharedObj;
			array[i]->Ref2 = sharedObj;
			array[i]->Ref3 = sharedObj;
			array[i]->Ref4 = sharedObj;
			array[i]->Ref5 = sharedObj;
			array[i]->Ref6 = sharedObj;
			array[i]->Ref7 = sharedObj;
			array[i]->Ref8 = sharedObj;
			array[i]->Ref9 = sharedObj;
			array[i]->Ref10 =sharedObj;
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 10 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Fifteen Ref classs ----------------------------------

class FifteenRefClassSharedPtr
{
public:
	shared_ptr<FifteenRefClassSharedPtr> Ref1;
	shared_ptr<FifteenRefClassSharedPtr> Ref2;
	shared_ptr<FifteenRefClassSharedPtr> Ref3;
	shared_ptr<FifteenRefClassSharedPtr> Ref4;
	shared_ptr<FifteenRefClassSharedPtr> Ref5;
	shared_ptr<FifteenRefClassSharedPtr> Ref6;
	shared_ptr<FifteenRefClassSharedPtr> Ref7;
	shared_ptr<FifteenRefClassSharedPtr> Ref8;
	shared_ptr<FifteenRefClassSharedPtr> Ref9;
	shared_ptr<FifteenRefClassSharedPtr> Ref10;
	shared_ptr<FifteenRefClassSharedPtr> Ref11;
	shared_ptr<FifteenRefClassSharedPtr> Ref12;
	shared_ptr<FifteenRefClassSharedPtr> Ref13;
	shared_ptr<FifteenRefClassSharedPtr> Ref14;
	shared_ptr<FifteenRefClassSharedPtr> Ref15;
};

void TestFifteenRefClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<FifteenRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<FifteenRefClassSharedPtr>();
		}

		auto sharedObj = make_shared<FifteenRefClassSharedPtr>();
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i]->Ref1 = sharedObj;
			array[i]->Ref2 = sharedObj;
			array[i]->Ref3 = sharedObj;
			array[i]->Ref4 = sharedObj;
			array[i]->Ref5 = sharedObj;
			array[i]->Ref6 = sharedObj;
			array[i]->Ref7 = sharedObj;
			array[i]->Ref8 = sharedObj;
			array[i]->Ref9 = sharedObj;
			array[i]->Ref10 = sharedObj;
			array[i]->Ref11 = sharedObj;
			array[i]->Ref12 = sharedObj;
			array[i]->Ref13 = sharedObj;
			array[i]->Ref14 = sharedObj;
			array[i]->Ref15 = sharedObj;
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 15 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Twenty Ref classs SharedPtr ----------------------------------

class TwentyRefClassSharedPtr
{
public:
	shared_ptr<TwentyRefClassSharedPtr> Ref1;
	shared_ptr<TwentyRefClassSharedPtr> Ref2;
	shared_ptr<TwentyRefClassSharedPtr> Ref3;
	shared_ptr<TwentyRefClassSharedPtr> Ref4;
	shared_ptr<TwentyRefClassSharedPtr> Ref5;
	shared_ptr<TwentyRefClassSharedPtr> Ref6;
	shared_ptr<TwentyRefClassSharedPtr> Ref7;
	shared_ptr<TwentyRefClassSharedPtr> Ref8;
	shared_ptr<TwentyRefClassSharedPtr> Ref9;
	shared_ptr<TwentyRefClassSharedPtr> Ref10;
	shared_ptr<TwentyRefClassSharedPtr> Ref11;
	shared_ptr<TwentyRefClassSharedPtr> Ref12;
	shared_ptr<TwentyRefClassSharedPtr> Ref13;
	shared_ptr<TwentyRefClassSharedPtr> Ref14;
	shared_ptr<TwentyRefClassSharedPtr> Ref15;
	shared_ptr<TwentyRefClassSharedPtr> Ref16;
	shared_ptr<TwentyRefClassSharedPtr> Ref17;
	shared_ptr<TwentyRefClassSharedPtr> Ref18;
	shared_ptr<TwentyRefClassSharedPtr> Ref19;
	shared_ptr<TwentyRefClassSharedPtr> Ref20;
};

void TestTwentyRefClassMemoryAllocationSharedPtr()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<TwentyRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<TwentyRefClassSharedPtr>();
		}

		auto sharedObj = make_shared<TwentyRefClassSharedPtr>();
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i]->Ref1 = sharedObj;
			array[i]->Ref2 = sharedObj;
			array[i]->Ref3 = sharedObj;
			array[i]->Ref4 = sharedObj;
			array[i]->Ref5 = sharedObj;
			array[i]->Ref6 = sharedObj;
			array[i]->Ref7 = sharedObj;
			array[i]->Ref8 = sharedObj;
			array[i]->Ref9 = sharedObj;
			array[i]->Ref10 = sharedObj;
			array[i]->Ref11 = sharedObj;
			array[i]->Ref12 = sharedObj;
			array[i]->Ref13 = sharedObj;
			array[i]->Ref14 = sharedObj;
			array[i]->Ref15 = sharedObj;
			array[i]->Ref16 = sharedObj;
			array[i]->Ref17 = sharedObj;
			array[i]->Ref18 = sharedObj;
			array[i]->Ref19 = sharedObj;
			array[i]->Ref20 = sharedObj;
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 20 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}





//-----------------------   One Ref classs Shared Pointer with stack overflow ----------------------------------

void TestOneRefClassMemoryAllocationSharedPtrStackOverflow()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<OneRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<OneRefClassSharedPtr>();
		}

		for (int i = 0; i < _testClassAllocationSize - 1; i++)
		{
			long refIndex = i + 1;
			array[i]->Ref1 = array[refIndex];
		}
		//--------------------------------------------------
		
		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 1 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Five Ref classs SharedPtr with stack overflow ----------------------------------

void TestFiveRefClassMemoryAllocationSharedPtrStackOverflow()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<FiveRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<FiveRefClassSharedPtr>();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 5 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Ten Ref classs with stack overflow ----------------------------------

void TestTenRefClassMemoryAllocationSharedPtrStackOverflow()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<TenRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<TenRefClassSharedPtr>();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 10 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Fifteen Ref classs with stack overflow ----------------------------------

void TestFifteenRefClassMemoryAllocationSharedPtrStackOverflow()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<FifteenRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<FifteenRefClassSharedPtr>();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
			refIndex = (i + 11) % _testClassAllocationSize;
			array[i]->Ref11 = array[refIndex];
			refIndex = (i + 12) % _testClassAllocationSize;
			array[i]->Ref12 = array[refIndex];
			refIndex = (i + 13) % _testClassAllocationSize;
			array[i]->Ref13 = array[refIndex];
			refIndex = (i + 14) % _testClassAllocationSize;
			array[i]->Ref14 = array[refIndex];
			refIndex = (i + 15) % _testClassAllocationSize;
			array[i]->Ref15 = array[refIndex];
		}
		//--------------------------------------------------

		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;	
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 15 Ref Class SharedPtr Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}

//-----------------------   Twenty Ref classs SharedPtr with stac overflow ----------------------------------

void TestTwentyRefClassMemoryAllocationSharedPtrStackOverflow()
{
	// --------------------- New Operator Test ---------------------------------

	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		auto array = new shared_ptr<TwentyRefClassSharedPtr>[_testClassAllocationSize];

		//--------------------------------------------------
		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			array[i] = make_shared<TwentyRefClassSharedPtr>();
		}

		for (int i = 0; i < _testClassAllocationSize; i++)
		{
			long refIndex = (i + 1) % _testClassAllocationSize;
			array[i]->Ref1 = array[refIndex];
			refIndex = (i + 2) % _testClassAllocationSize;
			array[i]->Ref2 = array[refIndex];
			refIndex = (i + 3) % _testClassAllocationSize;
			array[i]->Ref3 = array[refIndex];
			refIndex = (i + 4) % _testClassAllocationSize;
			array[i]->Ref4 = array[refIndex];
			refIndex = (i + 5) % _testClassAllocationSize;
			array[i]->Ref5 = array[refIndex];
			refIndex = (i + 6) % _testClassAllocationSize;
			array[i]->Ref6 = array[refIndex];
			refIndex = (i + 7) % _testClassAllocationSize;
			array[i]->Ref7 = array[refIndex];
			refIndex = (i + 8) % _testClassAllocationSize;
			array[i]->Ref8 = array[refIndex];
			refIndex = (i + 9) % _testClassAllocationSize;
			array[i]->Ref9 = array[refIndex];
			refIndex = (i + 10) % _testClassAllocationSize;
			array[i]->Ref10 = array[refIndex];
			refIndex = (i + 11) % _testClassAllocationSize;
			array[i]->Ref11 = array[refIndex];
			refIndex = (i + 12) % _testClassAllocationSize;
			array[i]->Ref12 = array[refIndex];
			refIndex = (i + 13) % _testClassAllocationSize;
			array[i]->Ref13 = array[refIndex];
			refIndex = (i + 14) % _testClassAllocationSize;
			array[i]->Ref14 = array[refIndex];
			refIndex = (i + 15) % _testClassAllocationSize;
			array[i]->Ref15 = array[refIndex];
			refIndex = (i + 16) % _testClassAllocationSize;
			array[i]->Ref16 = array[refIndex];
			refIndex = (i + 17) % _testClassAllocationSize;
			array[i]->Ref17 = array[refIndex];
			refIndex = (i + 18) % _testClassAllocationSize;
			array[i]->Ref18 = array[refIndex];
			refIndex = (i + 19) % _testClassAllocationSize;
			array[i]->Ref19 = array[refIndex];
			refIndex = (i + 20) % _testClassAllocationSize;
			array[i]->Ref20 = array[refIndex];
		}
		//--------------------------------------------------
		
		// ---------------------- Delete Operator Test ------------------------

		Start();

		delete[] array;

		auto time = GetTime();
		summDeleteTime += time;
	}

	auto avgDeleteTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete 20 Ref Class Test = ");
	WriteDouble(avgDeleteTime);
	WriteString("ms\r\n");
}





void TestArraysMemoryAllocation()
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
			auto array = new int*[_testArrayAccessSize];

			Start();

			//--------------------------------------------------
			for (int i = 0; i < arrayCount; i++)
			{
				array[i] = new int[subArraySize];
			}
			//--------------------------------------------------

			auto time = GetTime();
			summAllocationTime += time;

			// ---------------------- Delete Operator Test ------------------------

			Start();

			//--------------------------------------------------
			for (int i = 0; i < arrayCount; i++)
			{
				delete[] array[i];
			}
			//--------------------------------------------------

			delete[] array;

			time = GetTime();
			summDeleteTime += time;
		}

		WriteString("------------------------------ \r\n");
		WriteString("Array Count = ");
		WriteDouble(arrayCount);
		WriteString("\r\n");

		auto avgAllocationTime = summAllocationTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size New Array Test = ");
		WriteDouble(avgAllocationTime);
		WriteString("ms\r\n");

		WriteDouble(subArraySize);
		auto avgDeleteTime = summDeleteTime / _testRepeatCount;
		WriteString( "Size Delete Array Test = ");
		WriteDouble(avgDeleteTime);
		WriteString("ms\r\n");
	}
}


void TestVectorMemoryAllocation()
{
	int subArraySize = 1;

	int arrayCount = 1000000000;

	for (int onderNumber = 0; onderNumber < 6; onderNumber++)
	{
		subArraySize *= 10;

		arrayCount /= 10;

		// --------------------- New Operator Test ---------------------------------

		double summAllocTime = 0;
		double summDeleteTime = 0;

		for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
		{
			auto array = new std::vector<int>*[_testArrayAccessSize];

			Start();

			//--------------------------------------------------
			for (int i = 0; i < arrayCount; i++)
			{
				array[i] = new std::vector<int>(subArraySize);
			}
			//--------------------------------------------------

			auto time = GetTime();
			summAllocTime += time;

			// ---------------------- Delete Operator Test ------------------------

			Start();

			//--------------------------------------------------
			for (int i = 0; i < arrayCount; i++)
			{
				delete array[i];
			}
			//--------------------------------------------------

			delete[] array;

			time = GetTime();
			summDeleteTime += time;
		}

		WriteString("------------------------------ \r\n");
		WriteString("Array Count = ");
		WriteDouble(arrayCount);
		WriteString("\r\n");

		auto avgAllocTime = summAllocTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size New Vector Test = ");
		WriteDouble(avgAllocTime);
		WriteString("ms\r\n");

		auto avgDeleteTime = summDeleteTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size Delete Vector Test = ");
		WriteDouble(avgDeleteTime);
		WriteString("ms\r\n");
	}
}

template <int threadCount>
void TestArraysMemoryAllocationMT()
{
	int subArraySize = 1;

	int arrayCount = 1000000000;

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
			auto array = new int*[_testArrayAccessSize];

			cout << "begin allocation\r\n";

			Start();

			//--------------------------------------------------

			std::thread threads[threadCount];

			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

				threads[threadIndex] = std::thread([array, arrayCount, subArraySize, threadIndex]()
					{
					long long beginIndexLong = (long long)arrayCount * threadIndex / threadCount;
					long long endIndexLong = (long long)arrayCount * (threadIndex + 1) / threadCount;

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


			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
				threads[threadIndex].join();
			}


			//--------------------------------------------------

			auto time = GetTime();
			summAllocationTime += time;

			cout << "end allocation\r\n";

			// ---------------------- Delete Operator Test ------------------------

			Start();

			//--------------------------------------------------
			std::thread threadsToDelete[threadCount];
			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

				threadsToDelete[threadIndex] = std::thread([array, arrayCount, subArraySize, threadIndex]()
				{
					long long beginIndexLong = (long long)arrayCount * threadIndex / threadCount;
					long long endIndexLong = (long long)arrayCount * (threadIndex + 1) / threadCount;

					int beginIndex = (int)beginIndexLong;
					int endIndex = (int)endIndexLong;

					for (int i = beginIndex
						; i < endIndex
						; i++)
					{
						delete[] array[i];
					}
				});
			}


			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
				threadsToDelete[threadIndex].join();
			}
			
			//--------------------------------------------------

			delete[] array;

			time = GetTime();
			summDeleteTime += time;
		}

		WriteString("------------------------------ \r\n");
		WriteString("Array Count = ");
		WriteDouble(arrayCount);
		WriteString("\r\n");

		auto avgAllocationTime = summAllocationTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size New Array Test MT = ");
		WriteDouble(avgAllocationTime);
		WriteString("ms\r\n");

		WriteDouble(subArraySize);
		auto avgDeleteTime = summDeleteTime / _testRepeatCount;
		WriteString("Size Delete Array Test MT = ");
		WriteDouble(avgDeleteTime);
		WriteString("ms\r\n");
	}
}

template <int threadCount>
void TestVectorMemoryAllocationMT()
{
	int subArraySize = 1;

	int arrayCount = 1000000000;

	for (int onderNumber = 0; onderNumber < 6; onderNumber++)
	{
		subArraySize *= 10;

		arrayCount /= 10;

		// --------------------- New Operator Test ---------------------------------

		double summAllocTime = 0;
		double summDeleteTime = 0;

		for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
		{
			auto array = new std::vector<int>*[_testArrayAccessSize];

			Start();

			//--------------------------------------------------

			std::thread threads[threadCount];

			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

				threads[threadIndex] = std::thread([array, arrayCount, subArraySize, threadIndex]()
				{
					long long beginIndexLong = (long long)arrayCount * threadIndex / threadCount;
					long long endIndexLong = (long long)arrayCount * (threadIndex + 1) / threadCount;

					int beginIndex = (int)beginIndexLong;
					int endIndex = (int)endIndexLong;

					for (int i = beginIndex
						; i < endIndex
						; i++)
					{
						array[i] = new std::vector<int>(subArraySize);
					}
				});
			}
			
			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
				threads[threadIndex].join();
			}
			
			
			//--------------------------------------------------

			auto time = GetTime();
			summAllocTime += time;

			// ---------------------- Delete Operator Test ------------------------

			Start();

			std::thread threadsToDelete[threadCount];
			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

				threadsToDelete[threadIndex] = std::thread([array, arrayCount, subArraySize, threadIndex]()
				{
					long long beginIndexLong = (long long)arrayCount * threadIndex / threadCount;
					long long endIndexLong = (long long)arrayCount * (threadIndex + 1) / threadCount;

					int beginIndex = (int)beginIndexLong;
					int endIndex = (int)endIndexLong;

					for (int i = beginIndex
						; i < endIndex
						; i++)
					{
						delete array[i];
					}
				});
			}

			for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
				threadsToDelete[threadIndex].join();
			}
			//--------------------------------------------------

			delete[] array;

			time = GetTime();
			summDeleteTime += time;
		}

		WriteString("------------------------------ \r\n");
		WriteString("Array Count = ");
		WriteDouble(arrayCount);
		WriteString("\r\n");

		auto avgAllocTime = summAllocTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size New Vector Test MT = ");
		WriteDouble(avgAllocTime);
		WriteString("ms\r\n");

		auto avgDeleteTime = summDeleteTime / _testRepeatCount;
		WriteDouble(subArraySize);
		WriteString(" Size Delete Vector Test MT = ");
		WriteDouble(avgDeleteTime);
		WriteString("ms\r\n");
	}
}






void TestClassMemoryAllocationMT()
{
	auto array = new EmptyClass*[_testClassAllocationSize];

	// --------------------- New Operator Test ---------------------------------

	double summAllocTime = 0;
	double summDeleteTime = 0;

	for (int iterationIndes = 0; iterationIndes < _testRepeatCount; iterationIndes++)
	{
		Start();

		const int threadCount = 4;
		std::thread threads[threadCount];

		for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

			threads[threadIndex] = std::thread([array, threadIndex, threadCount]()
			{
				long long beginIndexLong = (long long)_testClassAllocationSize * threadIndex / threadCount;
				long long endIndexLong = (long long)_testClassAllocationSize * (threadIndex + 1) / threadCount;

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
		
		for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
			threads[threadIndex].join();
		}

		auto time = GetTime();
		summAllocTime += time;
	

	// ---------------------- Delete Operator Test ------------------------
		
		Start();

		std::thread threadsToDelete[threadCount];
		for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {

			threadsToDelete[threadIndex] = std::thread([array, threadIndex, threadCount]()
			{
				long long beginIndexLong = (long long)_testClassAllocationSize * threadIndex / threadCount;
				long long endIndexLong = (long long)_testClassAllocationSize * (threadIndex + 1) / threadCount;

				int beginIndex = (int)beginIndexLong;
				int endIndex = (int)endIndexLong;

				for (int i = beginIndex
					; i < endIndex
					; i++)
				{
					delete array[i];
				}
			});
		}
		
		for (int threadIndex = 0; threadIndex < threadCount; threadIndex++) {
			threadsToDelete[threadIndex].join();
		}

	
		time = GetTime();
		summDeleteTime += time;
	}

	auto avgTime = summAllocTime / _testRepeatCount;
	WriteString("New Class Test MT = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");

	avgTime = summDeleteTime / _testRepeatCount;
	WriteString("Delete Class Test MT = ");
	WriteDouble(avgTime);
	WriteString("ms\r\n");


	delete[] array;
}


void RestoreZeroes(int* inData, int inDataLength, int* outData)
{
	for (int inDataPosition = 0; inDataPosition < inDataLength; inDataPosition++) {
		int inItem = inData[inDataPosition];
		if (0 == inItem) {
			inDataPosition++;
			int zeroCount = inData[inDataPosition];
			for (int zeroIndex = 0; zeroIndex < zeroCount; zeroIndex++) {
				*outData = 0;
				outData++;
			}
		}
		else {
			*outData = inItem;
			outData++;
		}
	}
}

template<typename T>
constexpr T pi = T(3.1415926535897932385);


constexpr int sum(int a, int b)
{
	return a + b;
}

int main()
{
	fileOut_.open("cpp_report.txt", std::ios_base::out);



	//TestInlineMethodsClass testInline;
	//testInline.test();
	//fileOut_.flush();

	//TestNoInlineMethodsClass testNoInline;
	//testNoInline.test();
	//fileOut_.flush();
	//
	//	
	//TestArrayAccess();
	//fileOut_.flush();
	//TestVectorAccess();
	//fileOut_.flush();

	//TestArrayRandomAccess();
	//fileOut_.flush();
	//TestVectorRandomAccess();
	//fileOut_.flush();
	//TestArrayRandomAccessNoRandom();
	//fileOut_.flush();
	//TestVectorRandomAccessNoRandom();
	//fileOut_.flush();

	//TestClassMemoryAllocationMT();
	//fileOut_.flush();

	//TestEmptyClassMemoryAllocation();
	//fileOut_.flush();
	//TestOneRefClassMemoryAllocation();
	//fileOut_.flush();
	//TestFiveRefClassMemoryAllocation();
	//fileOut_.flush();
	//TestTenRefClassMemoryAllocation();
	//fileOut_.flush();
	//TestFifteenRefClassMemoryAllocation();
	//fileOut_.flush();
	//TestTwentyRefClassMemoryAllocation();
	//fileOut_.flush();
	//
	//TestEmptyClassMemoryAllocationSharedPtr();
	//fileOut_.flush();

	//TestOneRefClassMemoryAllocationSharedPtr();
	//fileOut_.flush();
	//TestFiveRefClassMemoryAllocationSharedPtr();
	//fileOut_.flush();
	//TestTenRefClassMemoryAllocationSharedPtr();
	//fileOut_.flush();
	//TestFifteenRefClassMemoryAllocationSharedPtr();
	//fileOut_.flush();
	//TestTwentyRefClassMemoryAllocationSharedPtr();
	//fileOut_.flush();

	//TestArraysMemoryAllocation();
	//fileOut_.flush();
	//TestVectorMemoryAllocation();
	//fileOut_.flush();

	WriteDouble(_threadCount);
	WriteString(" threads:\r\n");

	TestArraysMemoryAllocationMT<8>();
	fileOut_.flush();

	TestVectorMemoryAllocationMT<8>();
	fileOut_.flush();

	WriteString("Single thread:\r\n");

	TestArraysMemoryAllocationMT<1>();
	fileOut_.flush();

	TestVectorMemoryAllocationMT<1>();
	fileOut_.flush();

	//TestClassMemoryAllocationMT();
	//fileOut_.flush();

	cout << "----------- Complete ------------------\r\n";


	// ��� ����� �������� � stack overflow ���� �� 10 ���. �����

	//TestOneRefClassMemoryAllocationSharedPtrStackOverflow();
	//TestFiveRefClassMemoryAllocationSharedPtrStackOverflow();
	//TestTenRefClassMemoryAllocationSharedPtrStackOverflow();
	//TestFifteenRefClassMemoryAllocationSharedPtrStackOverflow();
	//TestTwentyRefClassMemoryAllocationSharedPtrStackOverflow();


	//getchar();
    return 0;
}

