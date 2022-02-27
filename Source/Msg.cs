﻿using SkyChain;

namespace Revital
{
    /// <summary>
    /// A reportive record of daily transaction for goods.
    /// </summary>
    public class Msg : Info
    {
        public static readonly Msg Empty = new Msg();

        public const short
            TYP_MRT = 1,
            TYP_PRV = 2;

        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_MRT, "市场"},
            {TYP_PRV, "供给"},
        };


        public override void Read(ISource s, short mask = 0xff)
        {
            base.Read(s, mask);
        }

        public override void Write(ISink s, short mask = 0xff)
        {
            base.Write(s, mask);
        }
    }
}