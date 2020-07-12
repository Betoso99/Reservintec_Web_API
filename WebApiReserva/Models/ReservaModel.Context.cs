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
            Configuration.ProxyCreationEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
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
    
        public virtual int AddUser(Nullable<int> matricula, string pass)
        {
            var matriculaParameter = matricula.HasValue ?
                new ObjectParameter("Matricula", matricula) :
                new ObjectParameter("Matricula", typeof(int));
    
            var passParameter = pass != null ?
                new ObjectParameter("Pass", pass) :
                new ObjectParameter("Pass", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddUser", matriculaParameter, passParameter);
        }
    
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
    
        public virtual ObjectResult<GetPersona_sp> GetPersona()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPersona_sp>("GetPersona");
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
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int AddReserseva(Nullable<int> idCurso, Nullable<int> idSemana, Nullable<int> idDia, Nullable<int> idHoraIn, Nullable<int> idHoraF, Nullable<int> idReservante, Nullable<System.DateTime> fechaRegsitro)
        {
            var idCursoParameter = idCurso.HasValue ?
                new ObjectParameter("idCurso", idCurso) :
                new ObjectParameter("idCurso", typeof(int));
    
            var idSemanaParameter = idSemana.HasValue ?
                new ObjectParameter("idSemana", idSemana) :
                new ObjectParameter("idSemana", typeof(int));
    
            var idDiaParameter = idDia.HasValue ?
                new ObjectParameter("idDia", idDia) :
                new ObjectParameter("idDia", typeof(int));
    
            var idHoraInParameter = idHoraIn.HasValue ?
                new ObjectParameter("idHoraIn", idHoraIn) :
                new ObjectParameter("idHoraIn", typeof(int));
    
            var idHoraFParameter = idHoraF.HasValue ?
                new ObjectParameter("idHoraF", idHoraF) :
                new ObjectParameter("idHoraF", typeof(int));
    
            var idReservanteParameter = idReservante.HasValue ?
                new ObjectParameter("idReservante", idReservante) :
                new ObjectParameter("idReservante", typeof(int));
    
            var fechaRegsitroParameter = fechaRegsitro.HasValue ?
                new ObjectParameter("FechaRegsitro", fechaRegsitro) :
                new ObjectParameter("FechaRegsitro", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddReserseva", idCursoParameter, idSemanaParameter, idDiaParameter, idHoraInParameter, idHoraFParameter, idReservanteParameter, fechaRegsitroParameter);
        }
    
        public virtual ObjectResult<getLastReserva_sp> getLastReserva()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getLastReserva_sp>("getLastReserva");
        }
    }
}
