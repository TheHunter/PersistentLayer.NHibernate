using System.Linq;
using NHibernate;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.Sessions
{
    public class ContextProviderTest
        : CurrentTester
    {
        private ISession internalSession = null;

        [Test]
        public void SessionContextProviderTest()
        {
            using (var tr = new SessionContextProvider(this.SessionFactory.OpenSession))
            {
                INhPagedDAO dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void SessionContextProviderWorngTest()
        {
            var tr = new SessionContextProvider(this.SessionFactory.OpenSession);

            INhPagedDAO dao;
            using (tr)
            {
                dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
            // an error must be thrown ..
            // 'cause 
            dao.FindAll<Student>();
        }

        [Test]
        public void SessionContextProviderTest2()
        {
            var tr = new SessionContextProvider(this.SessionFactory.OpenSession);

            using (null)
            {
                INhPagedDAO dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider()
        {
            new SessionContextProvider(null);
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider3()
        {
            new SessionContextProvider(this.SessionFactory.OpenSession, "");
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider4()
        {
            new SessionContextProvider(this.SessionFactory.OpenSession, "             ");
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider5()
        {
            new SessionContextProvider(this.SessionFactory.OpenSession, null);
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void FailedSessionContextProvider6()
        {
            new SessionContextProvider(this.GetSession, "keyContext");
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedSessionContextProvider7()
        {
            new SessionContextProvider(() => null, "keyContext");
        }

        [Test]
        public void TestMultiTransactions()
        {
            using (ISessionProvider provider = new SessionContextProvider(this.SessionFactory.OpenSession))
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
            using (ISessionProvider provider = new SessionContextProvider(this.SessionFactory.OpenSession))
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
            if (this.internalSession.IsOpen)
                this.internalSession.Close();
        }
    }
}
