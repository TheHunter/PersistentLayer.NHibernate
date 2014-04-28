using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.Sessions
{
    public class CacheProviderTest
        : CurrentTester
    {
        private ISession internalSession = null;

        [Test]
        public void SessionCacheProviderTest()
        {
            using (var tr = new SessionCacheProvider(this.SessionFactory.OpenSession, false, false))
            {
                Assert.AreEqual(tr.KeyContext, SessionContextProvider.DefaultContext);

                INhPagedDAO dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void SessionCacheProviderWorngTest()
        {
            var tr = new SessionCacheProvider(this.SessionFactory.OpenSession, false, false);

            INhPagedDAO dao;
            using (tr)
            {
                Assert.AreEqual(tr.KeyContext, SessionContextProvider.DefaultContext);

                dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
            // an error must be thrown ..
            // 'cause 
            dao.FindAll<Student>();
        }

        [Test]
        public void SessionCacheProviderTest2()
        {
            var tr = new SessionCacheProvider(this.SessionFactory.OpenSession, false, false);
            INhPagedDAO dao;

            using (null)
            {
                Assert.AreEqual(tr.KeyContext, SessionContextProvider.DefaultContext);

                dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }

            // you can keep using the dao instance, because the its session provider didn't disposed.
            var res2 = dao.FindAll<Salesman>(n => n.ID < 10);
            Assert.IsTrue(res2 != null);
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionCacheProvider()
        {
            new SessionContextProvider(null);
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionCacheProvider3()
        {
            new SessionCacheProvider(this.SessionFactory.OpenSession, "", false, false);
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionCacheProvider4()
        {
            new SessionCacheProvider(this.SessionFactory.OpenSession, "             ", false, false);
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionCacheProvider5()
        {
            new SessionCacheProvider(this.SessionFactory.OpenSession, null, false, false);
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void FailedSessionCacheProvider6()
        {
            new SessionCacheProvider(this.GetSession, "keyContext", false, false);
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedSessionCacheProvider7()
        {
            new SessionCacheProvider(() => null, "keyContext", false, false);
        }

        [Test]
        public void SessionCachedWithNewSessionAfterCommit()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, true, false))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(provider);
                
                provider.BeginTransaction("first");
                var res1 = dao.Load<Salesman>(1L, LockMode.Read);
                
                Assert.IsTrue(dao.IsCached(res1));          /* res1 is associated into the current session, so it's cached */
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res1));         /* after commit the current session was closed and unreferenced, so res1 is not present in the new / next session to open. */


                provider.BeginTransaction("second");
                var res2 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreNotSame(res1, res2);
                Assert.IsTrue(dao.IsCached(res2));
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res2));


                provider.BeginTransaction("third");
                var res3 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreNotSame(res1, res3);
                Assert.AreNotSame(res2, res3);
                Assert.IsTrue(dao.IsCached(res3));
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res3));
            }
        }

        [Test]
        public void SessionCachedWithNewSessionAfterRollback()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, false, true))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(provider);

                provider.BeginTransaction("first");
                var res1 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.IsTrue(dao.IsCached(res1));          /* res1 is associated into the current session, so it's cached */
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res1));          /* after commit the current session was closed and unreferenced, so res1 is not present in the new / next session to open. */


                provider.BeginTransaction("second");
                var res2 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreSame(res1, res2);
                Assert.IsTrue(dao.IsCached(res2));
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res2));


                provider.BeginTransaction("third");
                var res3 = dao.Load<Salesman>(1L, LockMode.Read);
                Assert.IsTrue(dao.IsCached(res3));
                provider.RollbackTransaction();

                
                provider.BeginTransaction("fourth");
                var res4 = dao.Load<Salesman>(1L, LockMode.Read);
                Assert.IsTrue(dao.IsCached(res4));
                Assert.IsFalse(dao.IsCached(res1));
                Assert.IsFalse(dao.IsCached(res2));
                Assert.IsFalse(dao.IsCached(res3));
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res4));
            }
        }

        [Test]
        public void SessionCachedWithSameSessionAfterCommit()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, false, false))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(provider);

                provider.BeginTransaction("first");
                var res1 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.IsTrue(dao.IsCached(res1));          /* res1 is associated into the current session, so it's cached */
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res1));         /* after commit the current session was cached, so res1 is present into the same session. */


                provider.BeginTransaction("second");
                var res2 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreSame(res1, res2);
                Assert.IsTrue(dao.IsCached(res2));
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res2));


                provider.BeginTransaction("third");
                var res3 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreSame(res1, res3);
                Assert.AreSame(res2, res3);
                Assert.IsTrue(dao.IsCached(res3));
                provider.CommitTransaction();
                Assert.IsTrue(dao.IsCached(res3));
            }
        }

        [Test]
        public void SessionCachedWithNewSessionAfterCommitOrRollback()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, true, true))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(provider);

                provider.BeginTransaction("first");
                var res1 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.IsTrue(dao.IsCached(res1));           /* res1 is associated into the current session, so it's cached */
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res1));          /* after commit the current session was closed and unreferenced, so res1 is not present in the new / next session to open. */


                provider.BeginTransaction("second");
                var res2 = dao.Load<Salesman>(1L, LockMode.Read);
                Assert.IsTrue(dao.IsCached(res2));
                Assert.IsFalse(dao.IsCached(res1));
                provider.RollbackTransaction();
                Assert.IsFalse(dao.IsCached(res2));


                provider.BeginTransaction("third");
                var res3 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.AreNotSame(res1, res2);
                Assert.AreNotSame(res1, res3);
                Assert.AreNotSame(res2, res3);
                Assert.IsTrue(dao.IsCached(res3));
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res3));
            }
        }


        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void FailedSessionCacheProviderMultiSession()
        {
            INhPagedDAO dao;
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, true, false))
            {
                dao = new EnterprisePagedDAO(provider);

                provider.BeginTransaction("first");
                var res1 = dao.Load<Salesman>(1L, LockMode.Read);

                Assert.IsTrue(dao.IsCached(res1));          /* res1 is associated into the current session, so it's cached */
                provider.CommitTransaction();
                Assert.IsFalse(dao.IsCached(res1));         /* after commit the current session was closed and unreferenced, so res1 is not present in the new / next session to open. */

            }
            dao.FindAll<Salesman>(n => n.ID > 1);       /* throws an exception because the ISessionProvider was disposed. */
        }

        [Test]
        public void TestMultiTransactions()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, true, false))
            {
                provider.BeginTransaction("first");
                Assert.IsTrue(provider.InProgress);

                provider.BeginTransaction("second");
                Assert.IsTrue(provider.InProgress);

                provider.BeginTransaction("third");
                Assert.IsTrue(provider.InProgress);

                provider.CommitTransaction();
                Assert.IsTrue(provider.InProgress);

                provider.CommitTransaction();
                Assert.IsTrue(provider.InProgress);

                provider.CommitTransaction();
                Assert.IsFalse(provider.InProgress);
            }
        }

        [Test]
        [ExpectedException(typeof(InnerRollBackException))]
        public void TestMultiTransactionsWithRollback()
        {
            using (ISessionProvider provider = new SessionCacheProvider(this.SessionFactory.OpenSession, true, true))
            {
                provider.BeginTransaction("first");
                Assert.IsTrue(provider.InProgress);

                provider.BeginTransaction("second");
                Assert.IsTrue(provider.InProgress);

                provider.BeginTransaction("third");
                Assert.IsTrue(provider.InProgress);

                provider.CommitTransaction();           /* commit the third transaction. */
                Assert.IsTrue(provider.InProgress);

                provider.RollbackTransaction();         /* this throws an exception because there are another inner transaction in progress. */
                
            }
        }

        private ISession GetSession()
        {
            ISession session;
            if (this.internalSession == null)
            {
                session = this.SessionFactory.OpenSession();
                this.internalSession = session;
            }
            else
            {
                session = this.internalSession;
            }

            return session;
        }


        public override void OnFinishedTest()
        {
            base.OnFinishedTest();
            if (this.internalSession != null && this.internalSession.IsOpen)
                this.internalSession.Close();
        }
    }
}
