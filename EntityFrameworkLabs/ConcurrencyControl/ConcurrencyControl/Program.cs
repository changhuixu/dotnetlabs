using System;
using System.Threading;
using System.Threading.Tasks;
using ConcurrencyControl.DbContext;
using ConcurrencyControl.Models;
using ConcurrencyControl.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConcurrencyControl
{
    internal class Program
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Information)
                    .AddConsole();
            });

        private static async Task Main()
        {
            MyDbContext.EnsureDatabaseIsCleaned();
            using (var dbContext = new MyDbContext())
            {
                dbContext.Database.Migrate();
                if (dbContext.Database.IsSqlite())
                {
                    await dbContext.Database.ExecuteSqlRawAsync(
                       @"
                            CREATE TRIGGER SetTimestampOnUpdate
                            AFTER UPDATE ON ConcurrentAccountsWithRowVersion
                            BEGIN
                                UPDATE ConcurrentAccountsWithRowVersion
                                SET Timestamp = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END
                        ");
                    await dbContext.Database.ExecuteSqlRawAsync(
                        @"
                            CREATE TRIGGER SetTimestampOnInsert
                            AFTER INSERT ON ConcurrentAccountsWithRowVersion
                            BEGIN
                                UPDATE ConcurrentAccountsWithRowVersion
                                SET Timestamp = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END
                        ");
                }
                await dbContext.NonconcurrentAccounts.AddAsync(new NonconcurrentAccount { Id = 1, Balance = 1000.0m });
                await dbContext.ConcurrentAccountsWithToken.AddAsync(new ConcurrentAccountWithToken { Id = 1, Balance = 1000.0m });
                await dbContext.ConcurrentAccountsWithRowVersion.AddAsync(new ConcurrentAccountWithRowVersion { Id = 1, Balance = 1000.0m });
                await dbContext.SaveChangesAsync();
            }

            Console.WriteLine("========== Concurrency Test with NonConcurrent Account ==============================");
            await TestWithoutConcurrencyControl();

            Console.WriteLine("\n\n========== Concurrency Test with Concurrent Account using Concurrent Token ==========");
            await ConcurrencyControlByConcurrencyToken();

            Console.WriteLine("\n\n========== Concurrency Test with Concurrent Account using Row Version ===============");
            await ConcurrencyControlByRowVersion();
        }

        private static async Task TestWithoutConcurrencyControl()
        {
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.NonconcurrentAccounts.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before): {account.Balance}");
            }

            var threads = new Thread[2];
            threads[0] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.NonconcurrentAccounts.FindAsync(1);
                    account.Credit(100);
                    await dbContext.SaveChangesAsync();
                }
            });
            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.NonconcurrentAccounts.FindAsync(1);
                    account.Debit(200);
                    await dbContext.SaveChangesAsync();
                }
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);     // The purpose of this line is merely to display Console output in sequence.
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.NonconcurrentAccounts.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After): {account.Balance}");
            }
        }

        private static async Task ConcurrencyControlByConcurrencyToken()
        {
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before): {account.Balance}");
            }

            var threads = new Thread[2];
            threads[0] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                    account.Credit(100);
                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        ConsoleUtils.WriteErr(e.Message);
                    }
                }
            });
            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                    account.Debit(200);
                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        ConsoleUtils.WriteErr(e.Message);
                    }
                }
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);     // The purpose of this line is merely to display Console output in sequence.
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After): {account.Balance}");
            }
        }

        private static async Task ConcurrencyControlByRowVersion()
        {
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before): {account.Balance}");
            }

            var threads = new Thread[2];
            threads[0] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                    account.Credit(100);
                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        ConsoleUtils.WriteErr(e.Message);
                    }
                }
            });
            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new MyDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                    account.Debit(200);
                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException e)
                    {
                        ConsoleUtils.WriteErr(e.Message);
                    }
                }
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);     // The purpose of this line is merely to display Console output in sequence.
            using (var dbContext = new MyDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After): {account.Balance}");
            }
        }
    }
}
