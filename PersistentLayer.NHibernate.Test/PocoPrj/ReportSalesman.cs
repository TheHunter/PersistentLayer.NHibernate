using EntityModel;

namespace PersistentLayer.NHibernate.Test.PocoPrj
{
    public class ReportSalesman
        : BaseEntity<long>
    {
        public long ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Surname
        {
            get;
            set;
        }

        public long NumSubAgents
        {
            get;
            set;
        }
    }
}
