using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day4
{
   private static Regex digitsExpression = new Regex(@"\d+");

   public int TotalScratchCardsPoints()
   {
      int totalPoints = 0;

      using var streamReader = new StreamReader("Day4/input.txt");
      while (!streamReader.EndOfStream)
      {
         string? scratchCard = streamReader.ReadLine();
         totalPoints += CalculateCardPoints(scratchCard.Substring(scratchCard.IndexOf(':') + 1));
      }

      return totalPoints;
   }

   private int CalculateCardPoints(string card)
   {
      int matchingNumbersCount = 0;

      var winningNumbersExpressions = digitsExpression.Matches(card.Substring(0, card.IndexOf('|')));
      var ourNumbersExpressions = digitsExpression.Matches(card.Substring(card.IndexOf('|') + 1));

      List<int> winningNumbers = winningNumbersExpressions.Cast<Match>().Select(match => int.Parse(match.Value)).ToList();
      List<int> ourNumbers = ourNumbersExpressions.Cast<Match>().Select(match => int.Parse(match.Value)).ToList();


      foreach (int number in ourNumbers)
      {
         if (winningNumbers.Contains(number))
         {
            matchingNumbersCount += 1;
         }
      }
      return matchingNumbersCount > 0 ? (int)Math.Pow(2, matchingNumbersCount - 1) : matchingNumbersCount;
   }
}
