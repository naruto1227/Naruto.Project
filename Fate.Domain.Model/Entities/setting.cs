using System;
using System.Collections.Generic;
using System.Text;
using Fate.Infrastructure.BaseRepository.Model;
using System.ComponentModel.DataAnnotations;

namespace Fate.Domain.Model.Entities
{
    public class setting : IEntity
    {
        public int Id { get; set; }
        public string DuringTime { get; set; }
        public string Rule { get; set; }
        [ConcurrencyCheck]
        public string Contact { get; set; }
        public string Description { get; set; }
        public int Integral { get; set; }
    }
}
