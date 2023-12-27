using System.Data;
using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day5
{
   private static readonly Regex DigitsPattern = new Regex(@"\d+");

   /// <summary>
   /// Dictionaries representing the maps, where:
   /// key = source range start
   /// value = tuple: (destination range start, range length)
   /// </summary>
   private Dictionary<double, (double, double)> SeedToSoil = new();
   private Dictionary<double, (double, double)> SoilToFertilizer = new();
   private Dictionary<double, (double, double)> FertilizerToWater = new();
   private Dictionary<double, (double, double)> WaterToLight = new();
   private Dictionary<double, (double, double)> LightToTemperature = new();
   private Dictionary<double, (double, double)> TemperatureToHumidity = new();
   private Dictionary<double, (double, double)> HumidityToLocation = new();

   // Part 1 solution
   public double FindLowestLocationNumber()
   {
      List<double> seedLocations;
      List<string> data = new List<string>(File.ReadAllLines("Day5/input.txt"));
      List<double> seeds;

      ParseAllMaps(data);
      seeds = ParseSeeds(data[0]);
      seedLocations = FindSeedLocations(seeds);

      return seedLocations.Min();
   }

   // Part 2 solution
   public double FindLowestLocationForSeedRanges()
   {
      List<string> data = new List<string>(File.ReadAllLines("Day5/input.txt"));
      List<double> seedRangesLocations;

      // List of tuples of seed ranges where: Item1 = range start, Item2 = length of range
      List<(double, double)> seedRanges;

      ParseAllMaps(data);
      seedRanges = ParseSeedRanges(data[0]);
      seedRangesLocations = FindSeedRangesLocations(seedRanges);

      return seedRangesLocations.Min();
   }

   private void ParseAllMaps(List<string> data)
   {
      for (int startIndex = 2; startIndex < data.Count; startIndex++)
      {
         switch (data[startIndex].Trim())
         {
            case "seed-to-soil map:":
               startIndex += 1;
               ParseMap(SeedToSoil, data, startIndex);
               startIndex += SeedToSoil.Count;
               break;
            case "soil-to-fertilizer map:":
               startIndex += 1;
               ParseMap(SoilToFertilizer, data, startIndex);
               startIndex += SoilToFertilizer.Count;
               break;
            case "fertilizer-to-water map:":
               startIndex += 1;
               ParseMap(FertilizerToWater, data, startIndex);
               startIndex += FertilizerToWater.Count;
               break;
            case "water-to-light map:":
               startIndex += 1;
               ParseMap(WaterToLight, data, startIndex);
               startIndex += WaterToLight.Count;
               break;
            case "light-to-temperature map:":
               startIndex += 1;
               ParseMap(LightToTemperature, data, startIndex);
               startIndex += LightToTemperature.Count;
               break;
            case "temperature-to-humidity map:":
               startIndex += 1;
               ParseMap(TemperatureToHumidity, data, startIndex);
               startIndex += TemperatureToHumidity.Count;
               break;
            case "humidity-to-location map:":
               startIndex += 1;
               ParseMap(HumidityToLocation, data, startIndex);
               startIndex += HumidityToLocation.Count;
               break;
            default:
               break;
         }
      }
   }

   private void ParseMap(Dictionary<double, (double, double)> map, List<string> data, int startIndex)
   {
      string line;
      double rangeLength;
      double sourceRangeStart;
      double destinationRangeStart;

      for (int i = startIndex; i < data.Count; i++ )
      {
         line = data[i];
         if (string.IsNullOrEmpty(line) || line.Equals("\n"))
         {
            break;
         }

         var rangeValues = DigitsPattern.Matches(line);
         rangeLength = double.Parse(rangeValues[2].Value);
         sourceRangeStart = double.Parse(rangeValues[1].Value);
         destinationRangeStart = double.Parse(rangeValues[0].Value);

         map.Add(sourceRangeStart, (destinationRangeStart, rangeLength));
      }
   }

   private List<double> ParseSeeds(string seedsLine)
   {
      var parsedSeeds = DigitsPattern.Matches(seedsLine);
      return parsedSeeds.Cast<Match>().Select(match => double.Parse(match.Value)).ToList();
   }

   private List<(double, double)> ParseSeedRanges(string seedsLine)
   {
      var seedRanges = new List<(double, double)>();
      var parsedSeedRanges = DigitsPattern.Matches(seedsLine);

      for (int i = 0; i < parsedSeedRanges.Count - 1; i += 2)
      {
         seedRanges.Add((double.Parse(parsedSeedRanges[i].Value), double.Parse(parsedSeedRanges[i+1].Value)));
      }
      return seedRanges;
   }

   private List<double> FindSeedLocations(List<double> seeds)
   {
      List<double> locations = new();
      double currentLocation;

      foreach (double seed in seeds)
      {
         currentLocation = FindDestinationInMap(SeedToSoil, seed);
         currentLocation = FindDestinationInMap(SoilToFertilizer, currentLocation);
         currentLocation = FindDestinationInMap(FertilizerToWater, currentLocation);
         currentLocation = FindDestinationInMap(WaterToLight, currentLocation);
         currentLocation = FindDestinationInMap(LightToTemperature, currentLocation);
         currentLocation = FindDestinationInMap(TemperatureToHumidity, currentLocation);
         currentLocation = FindDestinationInMap(HumidityToLocation, currentLocation);
         locations.Add(currentLocation);
      }
      return locations;
   }

   private double FindDestinationInMap(Dictionary<double, (double, double)> map, double start)
   {
      double destination = -1;

      foreach (double sourceStart in map.Keys)
      {
         if (start >= sourceStart && start <= sourceStart + (map[sourceStart].Item2 - 1))
         {
            destination = map[sourceStart].Item1 + (start - sourceStart);
         }
      }

      if (destination == -1)
      {
         destination = start;
      }

      return destination;
   }

   private List<double> FindSeedRangesLocations(List<(double, double)> seedRanges)
   {
      List<double> locations = new();

      (double startLocation, double endLocation) currentRange = new();
      foreach ((double, double) seedRange in seedRanges)
      {
         currentRange.startLocation = seedRange.Item1;
         currentRange.endLocation = seedRange.Item1 + (seedRange.Item2 - 1);

         currentRange = FindDestinationRangeInMap(SeedToSoil, currentRange);
         currentRange = FindDestinationRangeInMap(SoilToFertilizer, currentRange);
         currentRange = FindDestinationRangeInMap(FertilizerToWater, currentRange);
         currentRange = FindDestinationRangeInMap(WaterToLight, currentRange);
         currentRange = FindDestinationRangeInMap(LightToTemperature, currentRange);
         currentRange = FindDestinationRangeInMap(TemperatureToHumidity, currentRange);
         currentRange = FindDestinationRangeInMap(HumidityToLocation, currentRange);
         locations.Add(currentRange.startLocation);
         locations.Add(currentRange.endLocation);
      }
      return locations;
   }

   private (double, double) FindDestinationRangeInMap(Dictionary<double, (double, double)> map,
      (double, double) range)
   {
      (double, double) destinationRange = new();
      destinationRange.Item1 = FindWithinRangeInMap(map, range.Item1);
      destinationRange.Item2 = FindWithinRangeInMap(map, range.Item2);
      return destinationRange;
   }

   private double FindWithinRangeInMap(Dictionary<double, (double, double)> map, double start)
   {
      // TODO: need to determine if seed/source range spans more than one destination ranges
      // If this happens, then there might be an element within the source range with lower
      // destination (because destination ranges can be inverted, i.e. map a source to a lower number)
      return 0;
   }
}
