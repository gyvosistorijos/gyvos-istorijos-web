using System;
using Microsoft.AspNetCore.Http;

namespace Hintme.Models
{
    public class HintDto
    {
        public string Text { get; set; }
        public IFormFile Picture { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Header { get; set; }
    }
}