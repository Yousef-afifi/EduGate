using EduGate.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduGate.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateOnly Start_Date { get; set; }
        public DateOnly End_Date { get; set; }
        public SubscriptionStatus Status { get; set; }
        [ForeignKey("package")]
        public int Package_Id { get; set; }
        public Package package { get; set; }
        [ForeignKey("teacher")]
        public int Teacher_Id {  get; set; }
        public Teacher teacher { get; set; }
    }
}
