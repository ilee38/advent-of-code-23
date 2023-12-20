using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day3
{
   private static char[] Symbols = {'@', '#', '$', '%', '&', '*', '/', '+', '-', '='};
   private static char GearSymbol = '*';
   private int sum;

   public int ComputeSum()
   {
      List<string?> lines;

      // Read all lines into a list
      lines = new List<string?>(File.ReadAllLines(@"Day3/input.txt"));
      sum = 0;

      for (int row = 0; row < lines.Count; row++)
      {
         for (int col = 0; col < lines[row].Length; col++)
         {
            if (Symbols.Contains(lines[row][col]))
            {
               // Look for numbers in the lines above, inline, and below of the symbol
               // *here we're assuming the first row and first column don't have any symbols
               sum += FindAdjacentNumberSum(lines[row - 1], col);
               sum += FindAdjacentNumberSum(lines[row], col);
               sum += FindAdjacentNumberSum(lines[row + 1], col);
            }
         }
      }
      return sum;
   }

   public int GearRatioSum()
   {
      List<string>? lines;
      lines = new List<string>(File.ReadAllLines(@"Day3/input.txt"));
      sum = 0;

      for (int row = 0; row < lines.Count; row++)
      {
         for (int col = 0; col < lines[row].Length; col++)
         {
            if (lines[row][col].Equals(GearSymbol))
            {
               sum += FindGearRatio(lines[row - 1], lines[row], lines[row + 1], col);
            }
         }
      }
      return sum;
   }

   private int FindAdjacentNumberSum(string line, int symbolCol)
   {
      Regex digitPattern = new Regex(@"\d+");
      List<int> columnsToCheck = new List<int> {symbolCol - 1, symbolCol, symbolCol + 1};
      int rowSum = 0;

      var numbersInRow = digitPattern.Matches(line);
      foreach (Match number in numbersInRow)
      {
         if (NumberIsAdjacent(number.Index, number.Length, columnsToCheck))
         {
            rowSum += int.Parse(number.Value);
         }
      }
      return rowSum;
   }

   private int FindGearRatio(string lineAbove, string sameLine, string lineBelow, int symbolCol)
   {
      Regex digitPattern = new Regex(@"\d+");
      List<int> columnsToCheck = new List<int> {symbolCol - 1, symbolCol, symbolCol + 1};
      List<string> linesToCheck = new List<string> { lineAbove, sameLine, lineBelow};
      var partNumbersList = new List<int>();
      int gearRatio = 0;

      foreach (string line in linesToCheck)
      {
         var numbersInRow = digitPattern.Matches(line);
         foreach (Match number in numbersInRow)
         {
            if (NumberIsAdjacent(number.Index, number.Length, columnsToCheck))
            {
               partNumbersList.Add(int.Parse(number.Value));
            }
         }
      }

      if (partNumbersList.Count == 2)
      {
         gearRatio = partNumbersList[0] * partNumbersList[1];
      }

      return gearRatio;
   }

   private bool NumberIsAdjacent(int startIndex, int length, List<int> columnsToCheck)
   {
      for (int i = startIndex; i < startIndex + length; i++)
      {
         if (columnsToCheck.Contains(i))
         {
            return true;
         }
      }
      return false;
   }
}
