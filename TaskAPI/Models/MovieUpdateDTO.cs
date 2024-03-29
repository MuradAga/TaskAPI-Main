﻿namespace TaskAPI.Models
{
    public class MovieUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public string TrailerUrl { get; set; }
        public ushort Year { get; set; }
        public bool Language { get; set; }
        public float ImdbPoint { get; set; }
    }
}
