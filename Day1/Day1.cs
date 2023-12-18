namespace AdventOfCode23;

public class Day1
{
    private int calibrationValuesSum = 0;

    public int ComputeSum()
    {
        try
        {
            using var sr = new StreamReader("Day1/input.txt");
            while (sr.Peek() != -1)
            {
                string? line = sr.ReadLine();
                string? digits = FindFirstAndLastDigitsInString(line);
                calibrationValuesSum += int.Parse(digits);
            }
       }
        catch (IOException e)
        {
            Console.WriteLine("File could not be read:");
            Console.WriteLine(e.Message);
        }

        return calibrationValuesSum;
    }

    private static string? FindFirstAndLastDigitsInString(string? str)
    {
        string[] stringDigits = {
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            "1", "2", "3", "4", "5", "6", "7", "8", "9"};
        int maxIndex = int.MinValue;
        int minIndex = int.MaxValue;
        string? firstDigit = null;
        string? secondDigit = null;

        if (!string.IsNullOrEmpty(str))
        {
            // Check if the first character is a digit
            if (int.TryParse(str.Substring(0, 1), out int first))
            {
                firstDigit = first.ToString();
                minIndex = 0;
            }
            // Check if the last character is a digit
            if (int.TryParse(str.Substring(str.Length - 1, 1), out int last))
            {
                secondDigit = last.ToString();
                maxIndex = str.Length - 1;
            }

            if (!string.IsNullOrEmpty(firstDigit) && !string.IsNullOrEmpty(secondDigit))
            {
                return $"{firstDigit}{secondDigit}";
            }
            else
            {
                foreach (var digit in stringDigits)
                {
                    int firstIndex = str.IndexOf(digit, StringComparison.OrdinalIgnoreCase);
                    int lastIndex = str.LastIndexOf(digit, StringComparison.OrdinalIgnoreCase);

                    if (firstIndex != -1 && lastIndex != -1)
                    {
                        if (firstIndex < minIndex)
                        {
                            minIndex = firstIndex;
                            firstDigit = digit;
                        }
                        if (lastIndex > maxIndex)
                        {
                            maxIndex = lastIndex;
                            secondDigit = digit;
                        }
                    }
                }
            }
            firstDigit = ParseDigitFromString(firstDigit);
            secondDigit = ParseDigitFromString(secondDigit);

            return $"{firstDigit}{secondDigit}";
        }

        return null; 
    }

    private static string ParseDigitFromString(string digit) 
    {
        var stringDigits = new Dictionary<string, string>
        {
            {"one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" }
        };

        if (int.TryParse(digit, out int result))
        {
            return result.ToString();
        }

        return stringDigits[digit.ToLower()];
    }
}
