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
            public tblReserva reserva { get; set; }
            public List<int?> idPersonas { get; set; }
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
        public IHttpActionResult GetReservaById(int idPersona)
        {
            if (!UserExists(idPersona))
            {
                log.Ok = false;
                log.ErrorMessage = "El usuario no existe";
            }

            var v = db.tblReserva.Where(r => r.idReservante == idPersona).ToList();
            Good(log);
            if (v.Count != 0)
            {
                var reserva = v.ToList();

                var result = MergeLogResult(log, reserva);
                return Ok(result);
            }
            else
            {
                List<GetReservaById_sp> getList = new List<GetReservaById_sp>();
                var id = db.GetReserva(idPersona).Select(r => r.idReserva).ToList();
                foreach (var idres in id)
                {
                    getList.Add(db.GetReservaById(idres).First());
                }
                //var reservaById = db.GetReservaById(id).ToList();
                var result = MergeLogResult(log, getList);
                return Ok(result);
            }

        }

        //POST: api/Reserva
        [HttpPost]
        [ResponseType(typeof(ReservaPersonas))]
        [ActionName("Add")]
        public IHttpActionResult AddReserva([FromBody] ReservaPersonas reservaP)
        {
            try
            {
                db.tblReserva.Add(reservaP.reserva);
                db.SaveChanges();
                Good(log);
            }
            catch (Exception)
            {
                log.Ok = false;
                log.ErrorMessage = "Error al agregar la reserva ";
            }

            if (reservaP.idPersonas.Count != 0)
            {
                foreach (var persona in reservaP.idPersonas)
                {
                    tblGrupoReserva grupo = new tblGrupoReserva() { idReserva = reservaP.reserva.idReserva, idPersona = persona.Value};
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
    }
}
