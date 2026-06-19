using Microsoft.AspNetCore.Mvc;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Api.DTOs;
using System.Threading.Tasks;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorCreateDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(request.LicenseNumber))
                throw new ValidationException("El número de matrícula (licencia) es requerido.");

            var speciality = _persistence.GetSpecialityById(request.SpecialityId);
            if (speciality == null)
                throw new ValidationException("La especialidad indicada no existe.");

            var doctor = new Doctor
            {
                Name = request.Name,
                LicenseNumber = request.LicenseNumber,
                Speciality = speciality,
                IsActive = true
            };

            _persistence.AddDoctor(doctor);

            return Created(string.Empty, null);
            return Created(string.Empty, null);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllActiveDoctors()
        {
            var activeDoctors = _persistence.GetActiveDoctors();
            return Ok(activeDoctors);
        }

        [HttpGet("doctors/{id:guid}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var doctor = _persistence.GetActiveDoctorById(id);
            if (doctor == null)
                return NotFound();

            var response = new DoctorResponseDto(
                doctor.Name,
                doctor.LicenseNumber,
                doctor.Speciality.Name);

            return Ok(response);
        }

        [HttpDelete("doctors/{id:guid}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var success = _persistence.DeactivateDoctor(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}