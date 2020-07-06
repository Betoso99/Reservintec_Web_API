using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiReserva.Models;
using WebApiReserva.Utilities;
using static WebApiReserva.Utilities.LogUtilities;


namespace WebApiReserva.Controllers
{
    public class UserController : ApiController
    {
        private ReservaEntities db = new ReservaEntities();
        private Logger log = new Logger();


        // GET: api/User
        [HttpGet]
        public IHttpActionResult GetUsuario()
        {
            var usuario = db.tblPersona.ToList();
            Good(log);

            var result = MergeLogResult(log, usuario);
            return Ok(result);
        }

        // GET: api/User/5
        [ResponseType(typeof(tblPersona))]
        [HttpGet]
        public IHttpActionResult GetUsuarioById(int id)
        {
            if (!UserExists(id))
            {
                log.Ok = false;
                log.ErrorMessage = "El usuario no existe";
                return Ok(log);
            }

            var user = db.GetUsuario(id).ToList();
            var result = MergeLogResult(log, user);

            return Ok(result);
        }


        // POST: api/User
        //[ResponseType(typeof(tblPersona))]
        [HttpPost]
        public IHttpActionResult ValidateUserRegister([FromBody]tblPersona user)
        {
            if (!UserExists(user.idPersona))
            {
                user.Pass = CryptoPass.Hash(user.Pass);

                try
                {
                    db.Entry(user).State = EntityState.Added;
                    db.tblPersona.Add(user);
                    db.SaveChanges();
                    Good(log);
                }
                catch (Exception)
                {
                    log.Ok = false;
                    log.ErrorMessage = "Hubo un problema al agregar el usuario";
                }

            }
            else
            {
                log.Ok = false;
                log.ErrorMessage = "La matricula ya esta registrada";
            }

            return Ok(log);
        }

        // POST: api/User
        [HttpPost]
        public IHttpActionResult ValidateUserLogin([FromBody]tblPersona user)
        {
            var v = db.tblPersona.Where(u => u.idPersona == user.idPersona).FirstOrDefault();
            if (v == null)
            {
                log.Ok = false;
                log.ErrorMessage = "Matricula no registrada";
                return Ok(log);
            }

            //int id = v.Select(i => i.idPersona).FirstOrDefault();
            
            var valid = db.GetPassword(v.idPersona).FirstOrDefault();

            if (CryptoPass.Hash(user.Pass) == valid)
            {
                Good(log);
            }
            else
            {
                log.Ok = false;
                log.ErrorMessage = "Usuario/contraseña no valida";
            }

            return Ok(log);
        }

        // PUT: api/User/5
        [HttpPut]
        public IHttpActionResult PuttblPersona(int id, tblPersona tblPersona)
        {

            if (id != tblPersona.idPersona)
            {
                return BadRequest();
            }

            db.Entry(tblPersona).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                log.Ok = false;
                if (!UserExists(id))
                {
                    log.ErrorMessage = "El usuario no existe";
                    return Ok(log);
                }
                else
                {
                    log.ErrorMessage = "Hubo un error al editar el usuario";
                    return Ok(log);
                }
            }

            return Ok(log);
        }

        public IHttpActionResult VerifyUserExists(int id)
        {
            if (db.tblPersona.Count(e => e.idPersona == id) > 0) Good(log);
            else
            {
                log.Ok = false;
                log.ErrorMessage = "El usuario no existe";
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.tblPersona.Count(e => e.idPersona == id) > 0;
        }
    }
}