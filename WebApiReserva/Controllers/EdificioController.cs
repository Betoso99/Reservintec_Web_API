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
            // SELECT * FROM tblEdificio
            var edif = db.GetEdificio().ToList(); // -sp
            //var edif = db.tblEdificio.ToList(); // -El que funcionaba

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
            //SELECT * FROM tblEdificio WHERE idEdificio = @idEdificio
            //var curso = db.tblCurso.Where(c => c.idEdificio == id).ToList(); // -El que funcionaba
            var curso = db.GetCursoEdificio(id).ToList(); // -sp

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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult GetCursosDisponibles([FromUri]Date date)
        {
            Good(log);
            if(date.idSemana > 12 || date.idDia > 7)
            {
                log.Ok = false;
                log.ErrorMessage = "Esa semana/dia no existe";
                return Ok(log);
            }

            List<tblCurso> cursosDisp = db.GetCursosDisponible(date.idHora, date.idDia, date.idSemana).ToList();

            List<CursoEdificio> cursoEdificio = new List<CursoEdificio>();

            foreach (tblEdificio edificio in db.GetEdificios())
            {
                cursoEdificio.Add(new CursoEdificio() { edificio = edificio, cursos = new List<tblCurso>() });
            }

            foreach (tblCurso curso in cursosDisp)
            {
                cursoEdificio[curso.idEdificio - 1].cursos.Add(curso);
            }

            var result = MergeLogResult(log, cursoEdificio);

            return Ok(result);

        }

        [HttpGet]
        public IHttpActionResult GetCursoEdificio(int id) // idCurso
        {
            //tblCurso curso = db.tblCurso.Where(c => c.idCurso == id).FirstOrDefault();
            tblCurso curso = db.GetCurso(id).FirstOrDefault(); // -sp

            string numCurso = curso.NumCurso;
            int idEdificio = curso.idEdificio;
            //string edificio = db.tblEdificio.Where(e => e.idEdificio == idEdificio).Select(e => e.Edificio).FirstOrDefault();
            string edificio = db.GetCodigoEdificio(idEdificio).FirstOrDefault(); // -sp

            string cursoEdif = edificio + numCurso;
            var result = MergeLogResult(log, cursoEdif);

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
            }
            public tblEdificio edificio { get; set; }
            public List<tblCurso> cursos { get; set; }
        }


    }
}
