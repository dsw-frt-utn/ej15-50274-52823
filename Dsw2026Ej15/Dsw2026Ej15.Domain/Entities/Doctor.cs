using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        
        public Guid? SpecialityId {  get; set; }

        public Speciality? Speciality { get; set; } 

        private Doctor()
        {
        }

        public Doctor(string name, string licenseNumber, Speciality speciality, Guid? id = null) : base(id)
        {
            Name = name;
            LicenseNumber = licenseNumber;
            Speciality = speciality;
            SpecialityId = speciality?.Id;
            IsActive = true;
        }
    }
}
