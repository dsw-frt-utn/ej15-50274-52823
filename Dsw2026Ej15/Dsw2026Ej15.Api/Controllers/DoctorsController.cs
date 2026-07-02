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

            var speciality = await _persistence.GetSpecialityByIdAsync(request.SpecialityId);
            if (speciality == null)
                throw new ValidationException("La especialidad indicada no existe.");

            var doctor = new Doctor(
                name: request.Name,
                licenseNumber: request.LicenseNumber,
                speciality: speciality
            );

            await _persistence.AddDoctorAsync(doctor);

            return Created(string.Empty, null);
           
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllActiveDoctors()
        {
            var activeDoctors = await _persistence.GetActiveDoctorsAsync();
            return Ok(activeDoctors);
        }

        [HttpGet("doctors/{id:guid}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var doctor = await _persistence.GetActiveDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();

            var response = new DoctorResponseDto(
                doctor.Name,
                doctor.LicenseNumber,
                doctor.Speciality?.Name ?? "Sin especialidad"
            );

            return Ok(response);
        }

        [HttpDelete("doctors/{id:guid}")]
        public async Task<IActionResult> DeleteDoctor(Guid id)
        {
            var success = await _persistence.DeactivateDoctorAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}