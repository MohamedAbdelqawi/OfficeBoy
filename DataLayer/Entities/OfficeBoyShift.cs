namespace DataLayer.Entities
{
    public class OfficeBoyShift
    {
        public int Id { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }

        // Navigation property
        public string EmployeeId { get; set; } // Reference to the employee
        public virtual Employee Employee { get; set; }
    }
}
