using System;
using System.Collections.Generic;
using System.Text;

namespace Fate.Domain.Model.Entities
{
   public class fy_download : IEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Code { get; set; }
    }
}
