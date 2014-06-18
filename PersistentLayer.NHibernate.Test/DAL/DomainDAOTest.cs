using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Data;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;
using PersistentLayer.NHibernate.Test.PocoPrj;

namespace PersistentLayer.NHibernate.Test.DAL
{
    [Description("Test for DAO's methods.")]
    public class DomainDAOTest
        : CurrentTester
    {
        [Test]
        [Category("QueryExecutions")]
        public void LoadTest0()
        {
            Assert.IsNotNull(CurrentPagedDAO.Load<Salesman>(1L));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedLoadTest0()
        {
            var cons = CurrentPagedDAO.Load<Salesman>(-1L, LockMode.Read);
            Assert.IsNotNull(cons);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify the right loading of object")]
        public void LoadTest1()
        {
            Assert.IsNotNull(CurrentPagedDAO.Load<Salesman>(1L, LockMode.Read));
            Assert.IsNotNull(CurrentPagedDAO.Load<Salesman>(2L, null));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedLoad2Test()
        {
            Assert.IsNull(CurrentPagedDAO.Load<Salesman>(-1L, LockMode.Read));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Using a non generic method Load.")]
        public void LoadTest3()
        {
            Assert.IsNotNull(CurrentPagedDAO.Load(typeof(Salesman), 1L, LockMode.Read));
            Assert.IsNotNull(CurrentPagedDAO.Load(typeof(Agency), 1L, LockMode.Read));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedLoadTest3()
        {
            object ret = CurrentPagedDAO.Load(typeof(Salesman), -1L, LockMode.Read);
            Assert.IsNotNull(ret);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest1()
        {
            object ret = CurrentPagedDAO.FindBy<Salesman, long?>(1L);
            Assert.IsNotNull(ret);

            object ret2 = CurrentPagedDAO.FindBy(typeof (Salesman), -1L);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest2()
        {
            object ret = CurrentPagedDAO.FindBy<Salesman, long?>(1L, LockMode.None);
            Assert.IsNotNull(ret);

            object ret2 = CurrentPagedDAO.FindBy(typeof(Salesman), -1L, LockMode.None);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindByTest3()
        {
            object ret = CurrentPagedDAO.FindBy<Salesman, long?>(1L, LockMode.Upgrade);
            Assert.IsNotNull(ret);

            object ret2 = CurrentPagedDAO.FindBy(typeof(Salesman), -1L, LockMode.Upgrade);
            Assert.IsNull(ret2);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies the loading of some instances.")]
        public void FindAllTest()
        {
            Assert.IsNotNull(CurrentPagedDAO.FindAll<Salesman>());
            // the follow test fails always because the static method indicated cannot be converted into Sql instruction
            // so it throws an exception when It's executed.
            //Assert.IsNotNull(ownPagedDAO.FindAll<Salesman>(n => string.IsNullOrEmpty(n.Email)));
            Assert.IsNotNull(CurrentPagedDAO.FindAll<Salesman>(n => n.Email == null || n.Email.Equals(string.Empty)));
            Assert.IsNotNull(CurrentPagedDAO.FindAll<Salesman>(true));
            Assert.IsNotNull(CurrentPagedDAO.FindAll<Salesman>(2));
            Assert.IsNotNull(CurrentPagedDAO.FindAll<Salesman>("ciccio"));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies if a valid detached criteria doesn't throw an exception")]
        public void FindAllDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            criteria.Add(Restrictions.Like("Name", "Dav", MatchMode.Start));
            Assert.IsTrue(CurrentPagedDAO.FindAll<Salesman>(criteria).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verifies if a valid detached criteria (non generic) doesn't throw an exception")]
        public void FindAllDetachedCriteriaTest1()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            criteria.Add(Restrictions.Like("Name", "Dav", MatchMode.Start));
            ICollection result = CurrentPagedDAO.FindAll(criteria);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedFindAllDetachedCriteriaTest1()
        {
            CurrentPagedDAO.FindAll<Salesman>((DetachedCriteria)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedFindAllDetachedCriteriaTest2()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>();
            CurrentPagedDAO.FindAll<Agency>(criteria);
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindAllQueryOver()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11);
            Assert.IsTrue(CurrentPagedDAO.FindAll(query).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedFindAllQueryOver()
        {
            // this query must throw an exception because the queryover instance has a projection result.
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11).Select(n => n.ID, n => n.Name);
            Assert.IsTrue(CurrentPagedDAO.FindAll(query).Any());
        }

        [Test]
        [Category("QueryExecutions")]
        public void FindAllFutureDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", (long?)1));
            Assert.IsNotNull(CurrentPagedDAO.FindAllFuture<Salesman>(criteria).FirstOrDefault());
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedFindAllFutureDetachedCriteriaTest()
        {
            CurrentPagedDAO.FindAllFuture<Salesman>((DetachedCriteria)null);
        }

        [Test]
        [Category("QueryFutureExecutions")]
        public void FindAllFutureQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 11);
            Assert.IsNotNull(CurrentPagedDAO.FindAllFuture(query).FirstOrDefault());
        }

        [Test]
        [Category("QueryFutureExecutions")]
        [Description("the method GetFutureValue returns the first elements of query result.")]
        public void GetFutureValueDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.IdEq(1));
            Assert.IsNotNull(CurrentPagedDAO.GetFutureValue<Salesman>(criteria).Value);
        }

        [Test]
        [Category("QueryFutureExecutions")]
        [Description("the method GetFutureValue returns the first elements of query result.")]
        public void GetFutureValueQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID == 1);
            Assert.IsNotNull(CurrentPagedDAO.GetFutureValue<Salesman, Salesman>(query));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists the given indentifiers.")]
        public void ExistsIDTest()
        {
            Assert.IsTrue(CurrentPagedDAO.Exists<Salesman, long?>(1));
            long?[] identifiers = new long?[]{ 1, 2};
            Assert.IsTrue(CurrentPagedDAO.Exists<Salesman, long?>(identifiers));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists the given indentifiers.")]
        public void ExistsWithExpressionTest()
        {
            //Assert.IsTrue(CurrentPagedDAO.Exists<Salesman, long?>(1));
            //long?[] identifiers = new long?[] { 1, 2 };
            //Assert.IsTrue(CurrentPagedDAO.Exists<Salesman, long?>(identifiers));
            Assert.IsTrue(CurrentPagedDAO.Exists<Salesman>(salesman => salesman.Name != null ));
            Assert.IsFalse(CurrentPagedDAO.Exists<Salesman>(salesman => salesman.Surname == null));
        }

        [Test]
        [Category("QueryExecutions")]
        public void FailedExistsIDTest()
        {
            Assert.IsFalse(CurrentPagedDAO.Exists<Salesman, long?>(-1));
            long?[] identifiers = new long?[] { 1, -2 }; //the identifier 1 exists, but -2 no!.
            Assert.IsFalse(CurrentPagedDAO.Exists<Salesman, long?>(identifiers));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsIDsTest()
        {
            Assert.IsFalse(CurrentPagedDAO.Exists<Salesman, long?>((long?[])null));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists any results with the given detached criteria.")]
        public void ExistsDetachedCriteriaTest()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("ID", (long)1));
            Assert.IsTrue(CurrentPagedDAO.Exists(criteria));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsDetachedCriteriaTest()
        {
            // ReSharper disable RedundantCast
            Assert.IsTrue(CurrentPagedDAO.Exists((DetachedCriteria)null));
            // ReSharper restore RedundantCast
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Verify if exists any results with the given query over.")]
        public void ExistsQueryOverTest()
        {
            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID == 1);
            Assert.IsTrue(CurrentPagedDAO.Exists(query));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedExistsQueryOverTest()
        {
            Assert.IsFalse(CurrentPagedDAO.Exists((QueryOver<Salesman>)null));
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("Test on IQueryable objects.")]
        public void ToIQueryableTest()
        {
            //Assert.IsTrue(CurrentPagedDAO.ToIQueryable<Salesman>().Any());
            Assert.IsTrue(CurrentPagedDAO.ExecuteExpression((IEnumerable<Salesman> col) => col.Any()));

            //Assert.IsTrue(CurrentPagedDAO.ToIQueryable<Salesman>(CacheMode.Refresh).Any());
            Assert.IsTrue(CurrentPagedDAO.ExecuteExpression((IEnumerable<Salesman> col) => col.Any(),
                          CacheMode.Refresh));

            //Assert.IsTrue(CurrentPagedDAO.ToIQueryable<Salesman>("pages1").Any());
            Assert.IsTrue(CurrentPagedDAO.ExecuteExpression((IEnumerable<Salesman> col) => col.Any(),
                          "pages1"));

            //Assert.IsTrue(CurrentPagedDAO.ToIQueryable<Salesman>(CacheMode.Refresh, "pages2").Any());
            Assert.IsTrue(CurrentPagedDAO.ExecuteExpression((IEnumerable<Salesman> col) => col.Any(),
                          CacheMode.Refresh, "pages2"));
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

            CurrentPagedDAO.MakePersistent(cons);
            Assert.IsTrue(CurrentPagedDAO.IsCached(cons));
            CurrentPagedDAO.Evict(cons);

            SessionProvider.RollbackTransaction();
        }

        [Test]
        [Category("QueryPersistentExecutions")]
        [ExpectedException(typeof(BusinessPersistentException))]
        public void FailedMakePersistentSaveTest()
        {
            ReportSalesman rep = new ReportSalesman {ID = 10, Name = "Ciccio"};
            CurrentPagedDAO.MakePersistent(rep);
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
            Salesman cons = CurrentPagedDAO.ExecuteExpression<Salesman, Salesman>(saleswoman => saleswoman.First());
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
                CurrentPagedDAO.MakePersistent(cons2);
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
                //Salesman cons = CurrentPagedDAO.ToIQueryable<Salesman>().First(n => n.ID == 11);
                Salesman cons = CurrentRootPagedDAO.ExecuteExpression<Salesman, Salesman>(saleswoman => saleswoman.First(salesman => salesman.ID == 11));
                Assert.IsNotNull(cons);
                Assert.IsTrue(CurrentPagedDAO.IsCached(cons));
                string oldEmail = cons.Email;
                string newEmail = string.Format("{0}_.{1}_@gmail.com", cons.Name, cons.Surname);
                cons.Email = newEmail;
                SessionProvider.CommitTransaction();

                SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);
                Salesman cons1 = CurrentPagedDAO.FindBy<Salesman, long?>(cons.ID, LockMode.Upgrade);
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
                //Salesman cons = CurrentRootPagedDAO.ToIQueryable<Salesman>().First(n => n.ID == 11);
                Salesman cons = CurrentRootPagedDAO.UniqueResult<Salesman>(salesman => salesman.ID == 11);
                cons.Email = "test_email";
                /*
                 * the calling of this method fails because It tries to update an persistent instance reference (cons),
                 * associated with the given identifier(11).
                 * So, in order to use this method, the instance to update must be transient or detached, and naturally
                 * the given indentifier must exists in data store.
                 */

                CurrentPagedDAO.MakePersistent(cons, 11);

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
            CurrentPagedDAO.MakePersistent((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedMakeTransientTest()
        {
            CurrentPagedDAO.MakeTransient((Salesman)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedMakeTransientCollectionTest()
        {
            CurrentPagedDAO.MakeTransient((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryMockPersistentExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedRefreshStateTest()
        {
            CurrentPagedDAO.RefreshState((Salesman)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedRefreshStateCollectionTest()
        {
            CurrentPagedDAO.RefreshState((IEnumerable<Salesman>)null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetPagedResultTest1()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1));
            IPagedResult<Salesman> result =  CurrentPagedDAO.GetPagedResult<Salesman>(1, 5, criteria);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetPagedResult1()
        {
            CurrentPagedDAO.GetPagedResult<Salesman>(1, 5, (DetachedCriteria)null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetPagedResultTest2()
        {
            IPagedResult<Salesman> result = CurrentPagedDAO.GetPagedResult(1, 5, QueryOver.Of<Salesman>().Where(n => n.ID > 1));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetPagedResult2()
        {
            CurrentPagedDAO.GetPagedResult(1, 5, (QueryOver<Salesman>)null);
        }

        [Test]
        [Category("QueryExecutions")]
        [Description("This method shows how It can be used a no generic IPagedResult object.")]
        public void GetPagedResultTest4()
        {
            IPagedResult result = CurrentPagedDAO.GetPagedResult(2, 5, DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1)));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void FailedGetPagedResultTest4()
        {
            IPagedResult result = CurrentPagedDAO.GetPagedResult(2, -1, DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1)));
            Assert.IsTrue(result.Counter > 0);
        }


        [Test]
        [Category("QueryExecutions")]
        [Description("This method shows how It can be used a no generic IPagedResult object.")]
        public void GetPagedResultTest5()
        {
            IPagedResult result1 = CurrentPagedDAO.GetPagedResult<Salesman>(2, 5, salesman => salesman.ID > 10);
            Assert.IsTrue(result1.Counter > 0);

            IPagedResult result2 = CurrentPagedDAO.GetPagedResult<Salesman>(2, 5, salesman => salesman.ID > 1000);
            Assert.IsFalse(result2.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest5()
        {
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long)1));
            IPagedResult<Salesman> result = CurrentPagedDAO.GetIndexPagedResult<Salesman>(2, 5, criteria);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIndexPagedResultTest5()
        {
            IPagedResult<Salesman> result = CurrentPagedDAO.GetIndexPagedResult<Salesman>(2, 5, (DetachedCriteria)null);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest6()
        {
            IPagedResult<Salesman> result = CurrentPagedDAO.GetIndexPagedResult(2, 5, QueryOver.Of<Salesman>().Where(n => n.ID > 1));
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIndexPagedResultTest6()
        {
            IPagedResult<Salesman> result = CurrentPagedDAO.GetIndexPagedResult(2, 5, (QueryOver<Salesman>)null);
            Assert.IsTrue(result.Counter > 0);
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest7()
        {
            var criteria = DetachedCriteria.For<Salesman>();
            
            long rowCount = CurrentPagedDAO.RowCount(criteria);
            int pageSize = 5;

            long rest = rowCount % pageSize;
            long numPages = (rowCount / pageSize) + (rest > 0 ? 1 : 0);
            
            var ris = CurrentPagedDAO.GetIndexPagedResult<Salesman>(Convert.ToInt32(numPages - 1), Convert.ToInt32(pageSize), criteria)
                                        .GetResult().Count();
            Assert.IsTrue(ris == rest);

            for (int index = 0; index < numPages - 1; index++)
            {
                ris = CurrentPagedDAO.GetIndexPagedResult<Salesman>(index, pageSize, criteria).GetResult().Count();
                Assert.IsTrue(ris == pageSize, "wrong element at index {0}", index);
            }
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIndexPagedResultTest8()
        {
            var criteria = QueryOver.Of<Salesman>();

            long rowCount = CurrentPagedDAO.RowCount(criteria);
            int pageSize = 5;

            long rest = rowCount % pageSize;
            long numPages = (rowCount / pageSize) + (rest > 0 ? 1 : 0);

            var ris = CurrentPagedDAO.GetIndexPagedResult(Convert.ToInt32(numPages - 1), Convert.ToInt32(pageSize), criteria)
                                        .GetResult().Count();
            Assert.IsTrue(ris == rest);

            for (int index = 0; index < numPages - 1; index++)
            {
                ris = CurrentPagedDAO.GetIndexPagedResult(index, pageSize, criteria).GetResult().Count();
                Assert.IsTrue(ris == pageSize, "wrong element at index {0}", index);
            }
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetRowCountTest1()
        {
            IEnumerable<Salesman> result = CurrentPagedDAO.FindAll<Salesman>();
            long rowCount = CurrentPagedDAO.RowCount(DetachedCriteria.For<Salesman>());
            Assert.IsTrue(result.Count() == rowCount);

            DetachedCriteria criteria1 = DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", 1L));
            result = CurrentPagedDAO.FindAll<Salesman>(criteria1);
            rowCount = CurrentPagedDAO.RowCount(criteria1);
            Assert.IsTrue(result.Count() == rowCount);

            QueryOver<Salesman> query = QueryOver.Of<Salesman>().Where(n => n.ID > 10);
            result = CurrentPagedDAO.FindAll(query);
            rowCount = CurrentPagedDAO.RowCount(query);
            Assert.IsTrue(result.Count() == rowCount);

        }

        [Test]
        [Category("QueryExecutions")]
        public void IsCachedTest()
        {
            var cons = CurrentPagedDAO.FindBy<Salesman, long?>(1, null);
            Assert.IsTrue(CurrentPagedDAO.IsCached(cons));

            CurrentPagedDAO.Evict(cons);
            Assert.IsFalse(CurrentPagedDAO.IsCached(cons));

            Assert.IsFalse(CurrentPagedDAO.IsCached((Salesman)null));
        }

        [Test]
        [Category("QueryExecutions")]
        public void GetIdentifierTest()
        {
            long? id = 1;
            var cons = CurrentPagedDAO.FindBy<Salesman, long?>(id, null);
            Assert.IsNotNull(CurrentPagedDAO.GetIdentifier<Salesman, long?>(cons));
        }

        [Test]
        [Category("QueryExecutions")]
        [ExpectedException(typeof(QueryArgumentException))]
        public void FailedGetIdentifierTest()
        {
            CurrentPagedDAO.GetIdentifier<Salesman, long?>(null);
        }

        [Test]
        [Category("QueryExecutions")]
        public void SessionWithChangesTest()
        {
            long? id = 1;
            var cons = CurrentPagedDAO.FindBy<Salesman, long?>(id, null);

            string oldEmail = cons.Email;
            cons.Email = "ciao_email";
            Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());

            cons.Email = oldEmail;
            Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictTest()
        {
            long? id = 1;
            var cons = CurrentPagedDAO.FindBy<Salesman, long?>(id, null);

            Assert.IsTrue(CurrentPagedDAO.IsCached(cons));
            CurrentPagedDAO.Evict(cons);

            Assert.IsFalse(CurrentPagedDAO.IsCached(cons));
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictCollectionTest()
        {
            var col = CurrentPagedDAO.FindAll<Salesman>(DetachedCriteria.For<Salesman>().Add(Restrictions.Gt("ID", (long?) 1)));
            
            Assert.IsTrue(CurrentPagedDAO.IsCached(col));
            CurrentPagedDAO.Evict(col);

            Assert.IsFalse(CurrentPagedDAO.IsCached(col));
        }

        [Test]
        [Category("QueryExecutions")]
        public void EvictAllTest()
        {
            var col = CurrentPagedDAO.FindAll<Salesman>(DetachedCriteria.For<Salesman>());
            Assert.IsTrue(CurrentPagedDAO.IsCached(col));
            CurrentPagedDAO.Evict();

            Assert.IsFalse(CurrentPagedDAO.IsCached(col));
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
            var col = CurrentPagedDAO.TransformResult<Salesman, SalesmanPrj>(query);
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
            var col = CurrentPagedDAO.ToProjectOver(query);
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
            var col = CurrentPagedDAO.TransformFutureResult<Salesman, SalesmanPrj>(query);
            var count = CurrentPagedDAO.GetFutureValue<Salesman, int>(query.ToRowCountQuery());
            
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

            var col = CurrentPagedDAO.ToProjectOverFuture(query);
            var count = CurrentPagedDAO.GetFutureValue<Salesman, int>(query.ToRowCountQuery());

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

            var col = CurrentPagedDAO.TransformResult<SalesmanPrj>(criteria);
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

            var col = CurrentPagedDAO.ToProjectOver(criteria);
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

            var col = CurrentPagedDAO.TransformFutureResult<SalesmanPrj>(criteria);
            var count = CurrentPagedDAO.GetFutureValue<int>(CriteriaTransformer.TransformToRowCount(criteria));
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

            var col = CurrentPagedDAO.ToProjectOverFuture(criteria);
            var count = CurrentPagedDAO.GetFutureValue<int>(CriteriaTransformer.TransformToRowCount(criteria));
            Assert.IsTrue(count.Value > 0 && col.Any());
        }
        
        [Test]
        public void MakeFutureFunctionTest()
        {
            FutureFunction function = new FutureFunction(typeof(Salesman));
            Assert.IsTrue(function != null);            
        }

        [Test]
        public void UniqueResult1()
        {
            var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.ID == 1);
            Assert.IsNotNull(result);
        }

        [Test]
        public void UniqueResult2()
        {
            var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.ID == -1);
            Assert.IsNull(result);
        }

        [Test]
        [ExpectedException(typeof(ExecutionQueryException))]
        public void WrongUniqueResult1()
        {
            var result = CurrentPagedDAO.UniqueResult<Salesman>(salesman => salesman.Name == "Manuel");
            Assert.IsNull(result);
        }


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

        [Test]
        public void TestExternalExpExecutors1()
        {
            var customDAO = this.CurrentPagedDAO;

            Expression<Func<IEnumerable<Salesman>, Salesman>> queryExpr
                = persons => (from a in persons
                              where a.ID == 1
                              select a)
                             .FirstOrDefault()
                ;

            var person = customDAO.ExecuteExpression(queryExpr);
            Assert.IsNotNull(person);
        }

        [Test]
        public void TestExternalExpExecutors2()
        {
            var customDAO = this.CurrentPagedDAO;

            Expression<Func<IEnumerable<Salesman>, IEnumerable<string>>> queryExpr
                = persons => (from a in persons
                              select a.Name
                              )
                              .ToList()
                ;

            var peopleName = customDAO.ExecuteExpression(queryExpr);
            Assert.IsNotNull(peopleName);
        }

        [Test]
        public void TestExpressionExecutors()
        {
            var customDAO = this.CurrentPagedDAO;

            var result1 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.FirstOrDefault());
            Assert.IsNotNull(result1);

            var result2 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.Where(person1 => person1.ID < 5));
            Assert.IsNotNull(result2);

            var result3 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.Select(person => new { person.Name, person.Surname }));
            Assert.IsNotNull(result3);

            var result4 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.Where(person => person.ID > 1).Select(person => new { person.Name, person.Surname }));
            Assert.IsNotNull(result4);

            var result5 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.Select(person => new SalesmanPrj { Name = person.Name, Surname = person.Surname }));
            Assert.IsNotNull(result5);

            // I don't understand how come this instruction works .... 
            // dynamic was introduced in framework 4.0 version !

            //var result6 = customDAO.ExecuteExpression((IEnumerable<Salesman> entities) => entities.Select<Salesman, dynamic>(person => new { Name = person.Name, Surname = person.Surname }));
            //Assert.IsNotNull(result6);

        }

        [Test]
        [ExpectedException(typeof(QueryArgumentException))]
        public void TestExecuteBadExpression1()
        {
            this.CurrentPagedDAO.ExecuteExpression<Salesman, Salesman>(null);
        }

        [Test]
        public void TestExecutesuspiciousExpression()
        {
            //this.CurrentRootPagedDAO.ExecuteExpression<Salesman, IEnumerable<Salesman>>(enumerable => enumerable.Where(salesman => salesman.Name.StartsWith("M")));
            this.CurrentPagedDAO.ExecuteExpression<Salesman, IEnumerable<Salesman>>
                (
                    //enumerable => enumerable.Where(salesman => salesman.Name.Trim().StartsWith("M"))                  //fuck
                    //enumerable => enumerable.Where(salesman => salesman.Name.Trim().Equals("Manuel"))                 //fuck
                    //enumerable => enumerable.Where(salesman => salesman.Name.Trim().GetHashCode() > 0)                //fuck
                    //enumerable => enumerable.Where(salesman => salesman.Name.Trim().Substring(2, 1).Equals("n"))      //fuck
                    //enumerable => enumerable.Where(salesman => salesman.Name.Trim().Substring(2, 1).Equals("n"))      //fuck
                    enumerable => enumerable.Where(salesman => !salesman.Name.Trim()
                        .Trim()
                        .Trim()
                        .Substring(2, 1).Equals("n"))    //fuck
                );
        }
    }
}
