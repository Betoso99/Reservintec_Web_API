using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiReserva.Models;
using WebApiReserva.Utilities;
using static WebApiReserva.Utilities.LogUtilities;

namespace WebApiReserva.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EdificioController : ApiController
    {
        private ReservaEntities db = new ReservaEntities();
        private Logger log = new Logger();

        /// <summary>
        /// Obtiene todos los edificios registradas
        /// </summary>
        // GET: api/Edificio
        [HttpGet]
        [ActionName("GetAll")]
        public IHttpActionResult GetEdificio()
        {
            Good(log);
            // GetEdificio Select * from tblEdificio
            var edif = db.tblEdificio.ToList();

            var result = MergeLogResult(log, edif);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene todos los cursos de un edificio con el ID del edificio.
        /// </summary>
        // GET: api/Edificio/{id}
        [HttpGet]
        [ActionName("GetCursos")]
        public IHttpActionResult GetCursosEdificio(int id)
        {
            // GetCursoEdif Select top 1 * from 
            var curso = db.tblCurso.Where(c => c.idEdificio == id).ToList();

            if (curso == null)
            {
                log.Ok = false;
                log.ErrorMessage = "El edificio no existe";
                return Ok(log);
            }

            Good(log);
            var result = MergeLogResult(log, curso);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene todos los cursos disponibles con el dia y la semana.
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetCursosDisponibles([FromUri]Date date)
        {
            Good(log);
            if(date.idSemana > 12 || date.idDia > 7)
            {
                log.Ok = false;
                log.ErrorMessage = "Esa semana/dia no existe";
                return Ok(log);
            }

            var cursosDisp = db.GetCursosDisponible(date.idHora, date.idDia, date.idSemana).ToList();
            List<CursoEdificio> listaResult = new List<CursoEdificio>();
            int cantidadEdificios = db.tblEdificio.Select(e => e.idEdificio).ToList().Count;
            int edificioActual = 0;

            for (int i = 0; i < cantidadEdificios; i++)
            {
                CursoEdificio cursoEdificio = new CursoEdificio();
                edificioActual = i+1;
                cursoEdificio.Edificio = db.tblEdificio.Where(e => e.idEdificio == edificioActual).Select(l => l.Edificio).FirstOrDefault();

                for (int j = 0; j < cursosDisp.Count; j++)
                {
                    var curso = cursosDisp[j];
                    if(db.tblCurso.Where(c => c.idEdificio == edificioActual).Select(c => c.idEdificio).FirstOrDefault() == curso.idEdificio)
                    {
                        //var numCurso = db.tblCurso.Where(c => c.idCurso == idcursoActual).Select(l => l.NumCurso).FirstOrDefault();
                        cursoEdificio.Cursos.Add(curso);
                    }
                }
                listaResult.Add(cursoEdificio);
            }

            var result = MergeLogResult(log, listaResult);

            return Ok(result);

        }

        public class Date
        {
            public int idSemana { get; set; }
            public int idDia { get; set; }
            public int idHora { get; set; }
        }

        public class CursoEdificio
        {
            public CursoEdificio()
            {
                Cursos = new List<GetCursosDisponible_sp>();
            }
            public string Edificio { get; set; }
            public List<GetCursosDisponible_sp> Cursos { get; set; }
        }


    }
}
