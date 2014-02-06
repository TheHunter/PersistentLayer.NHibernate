//using System.Collections.Generic;

namespace PersistentLayer.NHibernate.Test.PocoPrj
{
    public class SalesmanPrj
    {
        private long? id;
        private string name;
        private string surname;
        private string email;

        /// <summary>
        /// 
        /// </summary>
        public SalesmanPrj(){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="email"></param>
        public SalesmanPrj(long? id, string name, string surname, string email)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.email = email;
        }

        public virtual long? ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public virtual string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public virtual string Surname
        {
            get { return this.surname; }
            set { this.surname = value; }
        }

        public virtual string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

    }
}
