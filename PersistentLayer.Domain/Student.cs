using System;

namespace PersistentLayer.Domain
{
    [Serializable]
    public class Student
    {
        private Guid code;
        private string name;
        private string surname;
        private string email;

        public virtual Guid Code
        {
            protected set
            {
                this.code = value;
            }
            get
            {
                return this.code;
            }
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

        public override bool Equals(object obj)
        {
            if (obj is Student)
            {
                return this.GetHashCode() == obj.GetHashCode();
            }
            return false;

        }

        public override int GetHashCode()
        {
            return (this.Name != null ? this.Name.GetHashCode() : 0) +
                (this.Surname != null ? this.Surname.GetHashCode() : 0);
        }
    }
}
