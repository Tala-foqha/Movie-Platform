using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePlatform1.DAL.Models
{
    public enum EntityStatus
    {
        Active = 1,
        InActive = 2
    }
    public class AuditEntity
    {
        public string CreatedById { get; set; }
        public string ?UpdatedById {  get; set; }
        public DateTime CreatedOn {  get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;

        public DateTime UpdateddOn { get; set; }
        //عملية الربط
        public ApplicationUser CreateBy { get; set; }
        public ApplicationUser UpdateById { get; set; }

    }
}
