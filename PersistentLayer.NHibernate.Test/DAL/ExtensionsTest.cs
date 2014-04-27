using System;
using System.Reflection;
using System.Text;
using NHibernate;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test.DAL
{
    public class ExtensionsTest
        : CurrentTester
    {
        [Test]
        [Category("Extensions")]
        public void GetMetadataInfo()
        {
            var info = CurrentPagedDAO.GetPersistentClassInfo(typeof (Salesman));
            Assert.IsNotNull(info);
        }

        [Test]
        [Category("Extensions")]
        [ExpectedException(typeof(BusinessLayerException))]
        public void FailedGetMetadaInfo()
        {
            CurrentPagedDAO.GetPersistentClassInfo(null);
        }

        [Test]
        [Category("Extensions")]
        public void GetNullMetadataInfo()
        {
            var info = CurrentPagedDAO.GetPersistentClassInfo(typeof(StringBuilder));
            Assert.IsNull(info);
        }


        [Test]
        [Category("Metadata")]
        public void MetadataSetterProperty()
        {
            this.DiscardCurrentSession();

            Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
            var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));

            Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());
            metadata.SetPropertyValue(instance, "Email", "My_Email_test", EntityMode.Poco);
            Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());
        }

        [Test]
        [Category("Metadata")]
        public void TestUpdatingWithReflection()
        {
            CurrentPagedDAO.Evict();

            Salesman salNoUpdated = CurrentPagedDAO.FindBy<Salesman, long?>(11);
            Type type = typeof(Salesman);
            PropertyInfo info = type.GetProperty("Email");

            Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());
            info.SetValue(salNoUpdated, "my_email_test", null);
            Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());
            
        }

    }
}
