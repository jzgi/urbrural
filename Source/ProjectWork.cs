using System.Threading.Tasks;
using CoChain.Web;
using Urbrural;
using Urbrural.Core;
using static CoChain.Nodal.Store;
using static CoChain.Web.Modal;

namespace Urbrural
{
    public abstract class ProjectWork : WebWork
    {
    }

    public class PublyProjectWork : ProjectWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<PublyProjectVarWork>();
        }

        public void @default(WebContext wc, int page)
        {
        }
    }

    [Ui("商户线上货架设置", "thumbnails")]
    public class OrglyProjectWork : ProjectWork
    {
        protected override void OnCreate()
        {
            CreateVarWork<OrglyProjectVarWork>();
        }

        [Ui("上架", group: 1), Tool(Anchor)]
        public async Task @default(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(MvProj.Empty).T(" FROM pieces WHERE orgid = @1 AND status >= 2 ORDER BY status DESC");
            var arr = await dc.QueryAsync<MvProj>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    h.TD_().A_TEL(o.name, o.tip)._TD();
                    // h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }

        [Ui("下架", group: 2), Tool(Anchor)]
        public async Task off(WebContext wc)
        {
            var org = wc[-1].As<Org>();
            using var dc = NewDbContext();
            dc.Sql("SELECT ").collst(MvProj.Empty).T(" FROM peices WHERE orgid = @1 AND status <= 1 ORDER BY status DESC");
            var arr = await dc.QueryAsync<MvProj>(p => p.Set(org.id));
            wc.GivePage(200, h =>
            {
                h.TOOLBAR();
                h.TABLE(arr, o =>
                {
                    // h.TD_().TOOLVAR(o.Key, nameof(BizlyPieceVarWork.upd), caption: o.name).SP()._TD();
                    // h.TD(o.price, true);
                    // h.TD(Statuses[o.status]);
                });
            });
        }
    }
}