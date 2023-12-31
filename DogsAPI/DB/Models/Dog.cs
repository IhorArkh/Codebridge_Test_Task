﻿using System.ComponentModel.DataAnnotations;

namespace DogsAPI.DB.Models
{
    public class Dog
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public int? TailLength { get; set; }

        public int? Weight { get; set; }
    }
}
