using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyHelloWorld.Models
{
    public class ComputerSnake
    {
        public int computer_id { get; set; }
        public string motherboard { get; set; } = "";
        public int? cpu_cores { get; set; } = 0;
        public bool has_wifi { get; set; }
        public bool has_lte { get; set; }
        public DateTime? release_date { get; set; }
        public decimal price { get; set; }
        public string video_card { get; set; } = "";

        // public ComputerSnake(
        //     string motherBoard,
        //     int cpuCores,
        //     bool hasWifi,
        //     bool hasLTE,
        //     decimal price,
        //     string videoCard,
        //     string releaseDate
        // ){

        //     Motherboard = motherBoard;
        //     CPUCores = cpuCores;
        //     HasWifi = hasWifi;
        //     HasLTE = hasLTE;
        //     ReleaseDate = Convert.ToDateTime(releaseDate);
        //     Price = price;
        //     VideoCard = videoCard;
        // } 
    }
}
