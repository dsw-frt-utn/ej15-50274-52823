using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Data
{
    public class PersistenceEF : IPersistence
    {
        private readonly ApplicationDbContext _context;

        public PersistenceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public IEnumerable<Doctor> GetActiveDoctors()
        {
            return _context.Doctors
                .Include(d => d.Speciality)
                .Where(d => d.IsActive)
                .ToList();
        }

        public Doctor? GetActiveDoctorById(Guid id)
        {
            return _context.Doctors
                .Include(d => d.Speciality)
                .FirstOrDefault(d => d.Id == id && d.IsActive);
        }

        public Speciality? GetSpecialityById(Guid id)
        {
            return _context.Specialities
                .FirstOrDefault(s => s.Id == id);
        }

        public bool DeactivateDoctor(Guid id)
        {
            var doctor = GetActiveDoctorById(id);

            if (doctor == null)
                return false;

            doctor.IsActive = false;
            _context.SaveChanges();

            return true;
        }
    }
}