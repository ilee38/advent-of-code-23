using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day6
{
   private static readonly Regex DigitsPattern = new Regex(@"\d+");

   public double WaysToWinMultiplication()
   {
      double total;
      List<string> racesInfo = new List<string>(File.ReadAllLines("Day6/input.txt"));
      List<int> raceLengths = ParseDigits(racesInfo[0]);
      List<int> reaceDistanceRecords = ParseDigits(racesInfo[1]);

      total = MultiplyWaysToWin(raceLengths, reaceDistanceRecords);
      return total;
   }

   private List<int> ParseDigits(string data)
   {
      var digitsExpression = DigitsPattern.Matches(data);
      return digitsExpression.Cast<Match>().Select(match => int.Parse(match.Value)).ToList();
   }

   private double MultiplyWaysToWin(List<int> raceLengths, List<int> raceDistanceRecords)
   {
      double totalMultiplication = 1;
      int numberOfRaces = raceLengths.Count;
      double waysToWin;

      for (int i = 0; i < numberOfRaces; i++)
      {
         waysToWin = GetWaysToWin(raceLengths[i], raceDistanceRecords[i]);
         if ( waysToWin > 0)
         {
            totalMultiplication *= waysToWin;
         }
      }

      return totalMultiplication;
   }

   private double GetWaysToWin(int raceLength, int raceDistanceRecord)
   {
      double waysToWinCount = 0;
      int distance;

      for (int i = 1; i <raceLength; i++)
      {
         distance = i * (raceLength - i);
         if (distance > raceDistanceRecord)
         {
            waysToWinCount += 1;
         }
      }

      return waysToWinCount;
   }

}
