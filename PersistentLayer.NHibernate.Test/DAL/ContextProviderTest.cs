using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.DAL
{
    
    public class ContextProviderTest
        : CurrentTester
    {
        [Test]
        public void SessionContextProviderTest()
        {
            var session = this.SessionFactory.OpenSession();

            using (var tr = new SessionContextProvider(session))
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
            var session = this.SessionFactory.OpenSession();
            var tr = new SessionContextProvider(session);

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
            var session = this.SessionFactory.OpenSession();
            var tr = new SessionContextProvider(session);

            using (null)
            {
                INhPagedDAO dao = new EnterprisePagedDAO(tr);
                var res = dao.FindAll<Salesman>();
                Assert.IsTrue(res.Any());
            }
        }


        [Test]
        [ExpectedException(typeof(SessionNotAvailableException))]
        public void FailedSessionContextProvider()
        {
            new SessionContextProvider(null);
        }

        [Test]
        [ExpectedException(typeof(InvalidSessionException))]
        public void FailedSessionContextProvider2()
        {
            var session = this.SessionFactory.OpenSession();
            session.Close();

            new SessionContextProvider(session);
        }


        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider3()
        {
            var session = this.SessionFactory.OpenSession();
            new SessionContextProvider(session, "");
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider4()
        {
            var session = this.SessionFactory.OpenSession();
            new SessionContextProvider(session, "             ");
        }

        [Test]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedSessionContextProvider5()
        {
            var session = this.SessionFactory.OpenSession();
            new SessionContextProvider(session, null);
        }
    }
}
