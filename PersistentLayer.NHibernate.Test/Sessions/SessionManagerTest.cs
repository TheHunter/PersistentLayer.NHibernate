using System;
using System.Data;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Test.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionManagerTest
        : CurrentTester
    {

        [Test]
        public void TestRightExecution1()
        {
            this.SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);

            this.Module1(1);
            this.Module2(1);
            this.Module3(1);

            this.SessionProvider.CommitTransaction();

            Assert.IsFalse(this.SessionProvider.InProgress);
        }

        [Test]
        public void TestRightExecution2()
        {
            this.SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);

            this.ModuleX<Salesman, long?>(1);
            this.ModuleX<TradeContract, long?>(1);
            this.ModuleX<Agency, long?>(1);

            this.SessionProvider.CommitTransaction();

            Assert.IsFalse(this.SessionProvider.InProgress);
        }

        [Test]
        public void TestWrongExecution1()
        {
            Assert.IsFalse(this.SessionProvider.InProgress);
            try
            {
                this.SessionProvider.BeginTransaction("BeginRequest", IsolationLevel.ReadCommitted);

                this.Module1(1);
                this.Module2(-1);
                this.Module3(1);

                this.SessionProvider.CommitTransaction();
            }
            catch (InnerRollBackException)
            {
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
            Assert.IsFalse(this.SessionProvider.InProgress);
        }

        [Test]
        public void TestWrongExecution2()
        {
            Assert.IsFalse(this.SessionProvider.InProgress);
            try
            {
                this.SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);

                this.Module1(1);
                this.Module2(1);
                this.Module3(-1);

                this.SessionProvider.CommitTransaction();
            }
            catch (BusinessLayerException)
            {
                Assert.IsTrue(true);
                Assert.IsFalse(this.SessionProvider.InProgress);
                this.DiscardCurrentSession();
            }
            catch (Exception)
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void TestWrongExecution3()
        {
            Assert.IsFalse(this.SessionProvider.InProgress);

            try
            {
                this.SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                this.ModuleX<Salesman, long?>(-1);
                this.ModuleX<TradeContract, long?>(1);
                this.ModuleX<Agency, long?>(1);
                this.SessionProvider.CommitTransaction();
            }
            catch (InnerRollBackException)
            {
                Assert.IsTrue(true);
                Assert.IsFalse(this.SessionProvider.InProgress);
            }
        }

        [Test]
        public void TestWrongExecution4()
        {
            Assert.IsFalse(this.SessionProvider.InProgress);

            try
            {
                this.SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                this.ModuleXX<Salesman, long?>(1);
                this.ModuleXX<TradeContract, long?>(-1);
                this.ModuleXX<Agency, long?>(1);
                this.SessionProvider.CommitTransaction();
            }
            catch (InnerRollBackException)
            {
                Assert.IsTrue(true);
                Assert.IsFalse(this.SessionProvider.InProgress);
            }
            catch (Exception)
            {
                // if the root transaction is active, so you have to make a rollback,
                // but in this case, invoking the rollback method, It could be throw an InnerRollBackException.
                if (this.SessionProvider.InProgress)
                {
                    try
                    {
                        this.SessionProvider.RollbackTransaction();
                    }
                    catch (Exception)
                    {
                        // in this case, you can ignore this exception.
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Module1(long? id)
        {
            try
            {
                this.SessionProvider.BeginTransaction("Module1", IsolationLevel.ReadCommitted);
                Salesman current = this.CurrentPagedDAO.FindBy<Salesman, long?>(id);
                this.SessionProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.SessionProvider.RollbackTransaction(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Module2(long? id)
        {
            try
            {
                this.SessionProvider.BeginTransaction("Module2", IsolationLevel.ReadCommitted);
                TradeContract current = this.CurrentPagedDAO.FindBy<TradeContract, long?>(id);
                this.SessionProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.SessionProvider.RollbackTransaction(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Module3(long? id)
        {
            try
            {
                this.SessionProvider.BeginTransaction("Module3", IsolationLevel.ReadCommitted);
                Agency current = this.CurrentPagedDAO.FindBy<Agency, long?>(id);
                this.SessionProvider.CommitTransaction();
            }
            catch (Exception ex)
            {
                this.SessionProvider.RollbackTransaction(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        internal void ModuleX<TSource, TKey>(TKey id)
            where TSource : class
        {
            var provider = this.CurrentPagedDAO.GetNhTransactionProvider();
            try
            {
                provider.BeginTransaction("ModuleX", IsolationLevel.ReadCommitted);
                this.CurrentPagedDAO.FindBy<TSource, TKey>(id);
                provider.CommitTransaction();
            }
            catch (Exception ex)
            {
                provider.RollbackTransaction(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="id"></param>
        internal void ModuleXX<TSource, TKey>(TKey id)
            where TSource : class
        {
            var provider = this.CurrentPagedDAO.GetNhTransactionProvider();
            try
            {
                provider.BeginTransaction("ModuleX", IsolationLevel.ReadCommitted);
                this.CurrentPagedDAO.FindBy<TSource, TKey>(id);
                provider.CommitTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception has occurred.", ex);
            }
        }
    }
}
