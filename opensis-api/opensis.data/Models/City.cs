﻿using System;
using System.Collections.Generic;

namespace opensis.data.Models
{
    public partial class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? StateId { get; set; }

        public virtual State State { get; set; }
    }
}
