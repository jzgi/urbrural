﻿using SkyChain;

namespace Revital
{
    /// <summary>
    /// The data modal for an standard item of product or service.
    /// </summary>
    public class Item : _Article, IKeyable<short>
    {
        public static readonly Item Empty = new Item();

        public const short
            TYP_AGRI = 1,
            TYP_DIETARY = 2,
            TYP_FACTORY = 3,
            TYP_CARE = 4,
            TYP_CHARITY = 5,
            TYP_AD = 6;


        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_AGRI, "生态农产"},
            {TYP_DIETARY, "调养饮食"},
            {TYP_FACTORY, "工业制品"},
            {TYP_CARE, "家政医养"},
            {TYP_CHARITY, "公益志愿"},
            {TYP_AD, "广告宣传"},
        };


        // types
        public static readonly Map<short, string> Cats = new Map<short, string>
        {
            // agri
            {11, "蔬菜"},
            {12, "瓜果"},
            {13, "粮油"},
            {14, "绿植"},
            {15, "禽产"},
            {16, "畜产"},
            {17, "水产"},
            {18, "副产"},
            {19, "农资"},
            // dietary
            {21, "调养"},
            {22, "餐品"},
            {23, "快餐"},
            // factory
            {31, "日用"},
            {31, "家居"},
            {31, "服装"},
            {31, "电器"},
            // care
            {41, "家政"},
            {42, "陪护"},
            {43, "养老"},
            // charity
            {51, "公益"},
            {52, "志愿"},
            // ad
            {61, "广告"},
            {62, "宣传"},
        };


        internal short id;
        internal short cat;
        internal string unit; // basic unit
        internal string unitip;

        // must have an icon

        public override void Read(ISource s, byte proj = 0x0f)
        {
            base.Read(s, proj);

            if ((proj & ID) == ID)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(cat), ref cat);
            s.Get(nameof(unit), ref unit);
            s.Get(nameof(unitip), ref unitip);
        }

        public override void Write(ISink s, byte proj = 0x0f)
        {
            base.Write(s, proj);

            if ((proj & ID) == ID)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(cat), cat);
            s.Put(nameof(unit), unit);
            s.Put(nameof(unitip), unitip);
        }

        public short Key => id;

        public override string ToString() => name;
    }
}