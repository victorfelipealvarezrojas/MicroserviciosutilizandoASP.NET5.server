﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servicios.api.Seguridad.Core.Entities;

namespace Servicios.api.Seguridad.Core.Persistence
{
  public class SeguridadContexto : IdentityDbContext<Usuario>
  {
    public SeguridadContexto(DbContextOptions options) : base(options) { }

    // heredo OnModelCreating desde IdentityDbContext
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }
  }
}
