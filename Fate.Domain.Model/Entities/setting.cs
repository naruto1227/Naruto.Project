using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Domain.Model.Entities
{
    public class setting : Fate.Common.Interface.IEntity
    {
        public int Id { get; set; }
        public string DuringTime { get; set; }
        public string Rule { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }
        public int Integral { get; set; }
    }
}
