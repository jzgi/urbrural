using System;
using System.Data;
using System.Threading.Tasks;
using CoChain;
using CoChain.Web;
using Urbrural.Core;
using static CoChain.Nodal.Store;
using static CoChain.Web.Modal;

namespace Urbrural
{
    public abstract class OrgVarWork : WebWork
    {
        protected async Task doimg(WebContext wc, string col)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").T(col).T(" FROM orgs WHERE id = @1");
                if (dc.QueryTop(p => p.Set(id)))
                {
                    dc.Let(out byte[] bytes);
                    if (bytes == null) wc.Give(204); // no content 
                    else wc.Give(200, new StaticContent(bytes), shared: false, 60);
                }
                else
                    wc.Give(404, shared: true, maxage: 3600 * 24); // not found
            }
            else // POST
            {
                var f = await wc.ReadAsync<Form>();
                ArraySegment<byte> img = f[nameof(img)];
                using var dc = NewDbContext();
                dc.Sql("UPDATE orgs SET ").T(col).T(" = @1 WHERE id = @2");
                if (await dc.ExecuteAsync(p => p.Set(img).Set(id)) > 0)
                {
                    wc.Give(200); // ok
                }
                else
                    wc.Give(500); // internal server error
            }
        }
    }


    public class AdmlyOrgVarWork : OrgVarWork
    {
        public async Task @default(WebContext wc, int typ)
        {
            short id = wc[0];
            var prin = (User) wc.Principal;
            var regs = Grab<short, MvScene>();
            var orgs = Grab<int, Org>();
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE id = @1");
                var m = dc.QueryTop<Org>(p => p.Set(id));
                wc.GivePane(200, h =>
                {
                    var typname = Org.Typs[m.typ];
                    h.FORM_().FIELDSUL_(typname + "????????????");
                    h.LI_().TEXT(typname + "??????", nameof(m.name), m.name, min: 2, max: 12, required: true)._LI();
                    h.LI_().TEXTAREA("??????", nameof(m.tip), m.tip, max: 30)._LI();
                    h.LI_().SELECT("????????????", nameof(m.regid), m.regid, regs)._LI();
                    h.LI_().TEXT("??????", nameof(m.addr), m.addr, max: 20)._LI();
                    h.LI_().NUMBER("??????", nameof(m.x), m.x, min: 0.000, max: 180.000).NUMBER("??????", nameof(m.y), m.y, min: -90.000, max: 90.000)._LI();
                    h.LI_().SELECT("??????", nameof(m.state), m.state, Entity.States, filter: (k, v) => k > 0)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else // POST
            {
                var m = await wc.ReadObjectAsync(Entity.DUAL, new Org
                {
                    typ = (short) typ,
                    adapted = DateTime.Now,
                    adapter = prin.name
                });
                using var dc = NewDbContext();
                dc.Sql("UPDATE orgs")._SET_(Org.Empty, Entity.DUAL).T(" WHERE id = @1");
                dc.Execute(p =>
                {
                    m.Write(p, Entity.DUAL);
                    p.Set(id);
                });
                wc.GivePane(200); // close
            }
        }

        [Ui("???", "???????????????", group: 7), Tool(ButtonOpen, Appear.Small)]
        public async Task mgr(WebContext wc, int cmd)
        {
            if (wc.IsGet)
            {
                string tel = wc.Query[nameof(tel)];
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("????????????");
                    h.LI_("uk-flex").TEXT("????????????", nameof(tel), tel, pattern: "[0-9]+", max: 11, min: 11, required: true).BUTTON("??????", nameof(mgr), 1, post: false)._LI();
                    h._FIELDSUL();
                    if (cmd == 1) // search user
                    {
                        using var dc = NewDbContext();
                        dc.Sql("SELECT ").collst(User.Empty).T(" FROM users WHERE tel = @1");
                        var o = dc.QueryTop<User>(p => p.Set(tel));
                        if (o != null)
                        {
                            h.FIELDSUL_();
                            h.HIDDEN(nameof(o.id), o.id);
                            h.LI_().FIELD("?????????", o.name)._LI();
                            h._FIELDSUL();
                            h.BOTTOMBAR_().BUTTON("??????", nameof(mgr), 2)._BOTTOMBAR();
                        }
                    }
                    h._FORM();
                });
            }
            else // POST
            {
                int orgid = wc[0];
                int id = (await wc.ReadAsync<Form>())[nameof(id)];
                using var dc = NewDbContext(IsolationLevel.ReadCommitted);
                dc.Execute("UPDATE orgs SET mgrid = @1 WHERE id = @2", p => p.Set(id).Set(orgid));
                dc.Execute("UPDATE users SET orgid = @1, orgly = 15 WHERE id = @2", p => p.Set(orgid).Set(id));
                wc.GivePane(200); // ok
            }
        }

        [Ui("???", "????????????", group: 7), Tool(ButtonCrop, Appear.Small)]
        public async Task icon(WebContext wc)
        {
            await doimg(wc, nameof(icon));
        }

        [Ui("???", "????????????", group: 7), Tool(ButtonCrop, Appear.Large)]
        public async Task cert(WebContext wc)
        {
            await doimg(wc, nameof(cert));
        }
    }


    public class MrtlyOrgVarWork : OrgVarWork
    {
    }

    public class SrclyOrgVarWork : OrgVarWork
    {
        public async Task @default(WebContext wc)
        {
            int id = wc[0];
            var regs = Grab<short, MvScene>();
            if (wc.IsGet)
            {
                using var dc = NewDbContext();
                dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE id = @1");
                var m = await dc.QueryTopAsync<Org>(p => p.Set(id));
                wc.GivePane(200, h =>
                {
                    h.FORM_().FIELDSUL_("????????????");
                    h.LI_().TEXT("????????????", nameof(m.name), m.name, max: 10, required: true)._LI();
                    h.LI_().TEXTAREA("??????", nameof(m.tip), m.tip, max: 30)._LI();
                    h.LI_().SELECT("????????????", nameof(m.fork), m.fork, Org.Forks, required: true)._LI();
                    h.LI_().SELECT("????????????", nameof(m.regid), m.regid, regs, filter: (k, v) => v.IsProv, required: true)._LI();
                    h.LI_().TEXT("??????", nameof(m.addr), m.addr, max: 20)._LI();
                    h.LI_().TEXT("??????", nameof(m.tel), m.tel, pattern: "[0-9]+", max: 11, min: 11, required: true);
                    h.LI_().SELECT("??????", nameof(m.state), m.state, Entity.States)._LI();
                    h._FIELDSUL()._FORM();
                });
            }
            else
            {
                var m = await wc.ReadObjectAsync<Org>(0);
                using var dc = NewDbContext();
                dc.Sql("UPDATE orgs")._SET_(Org.Empty, 0).T(" WHERE id = @1");
                dc.Execute(p =>
                {
                    m.Write(p, 0);
                    p.Set(id);
                });
                wc.GivePane(200); // close
            }
        }

        [Ui("???", "??????"), Tool(ButtonShow, Appear.Small)]
        public async Task rm(WebContext wc)
        {
            int id = wc[0];
            if (wc.IsGet)
            {
                const bool ok = true;
                wc.GivePane(200, h =>
                {
                    h.ALERT("?????????????????????");
                    h.FORM_().HIDDEN(nameof(ok), ok)._FORM();
                });
            }
            else
            {
                using var dc = NewDbContext();
                dc.Sql("DELETE FROM orgs WHERE id = @1 AND typ = ").T(Org.TYP_STA);
                await dc.ExecuteAsync(p => p.Set(id));

                wc.GivePane(200);
            }
        }
    }
}