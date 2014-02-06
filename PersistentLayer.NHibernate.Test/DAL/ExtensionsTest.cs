using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using PersistentLayer.Domain;
using PersistentLayer.Exceptions;
using PersistentLayer.NHibernate.Impl;
using PersistentLayer.NHibernate.Test;

namespace PersistentLayer.Test.DAL
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

        //[Test]
        //[Category("Metadata")]
        //public void TestWrongProperty()
        //{
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    Assert.IsFalse(metadata.ExistsProperty("MyProperty"));
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(MissingPropertyException))]
        //public void FailedMetadataSetterProperty1()
        //{
        //    Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof (Salesman));
        //    metadata.SetPropertyValue(instance, "Emails", "My_Email_test", EntityMode.Poco);
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(MissingPropertyException))]
        //public void FailedMetadataSetterProperty2()
        //{
        //    Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    metadata.SetPropertyValue(instance, "", "My_Email_test", EntityMode.Poco);
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(MissingPropertyException))]
        //public void FailedMetadataSetterProperty3()
        //{
        //    Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    metadata.SetPropertyValue(instance, null, "My_Email_test", EntityMode.Poco);
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(BusinessObjectException))]
        //public void FailedMetadataSetterProperty4()
        //{
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    metadata.SetPropertyValue(null, null, "My_Email_test", EntityMode.Poco);
        //}

        //[Test]
        //[Category("Metadata")]
        //public void MetadataSetterProperties1()
        //{
        //    Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));

        //    var dic = new Dictionary<string, object>();
        //    dic.Add("Email", "My_Email_test");

        //    Assert.IsFalse(CurrentPagedDAO.SessionWithChanges());
        //    metadata.SetPropertyValues(instance, dic, EntityMode.Poco);
        //    Assert.IsTrue(CurrentPagedDAO.SessionWithChanges());
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(BusinessObjectException))]
        //public void FailedMetadataSetterProperties1()
        //{
        //    Salesman instance = CurrentPagedDAO.FindBy<Salesman, long?>(11);
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    metadata.SetPropertyValues(instance, null, EntityMode.Poco);
        //}

        //[Test]
        //[Category("Metadata")]
        //[ExpectedException(typeof(BusinessObjectException))]
        //public void FailedMetadataSetterProperties2()
        //{
        //    var metadata = CurrentPagedDAO.GetPersistentClassInfo(typeof(Salesman));
        //    metadata.SetPropertyValues(null, new Dictionary<string, object>(), EntityMode.Poco);
        //}

        //[Test]
        //public void TestMetadataN()
        //{
        //    CarContract crt = new CarContract();
        //    Salesman sal = new Salesman(1L);

        //    crt.Owner = sal;

        //    CriteriaBuilder criteriaMaker = new CriteriaBuilder(this.CurrentPagedDAO.GetPersistentClassInfo);
        //    DetachedCriteria criteria = criteriaMaker.MakeCriteria<TradeContract>(crt);

        //    var det = criteria.GetExecutableCriteria(this.CurrentSession);
        //    var ctrs = det.List<TradeContract>();
        //    Assert.IsNotNull(ctrs);
        //}

        //[Test]
        //public void TestMetadata2N()
        //{
        //    CarContract crt = new CarContract();
        //    Salesman sal = new Salesman(10) { Name = "robert", Surname = "law order" };

        //    crt.Owner = sal;

        //    CriteriaBuilder criteriaMaker = new CriteriaBuilder(this.CurrentPagedDAO.GetPersistentClassInfo);
        //    criteriaMaker.EnableLike(MatchMode.Start);

        //    DetachedCriteria criteria = criteriaMaker.MakeCriteria<TradeContract>(crt);

        //    var det = criteria.GetExecutableCriteria(this.CurrentSession);
        //    var ctrs = det.List<TradeContract>();
        //    Assert.IsNotNull(ctrs);
        //}

    }
}
