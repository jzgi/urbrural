﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SkyChain;
using Zhnt.Mart;
using Zhnt.Supply;
using static System.Data.IsolationLevel;

namespace Zhnt
{
    public class ZhntApp : ServerEnviron
    {
        // periodic polling and concluding ended lots 
        static readonly Thread cycler = new Thread(Cycle);

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        public static async Task Main(string[] args)
        {
// var s = SkyiahComUtility.ComputeCredential("13870639072", "123");

            LoadConfig();

            CacheUp();

            // prepare chain
            // await StartChainAsync();

            // start the concluder thead
            // cycler.Start();

            // prepare web
            MakeWebService<ZhntService>("main");
            await StartWebAsync();
        }


        public static void CacheUp()
        {
            MakeObjectCache(dc =>
                {
                    dc.Sql("SELECT ").collst(Reg.Empty).T(" FROM regs ORDER BY id");
                    return dc.Query<short, Reg>();
                }, 3600 * 24
            );

            MakeDictionaryCache<short, Map<short, Biz>>((dc, bizid) =>
                {
                    dc.Sql("SELECT ").collst(Biz.Empty).T(" FROM bizs_vw WHERE status > 0 ORDER BY id");
                    return dc.Query<short, Biz>();
                }, 900
            );

            MakeObjectCache(dc =>
                {
                    dc.Sql("SELECT ").collst(Org.Empty).T(" FROM orgs_vw WHERE status > 0 ORDER BY id");
                    return dc.Query<short, Org>();
                }, 900
            );
        }

        static async void Cycle(object state)
        {
            var orgs = Fetch<Map<short, Org>>();
            var lst = new List<int>(64);
            while (true)
            {
                Thread.Sleep(60 * 1000);

                var today = DateTime.Today;
                // WAR("cycle: " + today);

                // to succeed
                lst.Clear();
                try
                {
                    using (var dc = NewDbContext())
                    {
                        dc.Sql("SELECT id FROM lots WHERE status = ").T(_Doc.STATUS_DRAFT).T(" AND ended < @1 AND qtys >= min");
                        await dc.QueryAsync(p => p.Set(today));
                        while (dc.Next())
                        {
                            dc.Let(out int id);
                            lst.Add(id);
                        }
                    }
                    foreach (var lotid in lst)
                    {
                        using var dc = NewDbContext(ReadCommitted);
                        try
                        {
                            if (!await dc.SucceedLotAsync(lotid, orgs))
                            {
                                dc.Rollback();
                            }
                        }
                        catch (Exception e)
                        {
                            dc.Rollback();
                            ERR(e.Message);
                        }
                    }

                    // to abort
                    lst.Clear();
                    using (var dc = NewDbContext())
                    {
                        dc.Sql("SELECT id FROM lots WHERE status = ").T(_Doc.STATUS_DRAFT).T(" AND ended < @1 AND qtys < min");
                        await dc.QueryAsync(p => p.Set(today));
                        while (dc.Next())
                        {
                            dc.Let(out int id);
                            lst.Add(id);
                        }
                    }
                    foreach (var lotid in lst)
                    {
                        using var dc = NewDbContext(ReadCommitted);
                        try
                        {
                            if (!await dc.AbortLotAsync(lotid, "拼团失败", orgs))
                            {
                                dc.Rollback();
                            }
                        }
                        catch (Exception e)
                        {
                            dc.Rollback();
                            ERR(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    ERR(nameof(Cycle) + ": " + e.Message);
                }
            }
        }
    }
}