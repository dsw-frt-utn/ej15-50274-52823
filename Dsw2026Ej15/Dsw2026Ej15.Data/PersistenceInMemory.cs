using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Dsw2026Ej15.Data
{
    public class PersistenceInMemory : IPersistence
    {
        private readonly List<Doctor> _doctors = new();
        private readonly List<Speciality> _specialities = new();

        public PersistenceInMemory()
        {
            LoadSpecialities();
        }

        private void LoadSpecialities()
        {
            try
            {
                var json = File.ReadAllText("specialities.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var specialities = JsonSerializer.Deserialize<List<Speciality>>(json, options);

                if (specialities != null)
                {
                    _specialities.AddRange(specialities);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cargando especialidades: {ex.Message}");
            }
        }

        public Speciality? GetSpecialityById(Guid id) => _specialities.FirstOrDefault(s => s.Id == id);

        public void AddDoctor(Doctor doctor) => _doctors.Add(doctor);

        public IEnumerable<Doctor> GetActiveDoctors() => _doctors.Where(d => d.IsActive).ToList();

        public Doctor? GetActiveDoctorById(Guid id) => _doctors.FirstOrDefault(d => d.Id == id && d.IsActive);

        public bool DeactivateDoctor(Guid id)
        {
            var doctor = GetActiveDoctorById(id);
            if (doctor == null) return false;

            doctor.IsActive = false;
            return true;
        }
    }
}
