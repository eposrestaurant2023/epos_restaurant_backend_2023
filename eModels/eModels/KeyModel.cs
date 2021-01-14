using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eModels
{
    public  class KeyModel
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
    }
}
