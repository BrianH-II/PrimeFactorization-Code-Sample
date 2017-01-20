using System;
using System.Collections.Generic;
using System.Numerics;

namespace PrimeFactorization
{
    class Program
    {
        /*
         *  This program allows the user to enter an arbitrarily large positive integer greater than 1.
         *  It takes the entered number, and returns the prime factorization of that number.
         *  The prime factorization will be displayed in both a "power" form (a^b) and an "expanded" form (a * a * a * ...)
         */

        private static BigInteger number;

        static void Main(string[] args)
        {
            Console.WriteLine("This program will display the prime factorization of a number");
            Console.WriteLine("Enter \"Exit\" to end");
            Console.WriteLine();
            Console.WriteLine("To begin, type a whole number greater than 1, then click Enter");
            Console.WriteLine();

            do
            {
                string input = Console.ReadLine();

                if (input == "Exit")
                {
                    Environment.Exit(0);
                }
                else
                {
                    try
                    {
                        //First, the program checks to see if the user entered a number in the appropriate format
                        //If a format exception is thrown, the program will prompt the user to try again
                        number = BigInteger.Parse(input);

                        try
                        {
                            //Since uLong math is significantly faster than BigInteger math, the program determines if uLongs may be used
                            //If an overflow exception is thrown, the program proceeds using BigIntegers
                            ulong remainingNumber =  ulong.Parse(input);

                            List<ulong> primeFactors = uLongProcess(remainingNumber);

                            string longPrimeFactorString = "";
                            string shortPrimeFactorString = "";

                            ulong factor = primeFactors[0];
                            byte factorCount = 0;

                            foreach (ulong prime in primeFactors)
                            {
                                if (prime == factor)
                                {
                                    factorCount++;
                                }
                                else
                                {
                                    if (shortPrimeFactorString == "")
                                    {
                                        shortPrimeFactorString = factor + "^" + factorCount;
                                    }
                                    else
                                    {
                                        shortPrimeFactorString = shortPrimeFactorString + " * " + factor + "^" + factorCount;
                                    }

                                    factor = prime;
                                    factorCount = 1;
                                }

                                if (longPrimeFactorString == "")
                                {
                                    longPrimeFactorString = prime.ToString();
                                }
                                else
                                {
                                    longPrimeFactorString = longPrimeFactorString + " * " + prime;
                                }
                            }

                            if (shortPrimeFactorString == "")
                            {
                                shortPrimeFactorString = factor + "^" + factorCount;
                            }
                            else
                            {
                                shortPrimeFactorString = shortPrimeFactorString + " * " + factor + "^" + factorCount;
                            }

                            Console.WriteLine(number + " = " + shortPrimeFactorString + " = " + longPrimeFactorString);
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("Factorization Complete. You may enter another number now.");
                            Console.WriteLine();
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Wow! That's a big integer! This may take a while!");
                            Console.WriteLine("(Calculation could take several minutes or longer depending on the factors)");
                            Console.WriteLine();

                            BigInteger remainingNumber = number;

                            List<BigInteger> primeFactors = BigIntegerProcess(remainingNumber);

                            string longPrimeFactorString = "";
                            string shortPrimeFactorString = "";

                            BigInteger factor = primeFactors[0];
                            byte factorCount = 0;

                            foreach (BigInteger prime in primeFactors)
                            {
                                if (prime == factor)
                                {
                                    factorCount++;
                                }
                                else
                                {
                                    if (shortPrimeFactorString == "")
                                    {
                                        shortPrimeFactorString = factor + "^" + factorCount;
                                    }
                                    else
                                    {
                                        shortPrimeFactorString = shortPrimeFactorString + " * " + factor + "^" + factorCount;
                                    }

                                    factor = prime;
                                    factorCount = 1;
                                }

                                if (longPrimeFactorString == "")
                                {
                                    longPrimeFactorString = prime.ToString();
                                }
                                else
                                {
                                    longPrimeFactorString = longPrimeFactorString + " * " + prime;
                                }
                            }

                            if (shortPrimeFactorString == "")
                            {
                                shortPrimeFactorString = factor + "^" + factorCount;
                            }
                            else
                            {
                                shortPrimeFactorString = shortPrimeFactorString + " * " + factor + "^" + factorCount;
                            }

                            Console.WriteLine(number + " = " + shortPrimeFactorString + " = " + longPrimeFactorString);
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine("Factorization Complete. You may enter another number now.");
                            Console.WriteLine();
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine("Unable to parse input to an integer. Please try again.");
                        Console.WriteLine("Note: Do not put any symbols (commas, periods, etc.) in the number");
                        Console.WriteLine();
                    }
                }
            }
            while (true);
        }

        private static List<ulong> uLongProcess(ulong remainingNumber)
        {
            ulong divisor = 2;

            List<ulong> primeFactors = new List<ulong>();

            while (divisor <= Math.Sqrt(remainingNumber))
            {
                if (remainingNumber % divisor == 0)
                {
                    primeFactors.Add(divisor);
                    remainingNumber = remainingNumber / divisor;
                }
                else
                {
                    divisor++;
                }
            }
            primeFactors.Add(remainingNumber);

            return primeFactors;
        }

        private static List<BigInteger> BigIntegerProcess(BigInteger remainingNumber)
        {
            BigInteger divisor = 2;
            bool mustBeBigInt = true;

            List<BigInteger> primeFactors = new List<BigInteger>();

            while (divisor * divisor <= remainingNumber && mustBeBigInt)
            {
                if (remainingNumber % divisor == 0)
                {
                    primeFactors.Add(divisor);
                    remainingNumber = remainingNumber / divisor;

                    //With each division, the program determines if the remaining value is less than the max value of a uLong
                    //If the remaining value is less than that of a uLong, the program does the rest of the computation using uLongs, since the math is significantly faster
                    if (remainingNumber < ulong.MaxValue)
                    {
                        mustBeBigInt = false;
                        List<ulong> remainingFactors = uLongProcess((ulong)remainingNumber);

                        foreach (ulong entry in remainingFactors)
                        {
                            primeFactors.Add(entry);
                        }
                    }
                }
                else
                {
                    divisor++;
                }
            }
            primeFactors.Add(remainingNumber);

            return primeFactors;
        }
    }
}