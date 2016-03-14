using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using PersistentLayer.Domain;

namespace PersistentLayer.NHibernate.Test
{
    [Description("Test for complex  DAO's methods.")]
    public class ComplexExpressions
        : CurrentTester
    {
        [Test]
        public void FunctionExpression()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, object> e =
                entities => entities.Where(salesman => salesman.ID > 5)
                                    .Select(salesman => new { salesman.ID, salesman.Name });

            Expression<Func<IQueryable<Salesman>, object>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNotNull(res5);
        }

        [Test]
        public void FunctionExpression2()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, dynamic> e =
                entities => entities.Where(salesman => salesman.ID > 5)
                                    .Select(salesman => new { salesman.ID, salesman.Name });

            Expression<Func<IQueryable<Salesman>, dynamic>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNotNull(res5);
        }

        [Test]
        public void FunctionExpression3()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, IQueryable<dynamic>> e =
                entities => entities.Where(salesman => salesman.ID > 5)
                                    .Select(salesman => new { salesman.ID, salesman.Name });

            Expression<Func<IQueryable<Salesman>, IQueryable<dynamic>>> f = queryable => e(queryable);
            IQueryable<dynamic> res5 = customDAO.ExecuteExpression(f);
            Assert.IsNotNull(res5);

            var first = res5.First();
            Assert.IsNotNull(first.ID);
            Assert.IsNotNull(first.Name);

            var last = res5.Last();
            Assert.IsNotNull(last.ID);
            Assert.IsNotNull(last.Name);
        }

        [Test]
        public void FunctionExpression4()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, IEnumerable<dynamic>> e =
                entities => entities.Where(salesman => salesman.ID > 5)
                                    .Select(salesman => new { salesman.ID, salesman.Name, salesman.Surname });

            Expression<Func<IQueryable<Salesman>, IEnumerable<dynamic>>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNotNull(res5);

            var first = res5.First();
            Assert.IsNotNull(first.ID);
            Assert.IsNotNull(first.Name);
            Assert.IsNotNull(first.Surname);
        }


        [Test]
        public void FunctionExpression5()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, IEnumerable<dynamic>> e =
                entities => entities.Where(salesman => salesman.ID > 5)
                                    .GroupBy(salesman => salesman.Name)
                                    .Select(grouping => new { Name=grouping.Key, Counter=grouping.Count() })
                ;
                                    //.Select(salesman => new { salesman.ID, salesman.Name, salesman.Surname });

            Expression<Func<IQueryable<Salesman>, IEnumerable<dynamic>>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNotNull(res5);

            var first = res5.First();
            Assert.IsNotNull(first.Name);
            Assert.IsTrue(first.Counter > 0);
        }

        [Test]
        public void FunctionUsingSingleFirstOrDefault()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, Salesman> e =
                entities => entities.SingleOrDefault(salesman => salesman.ID == -1);

            Expression<Func<IQueryable<Salesman>, Salesman>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNull(res5);
        }

        [Test]
        public void FunctionUsingSingleFirstOrDefaultScalar()
        {
            var customDAO = this.CurrentPagedDAO;

            Func<IQueryable<Salesman>, long?> e =
                entities => entities.Where(salesman => salesman.ID == -5).Sum(salesman => salesman.ID);

            Expression<Func<IQueryable<Salesman>, long?>> f = queryable => e(queryable);
            var res5 = customDAO.ExecuteExpression(f);
            Assert.IsNull(res5);
        }
    }
}
