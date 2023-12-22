using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day4
{
   private static Regex digitsExpression = new Regex(@"\d+");

   // Part 1 solution
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

   // Part 2 solution
   public int TotalScratchCards()
   {
      var cardWinnings = new List<List<int>>();
      var cardsCopiesWonCache = new Dictionary<int, List<int>>();
      int currentCardNumber;

      using var streamReader = new StreamReader("Day4/input.txt");
      while (!streamReader.EndOfStream)
      {
         string? scratchCard = streamReader.ReadLine();
         currentCardNumber = int.Parse(digitsExpression.Match(scratchCard.Substring(0, scratchCard.IndexOf(":"))).Value);
         ObtainWinningsFromCard(
            scratchCard.Substring(scratchCard.IndexOf(':') + 1),
            cardWinnings,
            currentCardNumber,
            cardsCopiesWonCache);
      }
      ParseCardCopies(cardWinnings, cardsCopiesWonCache);

      return cardWinnings.Count;
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

   private void ObtainWinningsFromCard(string card, List<List<int>> cardWinnings, int currentCardNumber, Dictionary<int, List<int>> cardsCopiesWonCache)
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

      // Add the current card to the adjacency list
      cardWinnings.Add(new List<int>{currentCardNumber});

      for (int i = 1; i <= matchingNumbersCount; i++)
      {
         cardWinnings[currentCardNumber - 1].Add(currentCardNumber + i);
      }

      if (!cardsCopiesWonCache.ContainsKey(currentCardNumber))
      {
         cardsCopiesWonCache.Add(currentCardNumber, cardWinnings[currentCardNumber - 1]);
      }
   }

   private void ParseCardCopies(List<List<int>> cardWinnings, Dictionary<int, List<int>> cardsCopiesWonCache)
   {
      int currentIndex = 0;
      while (true)
      {
         if (currentIndex >= cardWinnings.Count)
         {
            break;
         }

         for (int i = 1; i < cardWinnings[currentIndex].Count; i++)
         {
            cardWinnings.Add(cardsCopiesWonCache[cardWinnings[currentIndex][i]]);
         }
         currentIndex += 1;
      }
   }
}
