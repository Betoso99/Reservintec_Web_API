using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using WebApiReserva.Models;
using WebApiReserva.Utilities;
using static WebApiReserva.Utilities.LogUtilities;

namespace WebApiReserva.Controllers
{
    public class ReservaController : ApiController
    {
        private ReservaEntities db = new ReservaEntities();
        private Logger log = new Logger();

        public class ReservaPersonas
        {
            public tblReserva Reserva { get; set; }
            public List<int?> IdPersonas { get; set; }
        }

        // GET: api/Reserva
        public IHttpActionResult GetReserva()
        {
            Good(log);
            var reserva = db.tblReserva.ToList();
            var result = MergeLogResult(log, reserva);
            return Ok(result);
        }


        // GET: api/Reserva/5
        [HttpGet()]
        [ActionName("GetSemana")]
        public IHttpActionResult GetHorarioBySemana(int id) // id = numeroSemana
        {
            /* Method to get Horario de Reservas by Semana */

            var semana = db.GetReservaSemana(id).ToList();

            List<int[]> horario = GetSemanaList(semana);

            var result = MergeLogResult(log, horario);

            return Ok(result.ToList());
        }

        // GET: api/Reserva
        [HttpGet]
        public IHttpActionResult GetReservaById(int idReserva)
        {
            if (!ReservaExists(idReserva))
            {
                log.Ok = false;
                log.ErrorMessage = "Esta reserva no existe";
            }
            List<int> idList = new List<int>();
            int idPersona = db.tblReserva.Where(r => r.idReserva == idReserva).Select(c => c.idReservante).FirstOrDefault();
            idList.Add(idPersona);
            var idsGrupo = db.tblGrupoReserva.Where(r => r.idReserva == idReserva).Select(c => c.idPersona).ToList();
            
            if(idsGrupo != null) foreach (int ids in idsGrupo) idList.Add(ids);

            List<Persona> personas = new List<Persona>();
            for (int i = 0; i < (idsGrupo.Count() + 1); i++)
            {               
                int idPersonaActual = idList[i];
                int id = db.tblPersona.Where(c => c.idPersona == idPersonaActual).Select(c => c.idPersona).FirstOrDefault();
                string name = db.tblPersona.Where(c => c.idPersona == idPersonaActual).Select(c => c.Nombre).FirstOrDefault();
                Persona persona = new Persona() { IdPersona = id, Nombre= name};
                personas.Add(persona);
            }

            var result = MergeLogResult(log, personas);
            return Ok(result);

        }

        public class Persona
        {
            public Persona()
            {
            }
            public int IdPersona { get; set; }
            public string Nombre { get; set; }
        }

        //POST: api/Reserva
        [HttpPost]
        //[ResponseType(typeof(ReservaPersonas))]
        [ActionName("Add")]
        public IHttpActionResult AddReserva([FromBody] ReservaPersonas reservaP)
        {
            try
            {
                db.tblReserva.Add(reservaP.Reserva);
                db.SaveChanges();
                Good(log);
            }
            catch (Exception ex)
            {
                log.Ok = false;
                log.ErrorMessage = "Error al agregar la reserva " + ex;
                return Ok(log);
            }

            if (reservaP.IdPersonas.Count != 0)
            {
                foreach (var persona in reservaP.IdPersonas)
                {
                    tblGrupoReserva grupo = new tblGrupoReserva() { idReserva = reservaP.Reserva.idReserva, idPersona = persona.Value};
                    try
                    {
                        db.tblGrupoReserva.Add(grupo);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        log.Ok = false;
                        log.ErrorMessage = "Hubo un error al agregar los integrantes";
                    }

                }
            }

            return Ok(log);
        }

        // PUT: api/Reserva/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]tblReserva reserva)
        {
            Good(log);
            var v = db.tblReserva.Where(r => r.idReservante == id).FirstOrDefault();
            if (v == null)
            {
                log.Ok = false;
                log.ErrorMessage = "El usuario no ha hecho una reserva";
                return Ok(log);
            }

            try
            {
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                log.Ok = false;
                log.ErrorMessage = "Hubo un error al editar el usuario";
                return Ok(log);
            }

            return Ok(log);

        }

        [NonAction]
        private int[] GetHoras(int n, List<GetReservaSemana_sp> semana)
        {
            int[] dia = new int[16];
            var clase = db.tblClase.ToList();

            foreach (var c in clase)
            {
                if (c.idDias == n)
                {
                    tblHoras horaIn = db.tblHoras.Where(h => h.idHoras == c.idHoraIn).FirstOrDefault();
                    tblHoras horaF = db.tblHoras.Where(h => h.idHoras == c.idHoraF).FirstOrDefault();
                    for (int l = 0; l < 16; l++)
                    {
                        if (l >= (horaIn.Horas - 7) && l <= (horaF.Horas - 7))
                        {
                            if (dia[l] == 1) dia[l] = 3; // Solapado
                            else dia[l] = 2; // Clase
                        }

                    }

                    //for (int i = idHoraIn; i < (idHoraF + 1); i++)
                    //{
                    //    dia[i] = 2 //Clase
                    //}

                }

            }

            foreach (var d in semana)
            {
                if (d.idDias == n)
                {                   
                    tblHoras horaIn = db.tblHoras.Where(h => h.idHoras == d.idHoraIn).FirstOrDefault();
                    tblHoras horaF = db.tblHoras.Where(h => h.idHoras == d.idHoraF).FirstOrDefault();
                    for (int i = 0; i < 16; i++)
                    {                       
                        if (i >= (horaIn.Horas - 7) && i <= (horaF.Horas - 7))
                        {
                            if (dia[i] == 2) dia[i] = 3; // Solapado
                            else dia[i] = 1; // Reservado
                        }
                        else if (dia[i] != 2)
                        {
                            dia[i] = 0; // Disponible    
                        }

                    }
                }
            }
            return dia;
        }

        [NonAction]
        private List<int[]> GetSemanaList(List<GetReservaSemana_sp> semana)
        {
            List<int[]> semanaList = new List<int[]>();

            for (int i = 1; i < 8; i++)
            {
                semanaList.Add(GetHoras(i, semana));
            }
            return semanaList;
        }

        private bool UserExists(int id)
        {
            return db.tblPersona.Count(e => e.idPersona == id) > 0;
        }

        private bool ReservaExists(int id)
        {
            return db.tblReserva.Count(e => e.idReserva == id) > 0;
        }
    }
}
