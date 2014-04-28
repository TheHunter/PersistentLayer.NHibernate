using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.Sessions
{
    public class BinderProviderTest
        : CurrentTester
    {
        [Test]
        public void SessionBinderProviderTest()
        {
            this.UnBindSession();

            ISessionBinderProvider provider = new SessionBinderProvider(this.SessionFactory.GetCurrentSession,
                                                                        this.LocalBindSession, this.LocalUnBindSession,
                                                                        () =>
                                                                        CurrentSessionContext.HasBind(
                                                                            this.SessionFactory));
            INhPagedDAO dao = new EnterprisePagedDAO(provider);
            
            provider.BindSession();
            dao.Load<Salesman>(1L);
            provider.UnBindSession();
        }

        [Test]
        [ExpectedException(typeof(SessionNotBindedException))]
        public void FailedSessionBinderProviderTest()
        {
            this.UnBindSession();

            ISessionBinderProvider provider = new SessionBinderProvider(this.SessionFactory.GetCurrentSession,
                                                                        this.LocalBindSession, this.LocalUnBindSession,
                                                                        () =>
                                                                        CurrentSessionContext.HasBind(
                                                                            this.SessionFactory));
            INhPagedDAO dao = new EnterprisePagedDAO(provider);
            dao.Load<Salesman>(1L);
        }

        [Test]
        public void TestMultiTransactions()
        {
            this.UnBindSession();

            using (ISessionBinderProvider provider = new SessionBinderProvider(this.SessionFactory.GetCurrentSession,
                                                                        this.LocalBindSession, this.LocalUnBindSession,
                                                                        () =>
                                                                        CurrentSessionContext.HasBind(
                                                                            this.SessionFactory)))
            {
                provider.BindSession();

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

                provider.UnBindSession();
            }
        }

        [Test]
        [ExpectedException(typeof(InnerRollBackException))]
        public void TestMultiTransactionsWithRollback()
        {
            this.UnBindSession();

            using (ISessionBinderProvider provider = new SessionBinderProvider(this.SessionFactory.GetCurrentSession,
                                                                        this.LocalBindSession, this.LocalUnBindSession,
                                                                        () =>
                                                                        CurrentSessionContext.HasBind(
                                                                            this.SessionFactory)))
            {
                try
                {
                    provider.BindSession();

                    provider.BeginTransaction("first");
                    Assert.IsTrue(provider.InProgress);

                    provider.BeginTransaction("second");
                    Assert.IsTrue(provider.InProgress);

                    provider.BeginTransaction("third");
                    Assert.IsTrue(provider.InProgress);

                    provider.CommitTransaction(); /* commit the third transaction. */
                    Assert.IsTrue(provider.InProgress);

                    provider.RollbackTransaction();
                        /* this throws an exception because there are another inner transaction in progress. */
                }
                finally
                {
                    provider.UnBindSession();
                }
            }
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void TestTransactionsWithoutBindSession()
        {
            this.UnBindSession();

            using (ISessionBinderProvider provider = new SessionBinderProvider(this.SessionFactory.GetCurrentSession,
                                                                        this.LocalBindSession, this.LocalUnBindSession,
                                                                        () =>
                                                                        CurrentSessionContext.HasBind(
                                                                            this.SessionFactory)))
            {
                provider.BeginTransaction("first");
            }
        }

        private void LocalBindSession()
        {
            CurrentSessionContext.Bind(this.SessionFactory.OpenSession());
        }

        
        private void LocalUnBindSession()
        {
            ISession session = CurrentSessionContext.Unbind(this.SessionFactory);
            if (session != null && session.IsOpen)
                session.Close();
        }

        
    }
}
