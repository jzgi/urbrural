using System;
using CoChain;

namespace Urbrural
{
    public class Clear : Entity, IKeyable<int>
    {
        public static readonly Clear Empty = new Clear();

        public const short
            TYP_BUY = 1,
            TYP_BOOK = 2;


        public static readonly Map<short, string> Typs = new Map<short, string>
        {
            {TYP_BUY, "零售"},
            {TYP_BOOK, "订货"},
        };

        public const short
            STA_ = 0,
            STA_APPROVED = 2,
            STA_PAID = 3;


        public new static readonly Map<short, string> Statuses = new Map<short, string>
        {
            {STA_, "新结算"},
            {STA_APPROVED, "已确认"},
            {STA_PAID, "已支付"},
        };

        internal int id;
        internal int orgid;
        internal DateTime dt;
        internal short count;
        internal decimal amt;
        internal int qty;

        public override void Read(ISource s, short msk = 0xff)
        {
            base.Read(s, msk);

            if ((msk & EXTRA) == EXTRA)
            {
                s.Get(nameof(id), ref id);
            }
            s.Get(nameof(orgid), ref orgid);
            s.Get(nameof(dt), ref dt);
            s.Get(nameof(count), ref count);
            s.Get(nameof(amt), ref amt);
            s.Get(nameof(qty), ref qty);
        }

        public override void Write(ISink s, short msk = 0xff)
        {
            base.Write(s, msk);

            if ((msk & EXTRA) == EXTRA)
            {
                s.Put(nameof(id), id);
            }
            s.Put(nameof(orgid), orgid);
            s.Put(nameof(dt), dt);
            s.Put(nameof(count), count);
            s.Put(nameof(amt), amt);
            s.Put(nameof(qty), qty);
        }

        public int Key => id;
    }
}