using System;

namespace PersistentLayer.Domain
{
    [Serializable]
    public class Comune
    {
        private long id;
        private string nominativo;
        private string descrizione;
        private int version;

        public virtual long ID
        {
            set { this.id = value; }
            get { return this.id; }
        }

        public virtual string Nominativo
        {
            set { this.nominativo = value; }
            get { return this.nominativo; }
        }

        public virtual string Descrizione
        {
            set { this.descrizione = value; }
            get { return this.descrizione; }
        }

        protected virtual int Version
        {
            set { this.version = value; }
            get { return this.version; }
        }
    }
}
