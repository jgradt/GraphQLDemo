﻿using System;
using System.Collections.Generic;

namespace GraphQLDemo.Data.Entities
{
    public class Customer : IDataEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public List<Order> Orders { get; set; }
    }
}
