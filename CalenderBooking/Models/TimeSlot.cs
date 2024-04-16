using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CalenderBooking.Models
{
    public class TimeSlot
    {
        public DateTime StartTime { get; set; }
        
        [Key]
        public int TimeSlotId {get; set;}

    }
}