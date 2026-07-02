using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Data;
using Dsw2026Ej15.Api.DTOs;

namespace Dsw2026Ej15.Extensions
{
    public static class DataExtensions
    {
        public static void SeedworkSpecialities(this Dsw2026Ej15DbContext context, string dataSource)
        {
            if (context.Set<Speciality>().Any()) return;

            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Sources", dataSource));

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var entities = JsonSerializer.Deserialize<List<SpecialityDto>>(json, options) ?? [];

            var specialities = entities.Select(s => new Speciality(s.Name, s.Description, s.Id));

            context.Set<Speciality>().AddRange(specialities);
            context.SaveChanges();
        }
    }
}