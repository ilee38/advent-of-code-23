using System.Text.RegularExpressions;

namespace AdventOfCode23;

public class Day2
{
    private static readonly int maxRedCubes = 12;
    private static readonly int maxGreenCubes = 13;
    private static readonly int maxBlueCubes = 14;
    private int sumOfIds = 0;
    private int sumOfPowers = 0;

    public int ComputeSumOfIds()
    {
        using var streamReader = new StreamReader("Day2/input.txt");
        while (streamReader.Peek() != -1)
        {
            string? line = streamReader.ReadLine();
            if (CheckGameIsPossible(line.Substring(line.IndexOf(":") + 1)))
            {
                sumOfIds += GetDigitFromString(line.Substring(0, 8));
            }
        }
        return sumOfIds;
    }

    public int ComputeSumOfPowers()
    {
        int powerOfMinSet;

        using var streamReader = new StreamReader("Day2/input.txt");
        while (streamReader.Peek() != -1)
        {
            string? line = streamReader.ReadLine();
            powerOfMinSet = ComputeMinSetPower(line.Substring(line.IndexOf(":") + 1));
            sumOfPowers += powerOfMinSet;
        }
        return sumOfPowers;
    }

    private bool CheckGameIsPossible(string gameData)
    {
        int numberOfCubesDrawn;

        var cubeSubsets = gameData.Trim().Split(";");
        foreach (var cubeSubset in cubeSubsets)
        {
            var gameSets = cubeSubset.Split(",");
            foreach (var gameSet in gameSets)
            {
                if (gameSet.Contains("red", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > maxRedCubes)
                    {
                        return false;
                    }
                }
                if (gameSet.Contains("green", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > maxGreenCubes)
                    {
                        return false;
                    }
                }
                if (gameSet.Contains("blue", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > maxBlueCubes)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private int ComputeMinSetPower(string gameData)
    {
        int minSetPower = 1;
        int numberOfCubesDrawn;
        var cubeFrequencies = new Dictionary<string, int>
        {
            {"green", 0},
            {"red", 0},
            {"blue", 0}
        };

        var cubeSubsets = gameData.Trim().Split(";");
        foreach (var cubeSubset in cubeSubsets)
        {
            var gameSets = cubeSubset.Split(",");
            foreach (var gameSet in gameSets)
            {
                if (gameSet.Contains("red", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > cubeFrequencies["red"])
                    {
                        cubeFrequencies["red"] = numberOfCubesDrawn;
                    }
                }
                if (gameSet.Contains("green", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > cubeFrequencies["green"])
                    {
                        cubeFrequencies["green"] = numberOfCubesDrawn;
                    }
                }
                if (gameSet.Contains("blue", StringComparison.OrdinalIgnoreCase))
                {
                    numberOfCubesDrawn = GetDigitFromString(gameSet);
                    if (numberOfCubesDrawn > cubeFrequencies["blue"])
                    {
                        cubeFrequencies["blue"] = numberOfCubesDrawn;
                    }
                }
            }
        }

        foreach (var cubeFrquency in cubeFrequencies)
        {
            minSetPower *= cubeFrquency.Value;
        }

        return minSetPower;
    }

    private int GetDigitFromString(string str)
    {
        // Regex to match a digits sequence inside a string
        Regex digit = new Regex(@"\d+");
        Match match = digit.Match(str);

        return(int.Parse(match.Value));
    }
}
