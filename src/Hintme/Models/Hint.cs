using System;
using Microsoft.AspNetCore.Http;

namespace Hintme.Models
{
    public class Hint
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Header { get; set; }

        public Hint()
        {
            Id= Guid.NewGuid();
        }
    }
}