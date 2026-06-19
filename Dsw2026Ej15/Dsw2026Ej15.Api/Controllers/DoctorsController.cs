using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IPersistence _persistence;

    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }
    [HttpPost]
    public IActionResult Creatre(CreateDoctorRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Nombre es requerido");
        }
        if(string.IsNullOrEmpty(request.LicenseNumber))
        {
            return BadRequest("Numero de Licencia es requerido");
        }
        var speciality = _persistence.Specialities
            .FirstOrDefault(s => s.Id == request.SpecialityId);
        if (speciality == null)
        {
            return BadRequest("La especialidad que busca no existe");
        }
        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            LicenseNumber = request.LicenseNumber,
            IsActive = true,
            Speciality = speciality
        };
        _persistence.Doctors.Add(doctor);
        return Created($"/api/doctors/{doctor.Id}", doctor);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var activeDoctors = _persistence.Doctors
            .Where(d => d.IsActive)
            .ToList();
        return Ok(activeDoctors);
    }
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var doctor = _persistence.Doctors
            .FirstOrDefault(d => d.Id == id && d.IsActive);
        if (doctor == null)
        {
            return NotFound();
        }

        var response = new DoctorResponse
        {
            Name = doctor.Name,
            LicenseNumber = doctor.LicenseNumber,
            SpecialityName = doctor.Speciality?.Name ?? string.Empty,
        };
        return Ok(response);
    }

}

