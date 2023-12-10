using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public static class PatternFinder
    {

        public static PatternDataResults GetPatternDataFromGrid<T>(ValuesManager<T> valuesManager, int patternSize,
            bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashCodeDictionary = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();

            Vector2 sizeOfGrid = valuesManager.GetGridSize();

            int patternGridSizeX = 0, patternGridSizeY = 0;
            int rowMin = -1, rowMax = -1, colMin = -1, colMax = -1;
            if (patternSize < 3)
            {
                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeY = (int)sizeOfGrid.y + 3 - patternSize;
                rowMax = patternGridSizeY - 1;
                colMax = patternGridSizeX - 1;
            }
            else
            {
                patternGridSizeX = (int)sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = (int)sizeOfGrid.y + patternSize - 1;
                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;
                rowMax = (int)sizeOfGrid.y;
                colMax = (int)sizeOfGrid.x;
            }

            int[][] patternIndicesGrid =
                MyCollectionExtension.CreateJaggedArray<int[][]>(patternGridSizeY, patternGridSizeX);
            int totalFrequency = 0, patternIndex = 0;

            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = colMin; col < colMax; col++)
                {
                    int[][] gridValues = valuesManager.GetPatternValuesFromGridAt(col, row, patternSize);
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if (!patternHashCodeDictionary.ContainsKey(hashValue))
                    {
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashCodeDictionary, patternIndexDictionary, hashValue, pattern);
                    }
                    else
                    {
                        if (!equalWeights)
                        {
                            patternIndexDictionary[patternHashCodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }
                    
                    totalFrequency++;

                    if (patternSize<3)
                    {
                        patternIndicesGrid[row + 1][col + 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                    else
                    {
                        patternIndicesGrid[row + patternSize - 1][col = patternSize - 1] =
                            patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                }


            }

            CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);
            return new PatternDataResults(patternIndicesGrid, patternIndexDictionary);
        }

        private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
        {
            foreach (var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }

        private static void AddNewPattern(Dictionary<string, PatternData> patternHashCodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
        {
            PatternData patternData = new PatternData(pattern);
            patternHashCodeDictionary.Add(hashValue, patternData);
            patternIndexDictionary.Add(pattern.Index, patternData);
        }

        public static Dictionary<int, PatternNeighbours> FindPossibleNeighboursForAllPatterns(IFindNeighbourStrategy strategy, PatternDataResults patternFinderResult)
        {
            return strategy.FindNeighbours(patternFinderResult);
        }

        public static PatternNeighbours CheckNeighboursInEachDirection(int x, int y,
            PatternDataResults patternDataResults)
        {
            PatternNeighbours patternNeighbours = new PatternNeighbours();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResults.GetNeighbourInDirection(x, y, dir);
                if (possiblePatternIndex > 0)
                {
                    patternNeighbours.AddPatternToDictionary(dir, possiblePatternIndex);
                }
            }

            return patternNeighbours;
        }

        public static void AddNeighboursToDictionary(Dictionary<int, PatternNeighbours> dictionary, int patternIndex,
            PatternNeighbours neighbours)
        {
            if (!dictionary.ContainsKey(patternIndex))
            {
                dictionary.Add(patternIndex, neighbours);
            }
            else
            {
                dictionary[patternIndex].AddNeighbour(neighbours);
            }
        }
    }
}
