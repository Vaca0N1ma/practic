//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace practic
{
    using System;
    using System.Collections.Generic;
    
    public partial class employeer
    {
        public employeer()
        {
            this.task = new HashSet<task>();
            this.task1 = new HashSet<task>();
        }
    
        public int id { get; set; }
        public string fullname { get; set; }
        public int experience { get; set; }
        public string login { get; set; }
        public string password { get; set; }
    
        public virtual user_role user_role { get; set; }
        public virtual ICollection<task> task { get; set; }
        public virtual ICollection<task> task1 { get; set; }

        public override string ToString()
        {
            return fullname;
        }
    }
}
