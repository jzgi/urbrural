using System.Threading.Tasks;
using CoChain;
using CoChain.Web;
using Urbrural;
using Urbrural.Core;
using static CoChain.Nodal.Store;
using static CoChain.Web.Modal;

namespace Urbrural
{
    public abstract class RegVarWork : WebWork
    {
    }

    public class AdmlyRegVarWork : RegVarWork
    {
        [Ui(group: 7), Tool(ButtonShow)]
        public async Task @default(WebContext wc, int typ)
        {
            short id = wc[0];
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(MvScene.Empty).T(" FROM regs WHERE id = @1");
                var o = await dc.QueryTopAsync<MvScene>(p => p.Set(id));
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("区域属性");
                    h.LI_().NUMBER("区域编号", nameof(o.id), o.id, min: 1, max: 99, required: true)._LI();
                    h.LI_().SELECT("类型", nameof(o.typ), o.typ, MvScene.Typs, filter: (k, v) => k == typ, required: true)._LI();
                    h.LI_().TEXT("名称", nameof(o.name), o.name, min: 2, max: 10, required: true)._LI();
                    h.LI_().NUMBER("排序", nameof(o.idx), o.idx, min: 1, max: 99)._LI();
                    h.LI_().SELECT("状态", nameof(o.state), o.state, Entity.States)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else
            {
                var o = await wc.ReadObjectAsync<MvScene>();
                using var dc = NewDbContext();
                dc.Sql("UPDATE regs")._SET_(o).T(" WHERE id = @1");
                await dc.ExecuteAsync(p =>
                {
                    o.Write(p);
                    p.Set(id);
                });
                wc.GivePane(200);
            }
        }

        [Ui("✕", "删除", group: 7), Tool(ButtonShow, Appear.Small)]
        public async Task rm(WebContext wc)
        {
            short id = wc[0];
            if (wc.IsGet)
            {
                const bool ok = true;
                wc.GivePane(200, h =>
                {
                    h.ALERT("确定删除此项？");
                    h.FORM_().HIDDEN(nameof(ok), ok)._FORM();
                });
            }
            else
            {
                using var dc = NewDbContext();
                dc.Sql("DELETE FROM regs WHERE id = @1");
                await dc.ExecuteAsync(p => p.Set(id));

                wc.GivePane(200);
            }
        }
    }
}