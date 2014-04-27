using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using PersistentLayer.NHibernate.Impl;

namespace PersistentLayer.NHibernate.Test
{
    [TestFixture]
    public class CurrentTester
    {
        static ISessionFactory sessionFactory;
        private NhConfigurationBuilder builder;
        private INhPagedDAO currentPagedDAO;
        private INhRootPagedDAO<object> currentRootPagedDAO;
        private ISessionManager sessionProvider;
        string rootPathProject;
        private ISession currentSession;

        protected ISession CurrentSession
        {
            get { return this.currentSession; }
        }

        /// <summary>
        /// 
        /// </summary>
        public INhPagedDAO CurrentPagedDAO
        {
            get { return currentPagedDAO; }
        }

        /// <summary>
        /// 
        /// </summary>
        public INhRootPagedDAO<object> CurrentRootPagedDAO
        {
            get { return currentRootPagedDAO; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ISessionManager SessionProvider
        {
            get { return this.sessionProvider; }
        }

        [TestFixtureSetUp]
        public void OnStartUp()
        {
            SetRootPathProject();

            XmlTextReader configReader = new XmlTextReader(new MemoryStream(NHibernate.Test.Properties.Resources.Configuration));
            DirectoryInfo dir = new DirectoryInfo(this.rootPathProject + ".Hbm");
            Console.WriteLine(dir);

            builder = new NhConfigurationBuilder(configReader, dir);

            builder.SetProperty("connection.connection_string", GetConnectionString());

            try
            {
                builder.BuildSessionFactory();
                sessionFactory = builder.SessionFactory;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            
            sessionProvider = new SessionManager(sessionFactory);
            currentPagedDAO = new EnterprisePagedDAO(sessionProvider);
            currentRootPagedDAO = new EnterpriseRootDAO<object>(sessionProvider);
            currentSession = sessionFactory.OpenSession();
        }

        [TestFixtureTearDown]
        public void OnFinishedTest()
        {
            if (currentSession != null && currentSession.IsOpen)
            {
                currentSession.Close();
                currentSession.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetRootPathProject()
        {
            var list = new List<string>(Directory.GetCurrentDirectory().Split('\\'));
            list.RemoveAt(list.Count - 1);
            list.RemoveAt(list.Count - 1);
            list.Add(string.Empty);
            this.rootPathProject = string.Join("\\", list.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string output = this.rootPathProject + "db\\";
            var str = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            return string.Format(str, output);
            //return str;
        }

        [SetUp]
        public void BindSession()
        {
            //IStatelessSession ss = sessionFactory.OpenStatelessSession();
            //CurrentSessionContext.Bind(ss);
            CurrentSessionContext.Bind(currentSession);
        }

        [TearDown]
        public void UnBindSession()
        {
            CurrentSessionContext.Unbind(sessionFactory);
        }

        /// <summary>
        /// Discard the current internal session, opening new one.
        /// </summary>
        public void DiscardCurrentSession()
        {
            lock (this)
            {
                this.UnBindSession();
                if (this.currentSession != null)
                {
                    this.currentSession.Close();
                    this.currentSession = sessionFactory.OpenSession();
                }
                this.BindSession();
            }
        }


        public ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }
    }
}
