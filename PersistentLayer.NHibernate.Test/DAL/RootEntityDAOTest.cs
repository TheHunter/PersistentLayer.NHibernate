using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;
using PersistentLayer.NHibernate.Test.PocoPrj;

namespace PersistentLayer.NHibernate.Test.DAL
{
    [Description("Test for DAO's methods using a root entity.")]
    public class RootEntityDAOTest
        : CurrentTester
    {
        [Test]
        [Category("QueryExecutions")]
        public void LoadTest0()
        {
            Assert.IsNotNull(CurrentRootPagedDAO.Load<Salesman>(1L));
        }

        [Test]
        [Category("QueryExecutions")]
        public void FailedLoadTest0()
        {
            var cons = CurrentRootPagedDAO.Load<Salesman>(-1L);
            Assert.IsNotNull(cons);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify the right loading of object")]
        public void LoadTest1()
        {
            Assert.IsNotNull(CurrentRootPagedDAO.Load<Salesman>(1L, LockMode.Read));
            Assert.IsNotNull(CurrentRootPagedDAO.Load<Salesman>(2L, null));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedLoad2Test()
        {
            Assert.IsNull(CurrentRootPagedDAO.Load<Salesman>(-1L, LockMode.Read));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Using a non generic method Load.")]
        public void LoadTest3()
        {
            Assert.IsNotNull(CurrentRootPagedDAO.Load(typeof(Salesman), 1L, LockMode.Read));
            Assert.IsNotNull(CurrentRootPagedDAO.Load(typeof(Agency), 1L, LockMode.Read));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedLoadTest3()
        {
            object ret = CurrentRootPagedDAO.Load(typeof(Salesman), -1L, LockMode.Read);
            Assert.IsNotNull(ret);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest1()
        {
            object ret = CurrentRootPagedDAO.FindBy<Salesman>(1L);
            Assert.IsNotNull(ret);

            object ret2 = CurrentRootPagedDAO.FindBy(typeof(Salesman), -1L);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest2()
        {
            object ret = CurrentRootPagedDAO.FindBy<Salesman>(1L, LockMode.None);
            Assert.IsNotNull(ret);

            object ret2 = CurrentRootPagedDAO.FindBy(typeof(Salesman), -1L, LockMode.None);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest3()
        {
            object ret = CurrentRootPagedDAO.FindBy<Salesman>(1L, LockMode.Upgrade);
            Assert.IsNotNull(ret);

            object ret2 = CurrentRootPagedDAO.FindBy(typeof(Salesman), -1L, LockMode.Upgrade);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies the loading of some instances.")]
        public void FindAllTest()
        {
            Assert.IsNotNull(CurrentRootPagedDAO.FindAll<Salesman>());
            // the follow test fails always because the static method indicated cannot be converted into Sql instruction
            // so it throws an exception when It's executed.
            //Assert.IsNotNull(ownPagedDAO.FindAll<Salesman>(n => string.IsNullOrEmpty(n.Email)));
            Assert.IsNotNull(CurrentRootPagedDAO.FindAll<Salesman>(n => n.Email == null || n.Email.Equals(string.Empty)));
            Assert.IsNotNull(CurrentRootPagedDAO.FindAll<Salesman>(true));
            Assert.IsNotNull(CurrentRootPagedDAO.FindAll<Salesman>(2));
            Assert.IsNotNull(CurrentRootPagedDAO.FindAll<Salesman>("ciccio"));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies if a valid detached criteria doesn't throw an exception")]
        public void FindAllDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            criteria.Add(Restrictions.Like("Name", "Dav", MatchMode.Start));
            Assert.IsTrue(CurrentRootPagedDAO.FindAll<Salesman>(criteria).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies if a valid detached criteria (non generic) doesn't throw an exception")]
        public void FindAllDetachedCriteriaTest1()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            criteria.Add(Restrictions.Like("Name", "Dav", MatchMode.Start));
            ICollection result = CurrentRootPagedDAO.FindAll(criteria);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedFindAllDetachedCriteriaTest1()
        {
            CurrentRootPagedDAO.FindAll<Salesman>((DetachedCriteria)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedFindAllDetachedCriteriaTest2()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            CurrentRootPagedDAO.FindAll<Agency>(criteria);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindAllQueryOver()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11);
            Assert.IsTrue(CurrentRootPagedDAO.FindAll(query).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedFindAllQueryOver()
        {
            // this query must throw an exception because the queryover instance has a projection result.
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11).Select(n => n.ID, n => n.Name);
            Assert.IsTrue(CurrentRootPagedDAO.FindAll(query).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindAllFutureDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", (long?)1));
            Assert.IsNotNull(CurrentRootPagedDAO.FindAllFuture<Salesman>(criteria).FirstOrDefault());
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedFindAllFutureDetachedCriteriaTest()
        {
            CurrentRootPagedDAO.FindAllFuture<Salesman>((DetachedCriteria)null);
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void FindAllFutureQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11);
            Assert.IsNotNull(CurrentRootPagedDAO.FindAllFuture(query).FirstOrDefault());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        [Description("the method GetFutureValue returns the first elements of query result.")]
        public void GetFutureValueDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.IdEq(1));
            Assert.IsNotNull(CurrentRootPagedDAO.GetFutureValue<Salesman>(criteria).Value);
        }

        [Test]
        [Category("QueryFutureExecutions")]
        [Description("the method GetFutureValue returns the first elements of query result.")]
        public void GetFutureValueQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID == 1);
            Assert.IsNotNull(CurrentRootPagedDAO.GetFutureValue<Salesman, Salesman>(query));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists the given indentifiers.")]
        public void ExistsIDTest()
        {
            Assert.IsTrue(CurrentRootPagedDAO.Exists<Salesman>(1L));
            long?[] identifiers = new long?[] { 1, 2 };
            Assert.IsTrue(CurrentRootPagedDAO.Exists<Salesman>(identifiers));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists the given indentifiers.")]
        public void ExistsWithExpressionTest()
        {
            Assert.IsTrue(CurrentRootPagedDAO.Exists<Salesman>(salesman => salesman.Name != null));
            Assert.IsFalse(CurrentRootPagedDAO.Exists<Salesman>(salesman => salesman.Surname == null));
        }

        [Test]
        [Category("QueryExecutions")]
        public void FailedExistsIDTest()
        {
            Assert.IsFalse(CurrentRootPagedDAO.Exists<Salesman>(-1L));
            long?[] identifiers = new long?[] { 1, -2 }; //the identifier 1 exists, but -2 no!.
            Assert.IsFalse(CurrentRootPagedDAO.Exists<Salesman>(identifiers));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsIDsTest()
        {
            Assert.IsFalse(CurrentRootPagedDAO.Exists<Salesman>((long?[])null));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists any results with the given detached criteria.")]
        public void ExistsDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", (long)1));
            Assert.IsTrue(CurrentRootPagedDAO.Exists(criteria));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsDetachedCriteriaTest()
        {
            // ReSharper disable RedundantCast
            Assert.IsTrue(CurrentRootPagedDAO.Exists((DetachedCriteria)null));
            // ReSharper restore RedundantCast
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists any results with the given query over.")]
        public void ExistsQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID == 1);
            Assert.IsTrue(CurrentRootPagedDAO.Exists(query));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsQueryOverTest()
        {
            Assert.IsFalse(CurrentRootPagedDAO.Exists((QueryOver<Salesman>)null));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Test on IQueryable objects.")]
        public void ToIQueryableTest()
        {
            Assert.IsTrue(CurrentRootPagedDAO.ToIQueryable<Salesman>().Any());
            Assert.IsTrue(CurrentRootPagedDAO.ToIQueryable<Salesman>(CacheMode.Refresh).Any());
            Assert.IsTrue(CurrentRootPagedDAO.ToIQueryable<Salesman>("pages1").Any());
            Assert.IsTrue(CurrentRootPagedDAO.ToIQueryable<Salesman>(CacheMode.Refresh, "pages2").Any());
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        public void MakePersistentSaveTest()
        {
            SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);

            Salesman cons = new Salesman
            {
                Name = "Maria",
                Surname = "Bonita",
                IdentityCode = 450,
                Email = "chica_bonita@hotmail.com"
            };

            CurrentRootPagedDAO.MakePersistent(cons);
            Assert.IsTrue(CurrentRootPagedDAO.IsCached(cons));
            CurrentRootPagedDAO.Evict(cons);

            SessionProvider.RollbackTransaction();
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        [ExpectedException(typeof(BusinessPersistentException))]
        public void FailedMakePersistentSaveTest()
        {
            ReportSalesman rep = new ReportSalesman { ID = 10, Name = "Ciccio" };
            CurrentRootPagedDAO.MakePersistent(rep);
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        [ExpectedException(typeof(BusinessPersistentException))]
        public void FailedMakePersistentSave2Test()
        {
            /*
             * NOTA:
             * This test tries to save an transient instance which properties state is copied by a persistent instance.
             * So.. in a few words, the calling of MakePersistent method tries to update the given instance (if this one is persistent)
             * otherwise MakePersistent method tries to save it, but It can throw an exception if the given transient state instance 
             * is equals to an existing persistent state into data store.
             */

            SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
            Salesman cons = CurrentRootPagedDAO.ToIQueryable<Salesman>().First();
            Salesman cons2 = new Salesman
            {
                //ID = cons.ID,
                IdentityCode = cons.IdentityCode,
                Name = cons.Name,
                Surname = cons.Surname,
                Email = cons.Email
            };
            cons2.UpdateId(cons);

            cons2.UpdateVersion(cons);
            try
            {
                CurrentRootPagedDAO.MakePersistent(cons2);
                SessionProvider.CommitTransaction();
            }
            catch (Exception)
            {
                SessionProvider.RollbackTransaction();
                DiscardCurrentSession();
                throw;
            }
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        public void MakePersistentUpdateTest()
        {
            try
            {
                SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                Salesman cons = CurrentRootPagedDAO.ToIQueryable<Salesman>().First(n => n.ID == 11);
                Assert.IsNotNull(cons);
                Assert.IsTrue(CurrentRootPagedDAO.IsCached(cons));
                string oldEmail = cons.Email;
                string newEmail = string.Format("{0}_.{1}_@gmail.com", cons.Name, cons.Surname);
                cons.Email = newEmail;
                SessionProvider.CommitTransaction();

                SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                Salesman cons1 = CurrentRootPagedDAO.FindBy<Salesman>(cons.ID, LockMode.Upgrade);
                cons1.Email = oldEmail;
                SessionProvider.CommitTransaction();
            }
            catch (Exception)
            {
                SessionProvider.RollbackTransaction();
                DiscardCurrentSession();
                throw;
            }
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        [ExpectedException(typeof(BusinessPersistentException))]
        public void FailedMakePersistentUpdateTest()
        {
            try
            {
                SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                Salesman cons = CurrentRootPagedDAO.ToIQueryable<Salesman>().First(n => n.ID == 11);
                cons.Email = "test_email";
                /*
                 * the calling of this method fails because It tries to update an persistent instance reference (cons),
                 * associated with the given identifier(11).
                 * So, in order to use this method, the instance to update must be transient or detached, and naturally
                 * the given indentifier must exists in data store.
                 */
                
                CurrentRootPagedDAO.MakePersistent(cons, 11);         // thrown an error, right
                //CurrentRootPagedDAO.MakePersistent(cons, null);       // thrown an error, right
                //CurrentRootPagedDAO.MakePersistent(cons, 0);            // thrown an error, right

                SessionProvider.CommitTransaction();
            }
            catch (Exception)
            {
                SessionProvider.RollbackTransaction();
                DiscardCurrentSession();
                throw;
            }
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedMakePersistentCollectionTest()
        {
            CurrentRootPagedDAO.MakePersistent((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedMakeTransientTest()
        {
            CurrentRootPagedDAO.MakeTransient((Salesman)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedMakeTransientCollectionTest()
        {
            CurrentRootPagedDAO.MakeTransient((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedRefreshStateTest()
        {
            CurrentRootPagedDAO.RefreshState((Salesman)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedRefreshStateCollectionTest()
        {
            CurrentRootPagedDAO.RefreshState((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetPagedResultTest1()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1));
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetPagedResult<Salesman>(1, 5, criteria);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetPagedResult1()
        {
            CurrentRootPagedDAO.GetPagedResult<Salesman>(1, 5, (DetachedCriteria)null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetPagedResultTest2()
        {
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetPagedResult(1, 5, QueryOver.Of<Salesman>().Where(n => n.ID > 1));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetPagedResult2()
        {
            CurrentRootPagedDAO.GetPagedResult(1, 5, (QueryOver<Salesman>)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("This method shows how It can be used a no generic IPagedResult object.")]
        public void GetPagedResultTest4()
        {
            IPagedResult result = CurrentRootPagedDAO.GetPagedResult(2, 5, DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1)));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedGetPagedResultTest4()
        {
            IPagedResult result = CurrentRootPagedDAO.GetPagedResult(2, -1, DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1)));
            Assert.IsTrue(result.Counter > 0);
        }


        [Test]
        [Category("QueryExecutions")]
        [Description("This method shows how It can be used a no generic IPagedResult object.")]
        public void GetPagedResultTest5()
        {
            IPagedResult result1 = CurrentRootPagedDAO.GetPagedResult<Salesman>(2, 5, salesman => salesman.ID > 10);
            Assert.IsTrue(result1.Counter > 0);

            IPagedResult result2 = CurrentRootPagedDAO.GetPagedResult<Salesman>(2, 5, salesman => salesman.ID > 1000);
            Assert.IsFalse(result2.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest5()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", 1L));
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetIndexPagedResult<Salesman>(2, 5, criteria);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIndexPagedResultTest5()
        {
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetIndexPagedResult<Salesman>(2, 5, (DetachedCriteria)null);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest6()
        {
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetIndexPagedResult(2, 5, QueryOver.Of<Salesman>().Where(n => n.ID > 1));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIndexPagedResultTest6()
        {
            IPagedResult<Salesman> result = CurrentRootPagedDAO.GetIndexPagedResult(2, 5, (QueryOver<Salesman>)null);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest7()
        {
            var criteria = DetachedCriteria.For<Salesman>();

            long rowCount = CurrentRootPagedDAO.RowCount(criteria);
            int pageSize = 5;

            long rest = rowCount % pageSize;
            long numPages = (rowCount / pageSize) + (rest > 0 ? 1 : 0);

            var ris = CurrentRootPagedDAO.GetIndexPagedResult<Salesman>(Convert.ToInt32(numPages - 1), Convert.ToInt32(pageSize), criteria)
                                        .GetResult().Count();
            Assert.IsTrue(ris == rest);

            for (int index = 0; index < numPages - 1; index++)
            {
                ris = CurrentRootPagedDAO.GetIndexPagedResult<Salesman>(index, pageSize, criteria).GetResult().Count();
                Assert.IsTrue(ris == pageSize, "wrong element at index {0}", index);
            }
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest8()
        {
            var criteria = QueryOver.Of<Salesman>();

            long rowCount = CurrentRootPagedDAO.RowCount(criteria);
            int pageSize = 5;

            long rest = rowCount % pageSize;
            long numPages = (rowCount / pageSize) + (rest > 0 ? 1 : 0);

            var ris = CurrentRootPagedDAO.GetIndexPagedResult(Convert.ToInt32(numPages - 1), Convert.ToInt32(pageSize), criteria)
                                        .GetResult().Count();
            Assert.IsTrue(ris == rest);

            for (int index = 0; index < numPages - 1; index++)
            {
                ris = CurrentRootPagedDAO.GetIndexPagedResult(index, pageSize, criteria).GetResult().Count();
                Assert.IsTrue(ris == pageSize, "wrong element at index {0}", index);
            }
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetRowCountTest1()
        {
            IEnumerable<Salesman> result = CurrentRootPagedDAO.FindAll<Salesman>();
            long rowCount = CurrentRootPagedDAO.RowCount(DetachedCriteria.For<Salesman>());
            Assert.IsTrue(result.Count() == rowCount);

            DetachedCriteria criteria1 = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", 1L));
            result = CurrentRootPagedDAO.FindAll<Salesman>(criteria1);
            rowCount = CurrentRootPagedDAO.RowCount(criteria1);
            Assert.IsTrue(result.Count() == rowCount);

            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 10);
            result = CurrentRootPagedDAO.FindAll(query);
            rowCount = CurrentRootPagedDAO.RowCount(query);
            Assert.IsTrue(result.Count() == rowCount);

        }

        [Test]
        [Category("QueryExecutions")]
        public void IsCachedTest()
        {
            var cons = CurrentRootPagedDAO.FindBy<Salesman>(1L, null);
            Assert.IsTrue(CurrentRootPagedDAO.IsCached(cons));

            CurrentRootPagedDAO.Evict(cons);
            Assert.IsFalse(CurrentRootPagedDAO.IsCached(cons));

            Assert.IsFalse(CurrentRootPagedDAO.IsCached((Salesman)null));
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIdentifierTest()
        {
            long? id = 1;
            var cons = CurrentRootPagedDAO.FindBy<Salesman>(id, null);
            Assert.IsNotNull(CurrentRootPagedDAO.GetIdentifier<Salesman, long?>(cons));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIdentifierTest()
        {
            CurrentRootPagedDAO.GetIdentifier<Salesman, long?>(null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void SessionWithChangesTest()
        {
            long? id = 1;
            var cons = CurrentRootPagedDAO.FindBy<Salesman>(id, null);

            string oldEmail = cons.Email;
            cons.Email = "ciao_email";
            Assert.IsTrue(CurrentRootPagedDAO.SessionWithChanges());

            cons.Email = oldEmail;
            Assert.IsFalse(CurrentRootPagedDAO.SessionWithChanges());
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictTest()
        {
            long? id = 1;
            var cons = CurrentRootPagedDAO.FindBy<Salesman>(id, null);

            Assert.IsTrue(CurrentRootPagedDAO.IsCached(cons));
            CurrentRootPagedDAO.Evict(cons);

            Assert.IsFalse(CurrentRootPagedDAO.IsCached(cons));
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictCollectionTest()
        {
            var col = CurrentRootPagedDAO.FindAll<Salesman>(DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", 1L)));

            Assert.IsTrue(CurrentRootPagedDAO.IsCached(col));
            CurrentRootPagedDAO.Evict(col);

            Assert.IsFalse(CurrentRootPagedDAO.IsCached(col));
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictAllTest()
        {
            var col = CurrentRootPagedDAO.FindAll<Salesman>(DetachedCriteria.For<Salesman>());
            Assert.IsTrue(CurrentRootPagedDAO.IsCached(col));
            CurrentRootPagedDAO.Evict();

            Assert.IsFalse(CurrentRootPagedDAO.IsCached(col));
        }

        [Test]
        [Category("QueryExecutions")]
        public void TransformResultQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>()
                .Select(
                    Projections.ProjectionList()
                    .Add(Projections.Property("ID"), "ID")
                    .Add(Projections.Property("Name"), "Name")
                    .Add(Projections.Property("Surname"), "Surname")
                    .Add(Projections.Property("Email"), "Email")
                );
            var col = CurrentRootPagedDAO.TransformResult<Salesman, SalesmanPrj>(query);
            Assert.IsTrue(col.Any());
        }

        [Test]
        [Category("QueryExecutions")]
        public void ToProjectOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>()
                .Select(
                    Projections.ProjectionList()
                    .Add(Projections.Property("ID"), "ID")
                    .Add(Projections.Property("Name"), "Name")
                    .Add(Projections.Property("Surname"), "Surname")
                    .Add(Projections.Property("Email"), "Email")
                );
            var col = CurrentRootPagedDAO.ToProjectOver(query);
            Assert.IsTrue(col.Any());
        }

        [Test]
        [Category("QueryExecutions")]
        public void TransformFutureResultTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>()
                .Select(
                    Projections.ProjectionList()
                    .Add(Projections.Property("ID"), "ID")
                    .Add(Projections.Property("Name"), "Name")
                    .Add(Projections.Property("Surname"), "Surname")
                    .Add(Projections.Property("Email"), "Email")
                );
            var col = CurrentRootPagedDAO.TransformFutureResult<Salesman, SalesmanPrj>(query);
            var count = CurrentRootPagedDAO.GetFutureValue<Salesman, int>(query.ToRowCountQuery());

            Assert.IsTrue(count.Value > 0 && col.Any());
        }

        [Test]
        [Category("QueryExecutions")]
        public void ToProjectOverFutureTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>()
                .Select(
                    Projections.ProjectionList()
                    .Add(Projections.Property("ID"), "ID")
                    .Add(Projections.Property("Name"), "Name")
                    .Add(Projections.Property("Surname"), "Surname")
                    .Add(Projections.Property("Email"), "Email")
                );

            var col = CurrentRootPagedDAO.ToProjectOverFuture(query);
            var count = CurrentRootPagedDAO.GetFutureValue<Salesman, int>(query.ToRowCountQuery());

            Assert.IsTrue(count.Value > 0 && col.Any());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void TransformResultDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>()
                .SetProjection
                (
                    Projections.ProjectionList()
                        .Add(Projections.Property("ID"), "ID")
                        .Add(Projections.Property("Name"), "Name")
                        .Add(Projections.Property("Surname"), "Surname")
                        .Add(Projections.Property("Email"), "Email")
                );

            var col = CurrentRootPagedDAO.TransformResult<SalesmanPrj>(criteria);
            Assert.IsTrue(col.Any());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void ToProjectOverDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>()
                .SetProjection
                (
                    Projections.ProjectionList()
                        .Add(Projections.Property("ID"), "ID")
                        .Add(Projections.Property("Name"), "Name")
                        .Add(Projections.Property("Surname"), "Surname")
                        .Add(Projections.Property("Email"), "Email")
                );

            var col = CurrentRootPagedDAO.ToProjectOver(criteria);
            Assert.IsTrue(col.Any());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void TransformFutureResultDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>()
                .SetProjection
                (
                    Projections.ProjectionList()
                        .Add(Projections.Property("ID"), "ID")
                        .Add(Projections.Property("Name"), "Name")
                        .Add(Projections.Property("Surname"), "Surname")
                        .Add(Projections.Property("Email"), "Email")
                );

            var col = CurrentRootPagedDAO.TransformFutureResult<SalesmanPrj>(criteria);
            var count = CurrentRootPagedDAO.GetFutureValue<int>(CriteriaTransformer.TransformToRowCount(criteria));
            Assert.IsTrue(count.Value > 0 && col.Any());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void ToProjectOverFutureDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>()
                .SetProjection
                (
                    Projections.ProjectionList()
                        .Add(Projections.Property("ID"), "ID")
                        .Add(Projections.Property("Name"), "Name")
                        .Add(Projections.Property("Surname"), "Surname")
                        .Add(Projections.Property("Email"), "Email")
                );

            var col = CurrentRootPagedDAO.ToProjectOverFuture(criteria);
            var count = CurrentRootPagedDAO.GetFutureValue<int>(CriteriaTransformer.TransformToRowCount(criteria));
            Assert.IsTrue(count.Value > 0 && col.Any());
        }

        [Test]
        public void MakeFutureFunctionTest()
        {
            FutureFunction function = new FutureFunction(typeof(Salesman));
            Assert.IsTrue(function != null);
        }

        //[Test]
        //public void UniqueResult1()
        //{
        //    var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.ID == 1);
        //    Assert.IsNotNull(result);
        //}

        //[Test]
        //public void UniqueResult2()
        //{
        //    var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.ID == -1);
        //    Assert.IsNull(result);
        //}

        //[Test]
        //[ExpectedException(typeof(ExecutionQueryException))]
        //public void WrongUniqueResult1()
        //{
        //    var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.Name == "Manuel");
        //    Assert.IsNull(result);
        //}


        [Test]
        public void UniqueResult3()
        {
            var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", 1L));
            var result = CurrentPagedDAO.UniqueResult<Salesman>(criteria);
            Assert.IsNotNull(result);
        }

        [Test]
        public void UniqueResult4()
        {
            var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", -1L));
            var result = CurrentPagedDAO.UniqueResult<Salesman>(criteria);
            Assert.IsNull(result);
        }

        [Test]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void WrongUniqueResult2()
        {
            var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("Name", "Manuel"));
            var result = CurrentPagedDAO.UniqueResult<Salesman>(criteria);
            Assert.IsNull(result);
        }


        [Test]
        public void UniqueResult5()
        {
            var criteria = QueryOver.Of<Salesman>().Where(salesman => salesman.ID == 1);
            var result = CurrentPagedDAO.UniqueResult(criteria);
            Assert.IsNotNull(result);
        }

        [Test]
        public void UniqueResult6()
        {
            var criteria = QueryOver.Of<Salesman>().Where(salesman => salesman.ID == -1);
            var result = CurrentPagedDAO.UniqueResult(criteria);
            Assert.IsNull(result);
        }

        [Test]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void WrongUniqueResult3()
        {
            var criteria = QueryOver.Of<Salesman>().Where(salesman => salesman.Name == "Manuel");
            var result = CurrentPagedDAO.UniqueResult(criteria);
            Assert.IsNull(result);
        }

    }
}
