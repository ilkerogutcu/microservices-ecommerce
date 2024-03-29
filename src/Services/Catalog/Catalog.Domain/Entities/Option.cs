﻿using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Option : BaseEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool IsActive { get; set; }
    }
}