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
    
    public partial class task_status
    {
        public task_status()
        {
            this.task = new HashSet<task>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<task> task { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}