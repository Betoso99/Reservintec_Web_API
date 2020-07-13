using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
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

        /// <summary>
        /// Obtiene a todas las personas registradas
        /// </summary>
        // GET: api/User
        [ResponseType(typeof(tblPersona))]
        [HttpGet]
        public IHttpActionResult Get()
        {
            // SELECT * FROM tblPersona
            var usuario = db.GetPersona().ToList(); // -sp
            Good(log);

            var result = MergeLogResult(log, usuario);
            return Ok(result);
        }


        /// <summary>
        /// Obtiene los datos del usuario con el ID
        /// </summary>
        /// <param name="id">ID del usuario</param>
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

            // SELECT * FROM tblUsuario WHERE idPersona = @idPersona
            var user = db.GetUsuario(id).ToList();
            var result = MergeLogResult(log, user);

            return Ok(result);
        }

        /// <summary>
        /// Valida si la persona existe, y que el usuario no este ya registrado. Luego, la registra.
        /// </summary>
        // POST: api/User
        [ResponseType(typeof(tblUsuario))]
        [HttpPost]
        public IHttpActionResult ValidateUserRegister([FromBody]Usuario user)
        {
            if (!PersonaExists(user.Matricula))
            {
                log.Ok = false;
                log.ErrorMessage = "Esta persona no esta registrada";
                return Ok(log);
            }

            if (UserExists(user.Matricula))
            {
                log.Ok = false;
                log.ErrorMessage = "Este usuario ya esta registrado";
                return Ok(log);
            }

            try
            {
                user.Pass = CryptoPass.Hash(user.Pass);
                // INSERT INTO tblPersona (idPersona, Password) VALUES (@Matricula, @Pass)
                db.AddUser(user.Matricula, user.Pass);
                db.SaveChanges();
                Good(log);
            }
            catch (Exception)
            {
                log.Ok = false;
                log.ErrorMessage = "Hubo un problema al agregar el usuario";
            }


            return Ok(log);
        }

        /// <summary>
        /// Valida si el usuario introdujo los datos correctos a la hora de iniciar sesion.
        /// </summary>
        // POST: api/User
        [HttpPost]
        public IHttpActionResult ValidateUserLogin([FromBody]Usuario user)
        {
            Good(log);
            if (!UserExists(user.Matricula))
            {
                log.Ok = false;
                log.ErrorMessage = "ID/Matricula no registrada";
                return Ok(log);
            }
            
            // SELECT Pass FROM tblUsuario WHERE idPersona = @idPersona
            var validPassword = db.GetPassword(user.Matricula).FirstOrDefault();

            if (CryptoPass.Hash(user.Pass) != validPassword)
            {
                log.Ok = false;
                log.ErrorMessage = "Usuario/contraseña no valida";
                return Ok(log);
            }         

            return Ok(log);
        }

        /// <summary>
        /// Edita al usuario y valida que este exista.
        /// </summary>
        // PUT: api/User/5
        [HttpPut]
        public IHttpActionResult EditUser(int id, Usuario user)
        {

            if (id != user.Matricula)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

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


        /// <summary>
        /// Verifica que la persona exista a traves del ID
        /// </summary>
        public IHttpActionResult VerifyPersonaExists(int id)
        {
            // SELECT COUNT(idPersona) FROM tblPersona WHERE idPersona = @idPersona
            int idP = db.CountPersona(id).FirstOrDefault().Value;
            if (idP > 0)
            {
                Good(log);
            }
            else
            {
                log.Ok = false;
                log.ErrorMessage = "Esta persona no existe";
            }
            return Ok(log);
        }

        /// <summary>
        /// Verifica que el usuario exista a traves del ID
        /// </summary>
        public IHttpActionResult VerifyUserExists(int id)
        {
           
            if (db.tblUsuario.Count(u => u.idPersona == id) > 0)
            {
                Good(log);
            }            
            else
            {
                log.Ok = false;
                log.ErrorMessage = "El usuario no existe";
            }
            return Ok(log);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonaExists(int id)
        {
            return db.tblPersona.Count(e => e.idPersona == id) > 0;
        }

        private bool UserExists(int id)
        {
            return db.tblUsuario.Count(e => e.idPersona == id) > 0;
        }

        public class Usuario
        {
            public int Matricula { get; set; }
            public string Pass { get; set; }
        }
    }
}