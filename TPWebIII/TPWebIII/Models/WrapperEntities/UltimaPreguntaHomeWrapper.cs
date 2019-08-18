using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Models.WrapperEntities
{
    public class UltimaPreguntaHomeWrapper
    {
        public int IdPregunta { get; set; }
        public string Pregunta { get; set; }
        public List<UltimaPreguntaHomeRankingWrapper> RankingPregunta { get; set; }
    }
}