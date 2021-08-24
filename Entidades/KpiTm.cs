using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class KpiTm
    {
        public string Periodo { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal MinAutomat { get; set; }
        public decimal MinMantto { get; set; }
        public decimal MinProgramado { get; set; }
        public decimal MinTroqueles { get; set; }
        public decimal MinParoProd { get; set; }
        public decimal MinLogistica { get; set; }
        public decimal MinError { get; set; }
        public decimal MinCorriendo { get; set; }
        public decimal MinCalidad { get; set; }
        public decimal TotalMinutos { get; set; } 
        public int EventosAutoma { get; set; }
        public int EventosMantto { get; set; }
        public int EventosTroqueles { get; set; }
        public decimal MttrAuto { get; set; }
        public decimal MtbfAuto { get; set; }
        public decimal MttrManto { get; set; }
        public decimal MtbfManto { get; set; }
        public decimal MttrTroquel { get; set; }
        public decimal MtbfTroquel { get; set; }
        public decimal MetaMtrrAuto { get; set; }
        public decimal MetaMtbfAuto { get; set; }
        public decimal MetaMtrrMnt { get; set; }
        public decimal MetaMtbfMnt { get; set; }
        public decimal MetaMtrrTkl { get; set; }
        public decimal MetaMtbfTkl { get; set; }
        public decimal Trend { get; set; }
    }
}
