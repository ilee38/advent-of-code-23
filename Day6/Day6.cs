using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day6
{
   private static readonly Regex DigitsPattern = new Regex(@"\d+");

   /// <summary>
   /// Solution to Part 1
   /// </summary>
   /// <returns>Multiplication of all ways to win for all races.</returns>
   public double WaysToWinMultiplication()
   {
      double total;
      List<string> racesInfo = new List<string>(File.ReadAllLines("Day6/input.txt"));
      List<double> raceLengths = ParseDigits(racesInfo[0]);
      List<double> reaceDistanceRecords = ParseDigits(racesInfo[1]);

      total = MultiplyWaysToWin(raceLengths, reaceDistanceRecords);
      return total;
   }

   /// <summary>
   /// Solution to Part 2
   /// </summary>
   /// <returns>Number of ways to win the single race.</returns>
   public double NumberOfWaysToWin()
   {
      double waysToWin;
      List<string> raceInfo = new List<string>(File.ReadAllLines("Day6/input.txt"));
      double raceLength = ParseSingleDigit(raceInfo[0].Substring(raceInfo[0].IndexOf(':') + 1));
      double raceDistanceRecord = ParseSingleDigit(raceInfo[1].Substring(raceInfo[1].IndexOf(':') + 1));

      waysToWin = GetWaysToWin(raceLength, raceDistanceRecord);
      return waysToWin;
   }

   private List<double> ParseDigits(string data)
   {
      var digitsExpression = DigitsPattern.Matches(data);
      return digitsExpression.Cast<Match>().Select(match => double.Parse(match.Value)).ToList();
   }

   private double MultiplyWaysToWin(List<double> raceLengths, List<double> raceDistanceRecords)
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

   private double GetWaysToWin(double raceLength, double raceDistanceRecord)
   {
      double waysToWinCount = 0;
      double distance;

      for (double i = 1; i < raceLength; i++)
      {
         distance = i * (raceLength - i);
         if (distance > raceDistanceRecord)
         {
            waysToWinCount += 1;
         }
      }

      return waysToWinCount;
   }

   private double ParseSingleDigit(string data)
   {
      string digits = data.Trim().Replace(" ", "");
      return double.Parse(DigitsPattern.Match(digits).Value);
   }

}
