using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Engine.Query;
using NHibernate.Impl;
using NHibernate.Transform;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.NHibernate.Impl;
using PersistentLayer.NHibernate.Test.PocoPrj;
using System.Collections;

namespace PersistentLayer.NHibernate.Test.DAL
{
    /// <summary>
    /// This class shows how can queries can be used, in particolar 
    /// coomon operations like Joins, Projections, Subqueries, Groups with Projections,
    /// HQL, executing SQL functions and store procedures ecc.
    /// </summary>
    public class QueryTest
        : CurrentTester
    {
        [Test]
        [Category("NamedQueries")]
        [Description("Get a defined query from mapping file.")]
        public void ExecuteNamedQueryTest1()
        {
            var query = CurrentPagedDAO.GetNamedQuery("InstancesByID");
            query.SetString("ID", "1");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Get a defined query from mapping file.")]
        public void ExecuteNamedQueryTest2()
        {
            var query = CurrentPagedDAO.GetNamedQuery("GetConsByDataFunc");
            query.SetString("datarif", "20020101");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Executes a string HQL instruction defined into mapping file.")]
        public void ExecuteNamedQueryTest3()
        {
            var query = CurrentPagedDAO.GetNamedQuery("ConsultantsQueryByCode");
            query.SetString("code", "100");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Another named query defined into mapping file.")]
        public void ExecuteNamedQueryTest4()
        {
            var query = CurrentPagedDAO.GetNamedQuery("SetConsultantByName");
            query.SetString("name", "man");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("A named query which executes a store procedure.")]
        public void ExecuteNamedQueryTest5()
        {
            var query = CurrentPagedDAO.GetNamedQuery("SPSetConsultantByName");
            query.SetString("name", "man");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Executes a simple SQL query.")]
        public void ExecuteNamedQueryTest6()
        {
            var query = CurrentPagedDAO.GetNamedQuery("RepConsultant");
            var result = query.List();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Executes a simple SQL query with transforming output.")]
        public void ExecuteNamedQueryTest7()
        {
            var query = CurrentPagedDAO.GetNamedQuery("RepConsultant");
            var result = query.SetResultTransformer(Transformers.AliasToBean<ReportSalesman>()).List<ReportSalesman>();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("NamedQueries")]
        [Description("Executes a simple SQL query.")]
        public void ExecuteNamedQueryTest8()
        {
            var query = CurrentPagedDAO.GetNamedQuery("GetConsultansBySub");
            query.SetString("counter", "1");
            var result = query.List<Salesman>();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("SimpleQueries")]
        public void TestLoader1()
        {
            var cons1 = CurrentPagedDAO.FindBy<Salesman, long?>(1);
            var cons2 = CurrentPagedDAO.FindBy<Salesman, long?>(2);
            var cons3 = CurrentPagedDAO.FindBy<Salesman, long?>(10);
            Assert.IsTrue(cons1.Contracts.Any());
            Assert.IsTrue(cons2.Contracts.Any());
            Assert.IsTrue(cons3.Contracts.Any());
        }

        [Test]
        [Category("CollectionFilter")]
        [Description("Using a collection filter.")]
        public void TestLoader2()
        {
            string strFilter = "select subclass from CarContract subclass where this.ID = subclass.ID";

            var salesman1 = CurrentPagedDAO.FindBy<Salesman, long?>(1);
            var salesman2 = CurrentPagedDAO.FindBy<Salesman, long?>(2);
            var salesman3 = CurrentPagedDAO.FindBy<Salesman, long?>(3);

            var col1 = CurrentPagedDAO.MakeFilter(salesman1.Contracts, strFilter)
                .List<CarContract>();

            var col2 = CurrentPagedDAO.MakeFilter(salesman2.Contracts, strFilter)
                .List<CarContract>();

            var col3 = CurrentPagedDAO.MakeFilter(salesman3.Contracts, strFilter)
                .List<CarContract>();

            Assert.IsTrue(col1.Count > 0);
            Assert.IsTrue(col2.Count > 0);
            Assert.IsTrue(col3.Count == 0);
        }

        [Test]
        [Category("NoScope")]
        [Description("This test had to be failed because of lazy loading collection that a runtime the encironment doesn't know the real type.")]
        public void TestLoader3()
        {
            //Acts
            var salesman = CurrentPagedDAO.FindBy<Salesman, long?>(1);
            var contratti = salesman.Contracts;
            foreach (var contratto in contratti)
            {
                if (contratto.ID == 4) //this ID is for the subclass HomeContract instance
                    Assert.IsTrue(contratto is HomeContract);
                else
                    Assert.IsTrue(contratto is CarContract);
            }

            var homeContracts = salesman.Contracts.Last();

            Assert.IsTrue(homeContracts is HomeContract);
            HomeContract crt = (HomeContract) homeContracts;

            //Asserts
            Assert.IsTrue(homeContracts != null);
            Assert.IsNotNull(crt.Town);
        }

        [Test]
        [Category("CollectionFilter")]
        [Description("This test verifies that all objects into collection are identical like filtered collection instances")]
        public void TestLoader4()
        {
            var salesman = CurrentPagedDAO.FindBy<Salesman, long>(1);
            var contracts = salesman.Contracts;
            var allContracts = CurrentPagedDAO.MakeFilter(salesman.Contracts, "") //take all contracts without filter
                                .List<TradeContract>()
                                //.Enumerable<TradeContract>()
                                ;

            // Asserts
            Assert.IsTrue(contracts.Count == allContracts.Count());
            foreach (TradeContract tradeContract in allContracts)
            {
                TradeContract current = contracts.First(n => tradeContract.ID == n.ID);
                Assert.AreEqual(current, tradeContract);
                Assert.AreSame(current, tradeContract);
            }
        }

        [Test]
        [Category("CollectionFilter")]
        public void TestMockModify()
        {
            SessionProvider.BeginTransaction(IsolationLevel.ReadCommitted);

            var salesman = CurrentPagedDAO.FindBy<Salesman, long>(1);
            var contracts = salesman.Contracts;
            var allContracts = CurrentPagedDAO.MakeFilter(salesman.Contracts, "") //take all contracts without filter
                                .List<TradeContract>()
                                ;

            // Asserts
            Assert.IsTrue(contracts.Count == allContracts.Count());
            Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());

            foreach (TradeContract tradeContract in allContracts)
            {
                TradeContract current = contracts.First(n => tradeContract.ID == n.ID);

                tradeContract.Description = tradeContract.Description + "_updated";

                //Asserts
                Assert.AreEqual(current, tradeContract);
                Assert.AreSame(current, tradeContract);
            }

            //Asserts
            Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());

            SessionProvider.RollbackTransaction();

            // making rollback no sql statements will be executed, but persistent modified objects remain in memory..
            Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());

            // so, in these cases you have to close the current session in order for discarding changes from memory.
            DiscardCurrentSession();
            Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());
        }

        [Test]
        [Category("HQL_Transform")]
        [Description("Demostrate how queries can be transform query results into specific CLR class using a constructor injected into HQL statement.")]
        public void TestDynamicInstantion1()
        {
            /*NOTE:
             * In order for using a injected constructor, you have to define an appropriate constructor..
             * and dont' forget to import the class used for transforming the query result, that in this case
             * It will be imported by hbm.xml file (ClsToImport.hbm.xml)
             */

            string hql = @"select new SalesmanPrj ( q.ID, q.Name, q.Surname, q.Email ) from Salesman q where q.ID > 10 ";
            var result = CurrentPagedDAO.MakeHQLQuery(hql)
                        //.List()
                        .List<SalesmanPrj>()
                        ;

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("HQL_Transform")]
        [Description("Demostrate how can be transform query results into specific CLR class using a transformer.")]
        public void TestDynamicInstantion2()
        {
            /*NOTE:
             * In order for using a transformer for a HQL result,
             * you must to use aliases for all fields present in HQL statement,
             * otherwise It throws an exception a runtime..
             */

            string hql = @"select instance.ID as ID, instance.Name as Name, instance.Surname as Surname, instance.Email as Email from Salesman instance where instance.ID > 10";
            var result = CurrentPagedDAO.MakeHQLQuery(hql)
                        .SetResultTransformer(Transformers.AliasToBean<SalesmanPrj>())
                        //.List()
                        .List<SalesmanPrj>()
                        ;

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("HQL_Transform")]
        [Description("Like TestDynamicInstantion2() but It uses an aggregation function with grouping, then The result is transformed.")]
        public void TestDynamicInstantion3()
        {
            string hql;
            IList<ReportSalesman> result;
            hql =
                @"
                select crt.Owner.ID as ID, crt.Owner.Name as Name, crt.Owner.Surname as Surname, count(crt) as NumSubAgents
                from TradeContract crt
                group by crt.Owner.ID, crt.Owner.Name, crt.Owner.Surname
                ";

            result = CurrentPagedDAO.MakeHQLQuery(hql)
                    .SetResultTransformer(Transformers.AliasToBean<ReportSalesman>())
                    .List<ReportSalesman>()
                    ;

            Assert.IsNotNull(result);

            // second test similar to previous.. with the same result.
            hql =
                @"
                select sal.ID as ID, sal.Name as Name, sal.Surname as Surname, count(crt) as NumSubAgents
                from Salesman sal
                join sal.Contracts crt
                group by sal.ID, sal.Name, sal.Surname
                ";

            result = CurrentPagedDAO.MakeHQLQuery(hql)
                        .SetResultTransformer(Transformers.AliasToBean<ReportSalesman>())
                        .List<ReportSalesman>()
                        ;

            Assert.IsNotNull(result);
        }

        [Test]
        [Category("HQL_Joins")]
        [Description("Explict an implicit joins on associations.")]
        public void TestJoins1()
        {
            string hql;
            IList result;

            #region
            hql = @"select sal from Salesman sal left join sal.Contracts crt where crt is null";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

            #region
            hql = @"select distinct sal from Salesman sal inner join sal.Contracts crt";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

            #region
            hql = @"select distinct sal from Salesman sal inner join sal.Agents";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

            #region
            hql = @"select distinct sal from Salesman sal left join sal.Agents ag where ag is null";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

        }

        [Test]
        [Category("HQL_Subqueries")]
        [Description("This test demostrate how can be used a simple subqueries with HQL")]
        public void TestSubqueries1()
        {
            string hql;
            IList<Salesman> result;

            #region This test filters Salesman instances by their own contracts (HomeContract and CarContract)

            hql = @"from Salesman sal where sal in (select contract.Owner from TradeContract contract)";
            result = CurrentPagedDAO.MakeHQLQuery(hql)
                .List<Salesman>();
            Assert.IsNotNull(result);
            #endregion


            #region so, in this test the subquery uses the subclass of TradeContract (CarContract), and result will contain only Salesman instance associated with CarContract.
            hql = @"from Salesman sal where sal in (select contract.Owner from CarContract contract)";
            result = CurrentPagedDAO.MakeHQLQuery(hql)
                .List<Salesman>();
            Assert.IsNotNull(result);
            #endregion
        }

        [Test]
        [Category("HQL_Projections")]
        [Description("A simple projection wich includes unique property selected")]
        public void TestProjection1()
        {
            string hql;
            IList result;

            #region
            // the property selected is an association..
            // so the collection result is typed..
            hql = @"select distinct crt.Owner from HomeContract crt";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

            #region
            hql = @"select distinct crt.ID as ID, crt.Town as Town, crt.Price as Peice, crt.Owner.ID as IDSalesman from HomeContract crt";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);

            #endregion

            #region
            hql = @"select crt.ID as ID, crt.Number as Number, crt.Price as Price from TradeContract crt";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);
            
            #endregion
            
        }

        [Test]
        [Category("HQL_Grouping")]
        [Description("This test demostrate ")]
        public void TestGrouping1()
        {
            string hql;
            IList result;

            hql = @"select 
                          sal.ID as ID,
                          sal.Name as Name,
                          sal.Surname as Surname,
                          count(crts) as NumContratti
                    from
                             Salesman sal
                    left join
                             sal.Contracts crts
                    group by
                             sal.ID, sal.Name, sal.Surname
                    having
                             count(crts) > 0
                    ";
            result = CurrentPagedDAO.MakeHQLQuery(hql).List();
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        [Category("HQL_Updating")]
        [Description("This test demostrates how can be done an UPDATE statment.")]
        public void TestUpdating()
        {
            Salesman salNoUpdated = CurrentPagedDAO.FindBy<Salesman, long?>(11);
            CurrentPagedDAO.Evict(salNoUpdated);

            Assert.IsFalse(CurrentPagedDAO.IsCached(salNoUpdated));

            string hql = @"update Salesman sal set sal.Email=:email, sal.Version = sal.Version + 1 where sal.ID=:id";
            IQuery query = CurrentPagedDAO.MakeHQLQuery(hql)
                            .SetParameter("email", "my_new_email_2")
                            .SetParameter("id", 11L);

            var result = query.ExecuteUpdate();
            Assert.IsTrue(result > 0);

            Salesman salUpdated = CurrentPagedDAO.FindBy<Salesman, long?>(11);
            Assert.AreEqual((salNoUpdated.Version + 1), salUpdated.Version);
        }

        [Test]
        [Category("NoScope")]
        public void TestSelect()
        {
            string hql = "from Salesman sal";
            var query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            IList list = query.List();
            Assert.IsNotNull(list);

            hql = "select cast(count(sal.id) as int) as rr from Salesman sal";
            query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            long rowCount = query.UniqueResult<int>();
            Assert.IsTrue(rowCount > 0);

        }


        //[Test]
        //[Category("NoScope")]
        //public void TestSelect2()
        //{
        //    string hql = "from Salesman sal order by sal.Name desc";
        //    var query = this.CurrentPagedDAO.MakeHQLQuery(hql);
        //    IList list = query.List();
        //    Assert.IsNotNull(list);

        //    query.NamedParameters.Initialize();
        //    ISQLQuery qq = null;
        //    //qq.SetParameterList()

        //    //hql = "select cast(count(sal.id) as int) as rr from Salesman sal";        //ok
        //    //hql = "select cast(count(id) as int) as rr from Salesman";                //ok
        //    //hql = ToHqlRowCount(query);
        //    //hql = "select cast(count(res.*) as int) as cc from ( select sal.* from Salesman sal) res";           //ko
        //    //hql = "select count( select sal.* from Salesman sal ) as total ";
        //    hql = "select count(sal) from Salesman sal ";
        //    query = this.CurrentPagedDAO.MakeHQLQuery(hql);
        //    long rowCount = query.UniqueResult<long>();
        //    Assert.IsTrue(rowCount > 0);

        //}

        [Test]
        [Category("NoScope")]
        public void TestSelectX()
        {
            string hql;
            IQuery query;
            IList list;

            //hql = "select sal.Name, sal.Surname from Salesman sal order by sal.Name desc";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //list = query.List();

            //hql = "select distinct sal.Name, sal.Surname from Salesman sal order by sal.Name desc";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //list = query.List();

            //hql = "select concat(sal.Name, '-' ,sal.Surname) as col1 from Salesman sal order by sal.Name desc";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //list = query.List();

            //hql = "select distinct concat(sal.Name, '-' ,sal.Surname) as col1 from Salesman sal";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //list = query.List();

            //hql = "select count( sal.Surname ) from Salesman sal";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //var counter = query.UniqueResult<long>();
            //Assert.IsTrue(counter > 0);

            hql = "select distinct new SalesmanPrj2 ( sal.Name, sal.Surname ) from Salesman sal";
            query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            list = query.List();
            Assert.IsTrue(list.Count > 0);

            //hql = "select count( new SalesmanPrj2 ( sal.Name, sal.Surname ) ) from Salesman sal";
            //query = this.CurrentPagedDAO.MakeHQLQuery(hql);
            //var counter = query.UniqueResult<long>();
            //Assert.IsTrue(counter > 0);
        }

        //[Test]
        //public void TestHql()
        //{
        //    string hql = "from Salesman sal where sal.Name=:nome";

        //    SessionFactoryImpl sf = this.SessionFactory as SessionFactoryImpl;
        //    if (sf != null)
        //    {
        //        var filtri = new Dictionary<string, IFilter>();
        //        var sql = new HQLStringQueryPlan(hql, true, filtri, sf);
        //        var aa = sql.ParameterMetadata.GetNamedParameterDescriptor("");
        //        var ee = aa.Name;

        //        var res = sql.SqlStrings[0];
        //        //HqlSqlGenerator a;
        //        Assert.IsNotNull(res);

        //    }
        //}

        //public static string ToHqlRowCount(IQuery hql)
        //{
        //    string str = hql.QueryString;
        //    int index1 = str.IndexOf("from", System.StringComparison.Ordinal);
        //    int lastindex = str.IndexOf("order", System.StringComparison.Ordinal);
            
        //    return lastindex == -1 ?
        //        str.Substring(index1) : str.Substring(index1, lastindex - index1);

        //}
    }
}
