using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiReserva.Models;
using WebApiReserva.Utilities;
using static WebApiReserva.Utilities.LogUtilities;

namespace WebApiReserva.Controllers
{
    public class EdificioController : ApiController
    {
        private ReservaEntities db = new ReservaEntities();
        private Logger log = new Logger();

        // GET: api/Edificio
        [HttpGet]
        
        [ActionName("Get")]
        public IHttpActionResult GetEdificio()
        {
            Good(log);
            var edif = db.tblEdificio.ToList();

            var result = MergeLogResult(log, edif);

            return Ok(result);
        }

        // GET: api/Edificio/{id}
        [HttpGet()]
        [ActionName("GetCurso")]
        public IHttpActionResult GetCursosEdificio(int id)
        {
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

    }
}
