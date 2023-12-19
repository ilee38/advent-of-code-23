using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day3
{
   private static char[] Symbols = {'@', '#', '$', '%', '&', '*', '/', '+', '-', '='};
   private int sum = 0;

   public int ComputeSum()
   {
      List<string?> lines;

      // Read all lines into a list
      lines = new List<string?>(File.ReadAllLines(@"Day3/input.txt"));

      for (int row = 0; row < lines.Count; row++)
      {
         for (int col = 0; col < lines[row].Length; col++)
         {
            if (Symbols.Contains(lines[row][col]))
            {
               // Look for numbers in the lines above, inline, and below of the symbol
               // *here we're assuming the first row and first column don't have any symbols
               sum += FindAdjacentNumbers(lines[row - 1], col);
               sum += FindAdjacentNumbers(lines[row], col);
               sum += FindAdjacentNumbers(lines[row + 1], col);
            }
         }
      }
      return sum;
   }

   private int FindAdjacentNumbers(string line, int symbolCol)
   {
      Regex digitPattern = new Regex(@"\d+");
      List<int> columnsToCheck = new List<int> {symbolCol - 1, symbolCol, symbolCol + 1};
      int rowSum = 0;

      var numbersInRow= digitPattern.Matches(line);
      foreach (Match number in numbersInRow)
      {
         if (NumberIsAdjacent(number.Index, number.Length, columnsToCheck))
         {
            rowSum += int.Parse(number.Value);
         }
      }
      return rowSum;
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
