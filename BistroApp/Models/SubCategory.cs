﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BistroApp.Models
{
    public class SubCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
