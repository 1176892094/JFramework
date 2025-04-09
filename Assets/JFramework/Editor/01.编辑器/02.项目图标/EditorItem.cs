// // *********************************************************************************
// // # Project: JFramework
// // # Unity: 6000.3.5f1
// // # Author: 云谷千羽
// // # Version: 1.0.0
// // # History: 2025-04-09 19:04:33
// // # Recently: 2025-04-09 19:04:33
// // # Copyright: 2024, 云谷千羽
// // # Description: This is an automatically generated comment.
// // *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    internal static class EditorItem
    {
         private static readonly Dictionary<Item, Lazy<string>> cacheItem = new Dictionary<Item, Lazy<string>>
        {
            { Item.Normal, new Lazy<string>(() => "iVBORw0KGgoAAAANSUhEUgAAAgAAAABACAYAAABsv8+/AAAETUlEQVR4Ae3dwa0aQRAEUK9FFE4D0nAUI0EapAFpOA1Iw3FYvtaBw/SpxPu3Fmqp+81fbWkvc6y1fvgjQIAAAQIEvkvg53eta1sCBAgQIEDgv4AA4P+AAAECBAh8oYAA8IWHbmUCBAgQIHAqIPgVM/6Nur08xwLvqNvL37HAn6jby2ss8Iy6vbzHAlnHz3XlIya+Rd1e5vOWz2P7fq9Y4BJ1e5nvu3wfjvbzBWDEp5kAAQIECHQKCACd52ZqAgQIECAwEhAARnyaCRAgQIBAp4AA0HlupiZAgAABAiMBAWDEp5kAAQIECHQKCACd52ZqAgQIECAwEhAARnyaCRAgQIBAp8DhLoDOgzM1AQIECBCYCPgCMNHTS4AAAQIESgUEgNKDMzYBAgQIEJgICAATPb0ECBAgQKBUQAAoPThjEyBAgACBiYAAMNHTS4AAAQIESgUEgNKDMzYBAgQIEJgICAATPb0ECBAgQKBU4FQwd95/nPcjF6zwccRz/PqOur3M+8fzfvL2/a6xwDPq9vIeC2QdP9eVj5j4FnV7mc9bPo/t+71igUvU7WW+7/J9ONrPF4ARn2YCBAgQINApIAB0npupCRAgQIDASEAAGPFpJkCAAAECnQICQOe5mZoAAQIECIwEBIARn2YCBAgQINApIAB0npupCRAgQIDASEAAGPFpJkCAAAECnQLHWqtzclMTIECAAAEC2wK+AGzTaSRAgAABAr0CAkDv2ZmcAAECBAhsCwgA23QaCRAgQIBAr4AA0Ht2JidAgAABAtsCAsA2nUYCBAgQINArIAD0np3JCRAgQIDAtoAAsE2nkQABAgQI9AqcCkbP+4/zfuSCFT6OeI5f31G3l3n/eN5P3r7fNRZ4Rt1e3mOBrOPnuvIRE9+ibi/zecvnsX2/Vyxwibq9zPddvg9H+/kCMOLTTIAAAQIEOgUEgM5zMzUBAgQIEBgJCAAjPs0ECBAgQKBTQADoPDdTEyBAgACBkYAAMOLTTIAAAQIEOgUEgM5zMzUBAgQIEBgJCAAjPs0ECBAgQKBT4FhrdU5uagIECBAgQGBbwBeAbTqNBAgQIECgV0AA6D07kxMgQIAAgW0BAWCbTiMBAgQIEOgVEAB6z87kBAgQIEBgW0AA2KbTSIAAAQIEegUEgN6zMzkBAgQIENgWEAC26TQSIECAAIFegVPB6Hn/cd6PXLDCxxHP8es76vYy7x/P+8nb97vGAs+o28t7LJB1/FxXPmLiW9TtZT5v+Ty27/eKBS5Rt5f5vsv34Wg/XwBGfJoJECBAgECngADQeW6mJkCAAAECIwEBYMSnmQABAgQIdAoIAJ3nZmoCBAgQIDASEABGfJoJECBAgECngADQeW6mJkCAAAECIwEBYMSnmQABAgQIdAoca63OyU1NgAABAgQIbAv4ArBNp5EAAQIECPQKCAC9Z2dyAgQIECCwLSAAbNNpJECAAAECvQL/AG/bKlGRkrW/AAAAAElFTkSuQmCC") },
            { Item.Height, new Lazy<string>(() => "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA8UlEQVR4Ae2ZQQrDMBADm9Kf+v9vaOnVh0BAgkx2cltihCyNffGx1npN/t6TN//fuwFIwPAEPALDAfAS/NyAgO/m4djm6ugdUI0XIC4BgJKqFiWgGi9AXAIAJVUtSkA1XoC4BABKqlqUgGq8AHEJAJRUtSgB1XgB4hIAKKlqUQKq8QLExxNwh3eBnZP9nWD/f3U+fWcYT4ABXOXpaevveAecntl0AR6BdKI0PQmgNZb2KwHpRGl6EkBrLO1XAtKJ0vQkgNZY2q8EpBOl6UkArbG0XwlIJ0rTkwBaY2m/EpBOlKYnAbTG0n4lIJ0oTW88AT8muwOwK+8yWQAAAABJRU5ErkJggg==") },
            { Item.Middle, new Lazy<string>(() => "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA8klEQVR4Ae2aMQ6EMADDAPHT/v8Nd2OlDgwo0Z1VM8GAiRLDxDnGOH58fJbnn8t19fKq0gFwCwCMVI2oAdV6AXANAIxUjagB1XoBcA0AjFSNqAHVegFwDQCMVI2oAdV6AXANAIxUjagB1XoBcA0AjFSNqAHVegFwDQCMVI2oAdV6AXANAIxUjXhX6e/g6/8C7yjzrsf/DXwFZlF7nm1vwD9+Ax7f2bSn2xtgAWmlaDwNoC2WzqsB6UZpPA2gLZbOqwHpRmk8DaAtls6rAelGaTwNoC2WzqsB6UZpPA2gLZbOqwHpRmk8DaAtls6rAelGabwvvrQDrtVOP8MAAAAASUVORK5CYII=") },
            { Item.Bottom, new Lazy<string>(() => "iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAA70lEQVR4Ae2aMQ5AQAAEET+9/7+BUnKFQjKxmxsVhbHZHSr7GGP7+bim5+/TNXp5oPQCuAUUjIRG1AC03gK4BhSMhEbUALTeArgGFIyERtQAtN4CuAYUjIRG1AC03gK4BhSMhEbUALTeArgGFIyERtQAtN4CuAYUjIRG1AC03gK4BhSMhEY8Ufo3+Py/wDfKc9fr/wa+Ak9Ra54tb0DCN+D1HaW9XN4AC6AVS+drQPpCdD4NoBtO52tA+kJ0Pg2gG07na0D6QnQ+DaAbTudrQPpCdD4NoBtO52tA+kJ0Pg2gG07na0D6QnQ+DaAbTuffvsQDriqTua4AAAAASUVORK5CYII=") },
        };
         
        private static readonly Dictionary<Item, Texture2D> usingItem = new Dictionary<Item, Texture2D>();

        public static Texture2D GetImage(Item type)
        {
            return EditorTool.GetIcon(type, cacheItem, usingItem);
        }
    }

    internal enum Item
    {
        Normal,
        Bottom,
        Middle,
        Height,
    }
}