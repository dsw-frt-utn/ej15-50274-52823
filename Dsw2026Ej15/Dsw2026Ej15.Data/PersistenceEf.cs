using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEf : IPersistence
    {
        private readonly Dsw2026Ej15DbContext _context;
        public PersistenceEf(Dsw2026Ej15DbContext context)
        {
            _context = context;
        }

        public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
        {
            return await _context.Specialities.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
        {
            return await _context.Doctors
                .Include(d => d.Speciality)
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<Doctor?> GetActiveDoctorByIdAsync(Guid id)
        {
            return await _context.Doctors
                .Include(d => d.Speciality)
                .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
        }

        public async Task<bool> DeactivateDoctorAsync(Guid id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null || !doctor.IsActive)
            {
                return false;
            }

            doctor.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}