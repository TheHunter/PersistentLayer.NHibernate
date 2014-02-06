using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PersistentLayer.Domain
{
    [Serializable]
    public class SalesmanNick
    {
        private string code = string.Empty;
        private string description = null;
        private ICollection<Salesman> consultants = null;

        public virtual string Code
        {
            protected set { this.code = value; }
            get { return this.code; }
        }

        public virtual string Description
        {
            protected set { this.description = value; }
            get { return this.description; }
        }

        public virtual ICollection<Salesman> Consultants
        {
            get { return this.consultants; }
            set { this.consultants = value; }

        }
    }
}
