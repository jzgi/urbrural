using System;
using CoChain;
using Urbrural.Core;

namespace Urbrural.Scenes
{
    /// <summary>
    /// An environment for project execution.
    /// </summary>
    public class MarketScene : MvScene, IKeyable<short>
    {
        readonly short id;

        readonly string _label;

        public Action DoLayout;

        public MarketScene()
        {
        }

        MarketScene(string id, string label)
        {
            id = id;
            _label = label;
        }

        public short Key => id;

        public string Label => _label;

        //
        // processing cycles and logics


        // public static Map<string, Sector> All = new Map<string, Sector>()
        // {
        //     new Sector("land", "田园种植"),
        //     new Sector("grnhs", "温室栽培"),
        //     new Sector("orchard", "生态果园"),
        //     new Sector("", "鱼菜共生"),
        //     new Sector("", "生态畜牧"),
        //     new Sector("cardon", "碳汇林业"),
        //     new Sector("art", "田园文化艺术"),
        //     new Sector("ggrid", "绿色电力"),
        // };
    }
}