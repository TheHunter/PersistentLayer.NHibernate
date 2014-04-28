using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityModel;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate.Test.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DaoFactoryTest
        :CurrentTester 
    {
        [Test]
        public void BuildingDaoTest1()
        {
            INhPagedDAO dao = NhDaoFactory.MakePagedDAO(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll<Salesman>(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDao1Test1()
        {
            INhPagedDAO dao = NhDaoFactory.MakePagedDAO(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll<Salesman>(salesman => salesman.ID > 10);
        }

        [Test]
        public void BuildingDaoTest2()
        {
            INhPagedDAO dao = NhDaoFactory.MakePagedDAO(this.SessionFactory, "userContext");
            using (dao)
            {
                var res1 = dao.FindAll<Salesman>(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest2()
        {
            INhPagedDAO dao = NhDaoFactory.MakePagedDAO(this.SessionFactory, "userContext");
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll<Salesman>(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest3()
        {
            INhPagedDAO<Salesman, long?> dao = NhDaoFactory.MakePagedDAO<Salesman, long?>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest3()
        {
            INhPagedDAO<Salesman, long?> dao = NhDaoFactory.MakePagedDAO<Salesman, long?>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest4()
        {
            INhPagedDAO<Salesman, long?> dao = NhDaoFactory.MakePagedDAO<Salesman, long?>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest4()
        {
            INhPagedDAO<Salesman, long?> dao = NhDaoFactory.MakePagedDAO<Salesman, long?>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest5()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<Salesman>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll<Salesman>(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest5()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<Salesman>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll<Salesman>(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest6()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<Salesman>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll<Salesman>(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest6()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<Salesman>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll<Salesman>(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest7()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<IEntity<long?>, Salesman>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest7()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<IEntity<long?>, Salesman>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll(salesman => salesman.ID < 10);
        }

        [Test]
        public void BuildingDaoTest8()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<IEntity<long?>, Salesman>(this.SessionFactory);
            using (dao)
            {
                var res1 = dao.FindAll(salesman => salesman.ID < 10);
                Assert.IsNotNull(res1);
            }
        }

        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedBuildingDaoTest8()
        {
            var dao = NhDaoFactory.MakeRootPagedDAO<IEntity<long?>, Salesman>(this.SessionFactory);
            using (dao)
            {
                // your custom queries calls...
            }
            dao.FindAll(salesman => salesman.ID < 10);
        }
    }
}
