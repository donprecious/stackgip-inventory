﻿using System;
using System.ComponentModel.DataAnnotations;

namespace StackgipInventory.Entities.Shared
{
    public class EntityBase
    {
        [Key]
        public string Id { get; set; }
        public EntityBase()
        {
           
            CreatedOn = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } 
        public string UpdatedBy { get; set; } 
        public string DeletedBy { get; set; } 
        public DateTime? UpdatedOn { get; set; } 
        public DateTime? DeletedOn { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
