using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a single delimiter or multiple delimiters enclosed by brackets eg: [$]");
            Console.Write("//");
            string delimRule = Console.ReadLine();

            // Regex for expressions between brackets, does not consider nested brackets
            Console.WriteLine();
            List<string> delimInList = new List<string>();
            if (delimRule != "")
            {
                string rgxExpr = @"(?<=\[).+?(?=\])";
                MatchCollection rgxRemoveBracketMC = Regex.Matches(delimRule, rgxExpr);
                for (int i = 0; i < rgxRemoveBracketMC.Count; i++)
                {
                    delimInList.Add(Regex.Escape(rgxRemoveBracketMC[i].ToString()));
                }

                // If no containing brackets are detected, default to the entire string
                if (delimInList.Count == 0)
                {
                    delimInList.Add(Regex.Escape(delimRule));
                }
            }

            string[] delimIn = delimInList.ToArray();
            // Returns a string array of delimiters that were inputted

            // Infinite while loop to read until enter is hit twice
            Console.WriteLine("Input values, enter blank value \"\" to end entry.");
            string input = "";
            do
            {
                string currInput = Console.ReadLine();
                if (currInput == "")
                {
                    break;
                }
                else
                {
                    input += currInput + @"\n";
                }
            } while (true);
            // Remove the last line break from hitting enter twice
            if (input.Contains(@"\n"))
            {
                input = input.Remove(input.LastIndexOf(@"\n"));
            }
            else
            {
                input = "0";
            }


            // Find all the negatives in the inputted string
            MatchCollection isThereNegative = Regex.Matches(input, @"-\d+");
            var negativeList = new string[isThereNegative.Count];
            try
            {
                for (int i = 0; i < negativeList.Length; i++)
                {
                    negativeList[i] = isThereNegative[i].ToString();
                }

                if (negativeList.Length > 0)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("negatives not allowed: " + String.Join(", ", negativeList));
            }

            // Default Delimiters
            string[] delimDefault = new string[] { ",", @"\\n" };

            // Concatenate the default delimiter array and the inputted delimiter array
            string[] delimAll = new string[delimDefault.Length + delimIn.Length];
            delimDefault.CopyTo(delimAll, 0);
            delimIn.CopyTo(delimAll, delimDefault.Length);

            // Substitute all delimiters with comma for easier split later
            string delimPattern = String.Join("|", delimAll);

            input = input.Replace(@"\ ", @"\s"); //Space Delimiter
            string numberStrReplace = Regex.Replace(input, delimPattern, ",");
            string numberStrTrim = numberStrReplace.Replace(@" ", ""); // Trim space characters if it's not a delimiter

            
            int sum = Program.Add(numberStrTrim);
            Console.WriteLine("= " + sum);
        }

        public static int Add(string numbers)
        {
            string[] stringList = numbers.Split(",", StringSplitOptions.None);

            // Sum values of all legitimate entries
            int calcValue = 0;
            Console.WriteLine("Begin sum.");
            for (int i = 0; i < stringList.Length; i++)
            {
                int currValue = 0;
                // Catch exceptions when there's issues parsing or the value is not a 32 bit integer
                try
                {
                    currValue = Convert.ToInt32(stringList[i]);
                }
                catch (System.FormatException)
                {
                    Console.WriteLine(stringList[i] + " is not a 32 bit integer");
                }
                catch (System.OverflowException)
                {
                    Console.WriteLine(stringList[i] + " is greater than 32 bits");
                }

                if (currValue >= 0 && currValue <= 1000)
                {
                    if (currValue > 0)
                    {
                        Console.WriteLine($"+{currValue}");
                    }
                    calcValue += currValue;
                }
            }
            Console.WriteLine("End sum.");

            return calcValue;
        }
    }
}

