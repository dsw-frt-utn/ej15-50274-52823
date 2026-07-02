using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        Task<Speciality?> GetSpecialityByIdAsync(Guid id);
        Task AddDoctorAsync(Doctor doctor);
        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync();
        Task<Doctor?> GetActiveDoctorByIdAsync(Guid id);
        Task<bool> DeactivateDoctorAsync(Guid id);
    }
}
