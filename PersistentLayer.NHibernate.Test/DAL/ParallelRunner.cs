using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NHibernate;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.DAL
{

    public class ParallelRunner
        : CurrentTester
    {
        //[Test]
        [ExpectedException(typeof(SessionNotBindedException))]
        public void WrongRunInParallel()
        {
            Thread t1 = new Thread(() => Run(0, 3));

            t1.Start();
            t1.Join();
        }

        public void Run(int startIndex, int pageSize)
        {
            lock (this.CurrentPagedDAO)
            {
                var result = this.CurrentPagedDAO.GetPagedResult<Salesman>(startIndex, pageSize, salesman => salesman.ID > 0);
                Assert.IsNotNull(result.Result);
            }
        }

        [Test]
        public void RunInParallel()
        {
            try
            {
                StringBuilder buffer = new StringBuilder();
                Thread t1 = new Thread(() => Run2(0, 3, buffer));
                Thread t2 = new Thread(() => Run2(1, 3, buffer));
                Thread t3 = new Thread(() => Run2(2, 3, buffer));

                t1.Start();
                t2.Start();
                t3.Start();

                t1.Join();
                t2.Join();
                t3.Join();

                foreach (var res in buffer.ToString().Split(new[] { @"\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Console.WriteLine(res);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ciao");
                throw;
            }
        }


        public void Run2(int startIndex, int pageSize, StringBuilder buffer)
        {
            using (var sss = new SessionContextProvider(this.SessionFactory.OpenSession))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(sss);
                var result = dao.GetPagedResult<Salesman>(startIndex, pageSize, salesman => salesman.ID > 0);
                Assert.IsNotNull(result.Result);
                lock (buffer)
                {
                    buffer.AppendLine("Begin writing on buffer");
                    foreach (var res in result.Result)
                    {
                        buffer.AppendLine(res.ToString());
                    }
                }
            }
        }

    }
}
