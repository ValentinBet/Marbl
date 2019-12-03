

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MarblFactory
{
    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public static List<Transform> ShuffleList(List<Transform> _myList)
    {
        for (int i = 0; i < _myList.Count; i++)
        {
            Transform temp = _myList[i];
            int randomIndex = UnityEngine.Random.Range(i, _myList.Count);
            _myList[i] = _myList[randomIndex];
            _myList[randomIndex] = temp;
        }

        return _myList;
    }
}