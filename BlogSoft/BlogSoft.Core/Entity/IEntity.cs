using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSoft.Core.Entity
{
    public interface IEntity<T>
    {
        T ID { get; set; }
    }
}
