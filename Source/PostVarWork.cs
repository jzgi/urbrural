using System;
using System.Data;
using System.Threading.Tasks;
using CoChain;
using CoChain.Web;
using static CoChain.Nodal.Store;
using static CoChain.Web.WebApp;
using static CoChain.Web.Modal;

namespace Urbrural
{
    public class PostVarWork : WebWork
    {
    }

    public abstract class BizlyPostVarWork : PostVarWork
    {
    }

    public class AgriBizlyPostVarWork : BizlyPostVarWork
    {
        [Ui, Tool(ButtonOpen)]
        public async Task act(WebContext wc, int cmd)
        {
            int lotid = wc[0];
            short orgid = wc[-2];
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                // dc.Sql("SELECT ").collst(Book.Empty).T(" FROM lots_vw WHERE id = @1");
                // var m = await dc.QueryTopAsync<Book>(p => p.Set(lotid));
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                int[] key = f[nameof(key)];
                if (cmd == 1)
                {
                    using var dc = NewDbContext(IsolationLevel.ReadCommitted);
                    try
                    {
                    }
                    catch (Exception e)
                    {
                        Err(e.Message);
                        dc.Rollback();
                    }
                }
                else
                {
                }

                wc.GiveRedirect(nameof(act));
            }
        }

        [Ui("修改", @group: 1), Tool(ButtonOpen)]
        public async Task upd(WebContext wc)
        {
            var prin = (User) wc.Principal;
            short orgid = wc[-2];
            int id = wc[0];
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                // dc.Sql("SELECT ").collst(Book.Empty).T(" FROM lots_vw WHERE id = @1");
                // var m = await dc.QueryTopAsync<Book>(p => p.Set(id));
            }
            else // POST
            {
                wc.GivePane(201);
            }
        }

        [Ui("图标", @group: 1), Tool(ButtonCrop, Appear.Small)]
        public async Task icon(WebContext wc)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ArraySegment<byte> img = f[nameof(img)];
                using var dc = NewDbContext();
                if (await dc.ExecuteAsync("UPDATE lots SET icon = @1 WHERE id = @2", p => p.Set(img).Set(id)) > 0)
                {
                    wc.Give(200); // ok
                }
                else wc.Give(500); // internal server error
            }
        }

        [Ui("图片", @group: 1), Tool(ButtonCrop, Appear.Small)]
        public async Task img(WebContext wc)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ArraySegment<byte> img = f[nameof(img)];
                using var dc = NewDbContext();
                if (await dc.ExecuteAsync("UPDATE lots SET img = @1 WHERE id = @2", p => p.Set(img).Set(id)) > 0)
                {
                    wc.Give(200); // ok
                }
                else wc.Give(500); // internal server error
            }
        }

        // [Ui("核实"), Tool(Modal.ButtonShow)]
    }

    public class DietBizlyPostVarWork : BizlyPostVarWork
    {
    }

    public abstract class CtrlyPostVarWork : PostVarWork
    {
    }

    public class CtrlyAgriPostVarWork : CtrlyPostVarWork
    {
    }

    public class DietCtrlyPostVarWork : CtrlyPostVarWork
    {
    }


    public class FrmlyPostVarWork : PostVarWork
    {
    }
}