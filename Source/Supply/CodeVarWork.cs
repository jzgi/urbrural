﻿using System.Threading.Tasks;
using SkyChain.Web;

namespace Revital.Supply
{
    public class PublyCodeVarWork : WebWork
    {
        public async Task @default(WebContext wc, int page)
        {
            int code = wc[0];

            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(Subscrib.Empty).T(" FROM purchs WHERE @1 BETWEEN start AND end");
            var o = dc.QueryTop<Subscrib>(p => p.Set(code));


            var prod = Obtain<short, Supply>(o.prodid);
            var src = Obtain<short, Org>(o.partyid);
            var ctr = Obtain<short, Org>(o.ctrid);
        }
    }
}