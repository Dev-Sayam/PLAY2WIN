using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public static class MyExtensions
{
    public static string Extend(this Array array)
    {
        return "Yes, you can extend an array";
    }

    public static T[] column<T>(this T[,] multidimArray, int wanted_column)
    {
        int l = multidimArray.GetLength(0);
        T[] columnArray = new T[l];
        for (int i = 0; i < l; i++)
        {
            columnArray[i] = multidimArray[i, wanted_column];
        }
        return columnArray;
    }

    public static T[] row<T>(this T[,] multidimArray, int wanted_row)
    {
        int l = multidimArray.GetLength(1);
        T[] rowArray = new T[l];
        for (int i = 0; i < l; i++)
        {
            rowArray[i] = multidimArray[wanted_row, i];
        }
        return rowArray;
    }
    public static T[,] Make2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }
}
