﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiReserva.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ReservaEntities : DbContext
    {
        public ReservaEntities()
            : base("name=ReservaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblCarrera> tblCarrera { get; set; }
        public virtual DbSet<tblClase> tblClase { get; set; }
        public virtual DbSet<tblCurso> tblCurso { get; set; }
        public virtual DbSet<tblDias> tblDias { get; set; }
        public virtual DbSet<tblDocente> tblDocente { get; set; }
        public virtual DbSet<tblEdificio> tblEdificio { get; set; }
        public virtual DbSet<tblEstudiante> tblEstudiante { get; set; }
        public virtual DbSet<tblGrupoReserva> tblGrupoReserva { get; set; }
        public virtual DbSet<tblHoras> tblHoras { get; set; }
        public virtual DbSet<tblPersona> tblPersona { get; set; }
        public virtual DbSet<tblPersonaTipo> tblPersonaTipo { get; set; }
        public virtual DbSet<tblReserva> tblReserva { get; set; }
        public virtual DbSet<tblSemana> tblSemana { get; set; }
        public virtual DbSet<tblTipo> tblTipo { get; set; }
        public virtual DbSet<tblTipoTipo> tblTipoTipo { get; set; }
        public virtual DbSet<tblUsuario> tblUsuario { get; set; }
        public virtual DbSet<database_firewall_rules> database_firewall_rules { get; set; }
    
        public virtual ObjectResult<GetCursoEdificio_sp> GetCursoEdificio(Nullable<int> idEdificio)
        {
            var idEdificioParameter = idEdificio.HasValue ?
                new ObjectParameter("idEdificio", idEdificio) :
                new ObjectParameter("idEdificio", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCursoEdificio_sp>("GetCursoEdificio", idEdificioParameter);
        }
    
        public virtual ObjectResult<string> GetPassword(Nullable<int> idMatricula)
        {
            var idMatriculaParameter = idMatricula.HasValue ?
                new ObjectParameter("idMatricula", idMatricula) :
                new ObjectParameter("idMatricula", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetPassword", idMatriculaParameter);
        }
    
        public virtual ObjectResult<GetReserva_sp> GetReserva(Nullable<int> idPersona)
        {
            var idPersonaParameter = idPersona.HasValue ?
                new ObjectParameter("idPersona", idPersona) :
                new ObjectParameter("idPersona", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReserva_sp>("GetReserva", idPersonaParameter);
        }
    
        public virtual ObjectResult<GetReservaById_sp> GetReservaById(Nullable<int> idReserva)
        {
            var idReservaParameter = idReserva.HasValue ?
                new ObjectParameter("idReserva", idReserva) :
                new ObjectParameter("idReserva", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReservaById_sp>("GetReservaById", idReservaParameter);
        }
    
        public virtual ObjectResult<GetReservaSemana_sp> GetReservaSemana(Nullable<int> numeroSemana)
        {
            var numeroSemanaParameter = numeroSemana.HasValue ?
                new ObjectParameter("numeroSemana", numeroSemana) :
                new ObjectParameter("numeroSemana", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReservaSemana_sp>("GetReservaSemana", numeroSemanaParameter);
        }
    
        public virtual ObjectResult<GetUsuario_sp> GetUsuario(Nullable<int> idUsuario)
        {
            var idUsuarioParameter = idUsuario.HasValue ?
                new ObjectParameter("idUsuario", idUsuario) :
                new ObjectParameter("idUsuario", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUsuario_sp>("GetUsuario", idUsuarioParameter);
        }
    }
}
