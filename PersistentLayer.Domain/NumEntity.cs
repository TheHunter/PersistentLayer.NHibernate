using EntityModel;
using EntityModel.Notifiers;

namespace PersistentLayer.Domain
{
    public class NumEntity
        : ObservableEntity<long>
    {
        private string testo = null;

        public virtual string Testo
        {
            get { return this.testo; }
            set { this.testo = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj is NumEntity)
            {
                return this.GetHashCode() == obj.GetHashCode();
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.Testo == null ? string.Empty.GetHashCode() : this.Testo.GetHashCode());
        }
    }
}
