//------------------------------------------------------------------------------
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
    
    public partial class GetReservaByIdRes_sp
    {
        public int idReserva { get; set; }
        public int idCurso { get; set; }
        public byte idSemana { get; set; }
        public byte idDias { get; set; }
        public byte idHoraIn { get; set; }
        public byte idHoraF { get; set; }
        public int idReservante { get; set; }
        public bool EstadoReserva { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public System.DateTime FechaReserva { get; set; }
    }
}