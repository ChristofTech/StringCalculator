using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringCalculator
{
    public class AddClass
    {
        static void Main(string[] args)
        {
            // Infinite while loop to read until enter is hit twice
            Console.WriteLine("Input values, press enter twice in a row to confirm submission (ie: enter \"\").");
            Console.WriteLine("\n" + "Optionally, in the first line begin delimiter entry with two forward-slashes //." + "\n" + "Enter a single delimiter or multiple delimiters enclosed by brackets (eg: [$])\nthen press enter." + "\n");
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
            if (input.Contains(@"\n") && input.Substring(input.Length - 2, 2) == @"\n")
            {
                input = input.Remove(input.LastIndexOf(@"\n"));
            }
            else
            {
                input = "0";
            }

            int sum = AddClass.Add(input);
            Console.WriteLine("= " + sum);
            
        }

        public static int Add(string numbers)
        {
            string input = numbers;
            Console.WriteLine("Input is: " + input);

            List<string> delimInList = new List<string>();
            if (input[0] == '/' && input[1] == '/')
            {
                Match delimLine = Regex.Match(input, @"(?<=//)(.*?)(?=\\n)");
                string delimRule = delimLine.ToString();

                // Regex for expressions between brackets, does not consider nested brackets
                if (delimRule != "")
                {
                    MatchCollection rgxRemoveBracketMC = Regex.Matches(delimRule, @"(?<=\[).+?(?=\])");
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

                // Remove delimiter line now that info has been parsed
                input = Regex.Replace(input, @"//(.*?)\\n", "");
            }
            // Create string array of delimiters that were inputted
            string[] delimIn = delimInList.ToArray();

            
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

            string[] stringList = numberStrTrim.Split(",", StringSplitOptions.None);
            

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

