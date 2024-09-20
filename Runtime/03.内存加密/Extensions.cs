// *********************************************************************************
// # Project: Forest
// # Unity: 2022.3.5f1c1
// # Author: jinyijie
// # Version: 1.0.0
// # History: 2024-09-03  15:09
// # Copyright: 2024, jinyijie
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public static partial class Extensions
    {
        public static List<T> ToList<T>(this List<Variable<T>> variables)
        {
            return variables?.Select(variable => variable.Value).ToList();
        }

        public static T[] ToArray<T>(this Variable<T>[] variables)
        {
            return variables?.Select(variable => variable.Value).ToArray();
        }
    }
}