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
            // GetEdificio Select * from tblEdificio
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
            // GetCursoEdif Select  * from 
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
            //List<CursoEdificio> listaResult = new List<CursoEdificio>();
            //int cantidadEdificios = db.tblEdificio.Select(e => e.idEdificio).ToList().Count;
            //int edificioActual = 0;

            //var EdificioActual = db.GetEdifActual(edificioActual);
            //List<tblCurso> cursosDsiponible = db.GetCursosDisponible(8, 1, 1).ToList();


            List<CursoEdificio> cursoEdificio = new List<CursoEdificio>();

            foreach (tblEdificio edificio in db.GetEdificios())
            {
                cursoEdificio.Add(new CursoEdificio() { edificio = edificio, cursos = new List<tblCurso>() });
            }

            foreach (tblCurso curso in cursosDisp)
            {
                cursoEdificio[curso.idEdificio - 1].cursos.Add(curso);
            }

            //for (int i = 0; i < cantidadEdificios; i++)
            //{
            //    CursoEdificio cursoEdificio = new CursoEdificio();
            //    edificioActual = i+1;
            //    //cursoEdificio.Edificio = db.tblEdificio.Where(e => e.idEdificio == edificioActual).Select(l => l.Edificio).FirstOrDefault(); // - el que funcionaba
            //    cursoEdificio.Edificio = db.GetEdifActual(edificioActual).ToString(); // -sp
            //    for (int j = 0; j < cursosDisp.Count; j++)
            //    {
            //        var curso = cursosDisp[j];
            //        if(/*db.tblCurso.Where(c => c.idEdificio == edificioActual).Select(c => c.idEdificio).FirstOrDefault() // -el que funcionaba*/ db.GetIdEdificio(edificioActual).First() == curso.idEdificio)
            //        {
            //            cursoEdificio.Cursos.Add(curso);
            //        }
            //    }
            //    listaResult.Add(cursoEdificio);
            //}

            var result = MergeLogResult(log, cursoEdificio);

            return Ok(result);

        }

        [HttpGet]
        public IHttpActionResult GetCursoEdificio(int id) // idCurso
        {
            tblCurso curso = db.tblCurso.Where(c => c.idCurso == id).FirstOrDefault();

            int idEdificio = curso.idEdificio;

            return Ok();
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
