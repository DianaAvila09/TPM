using Entidades;
using QRCoder;
using RepositorySql;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace BusinessLogic
{
   public class BLCatCodeQr
   {

      public List<EquipoPadre> DatosEquipos(string cnxSqlMT, string pCtroCtos)
      {
         SqlRepository repoSql = new SqlRepository();
         List<EquipoPadre> lstEquPadres = new List<EquipoPadre>();
         lstEquPadres = repoSql.GetCatEquiposPadres(cnxSqlMT, pCtroCtos);
         return (lstEquPadres);
      }

      public List<TipoQr> TiposQr(string cnxSqlMT)
      {

         SqlRepository repoSql = new SqlRepository();
         List<TipoQr> lstTipos = new List<TipoQr>();

         lstTipos = repoSql.GetTiposQr(cnxSqlMT);

         return (lstTipos);
      }

      public CodeQr GeneraCodeQr(string cnxSqlMT, CodeQr codigo, List<TipoQr> lstTipos)
      {
         SqlRepository repo = new SqlRepository();
         byte[] cde = null;
         string newLink = "";

         CatEquipo equipo = repo.GetDatEquipo(cnxSqlMT, codigo.CodEquipo);
         codigo.WorkCenter = equipo.WorkCenter;
         codigo.DescripEquipo = equipo.Cod_Descrip;

         string linkp11 = "http://" + codigo.WebServer;

         string linkp12 = "/AtkTpmMantto/Tickets/ListaTickets?CodEq=" + equipo.CodEquipo;
         string linkp13 = "&statusTick=True&showError=False";

         string linkp22 = "/AtkTpmMantto/Tickets/Crear?CodWc=" + equipo.WorkCenter.Trim();
         string linkp23 = "&codEqui="+codigo.CodEquipo.Trim();
         string linkp24 = "&equipo=" + equipo.DescripTechnical.Trim();
         string linkp25 = "&tipoTick=R";

         string linkp31 = "&tipoTick=A";
         string linkp41 = "&tipoTick=Z";
         string linkp51 = "&tipoTick=G";
         string linkp61 = "&tipoTick=M";

         string linkp70 = "/AtkTpmMantto/CaptChklst/ChklistProgram";
         string linkp71 = "";

         switch (codigo.Tipo)
         {
          case "EQUIPO":             
               newLink = linkp11 + linkp12 + linkp13;              
               break;          
            case "TICKETROJO":              
               newLink = linkp11 + linkp22 + linkp23 + linkp24 + linkp25;
               break;
            case "TICKETAMAR":
               newLink = linkp11 + linkp22 + linkp23 + linkp24 + linkp31;
               break;
            case "TICKETAZUL":
               newLink = linkp11 + linkp22 + linkp23 + linkp24 + linkp41;
               break;
            case "TICKETMEJO":
               newLink = linkp11 + linkp22 + linkp23 + linkp24 + linkp51;
               break;
            case "TICKETNARA":
               newLink = linkp11 + linkp22 + linkp23 + linkp24 + linkp61;
               break;

            case "CHECKLIST":
               newLink = linkp11 + linkp70 + linkp71 + linkp23;
               break;
         }


         QRCodeGenerator qrGenerator = new QRCodeGenerator();
         QRCodeData qrCodeData = qrGenerator.CreateQrCode(newLink, QRCodeGenerator.ECCLevel.Q);
         QRCode qrCode = new QRCode(qrCodeData);
         using (Bitmap bitMap = qrCode.GetGraphic(20))
         {
            using (MemoryStream ms = new MemoryStream())
            {
               bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
               cde = ms.ToArray();        
            }
         }

         int index = lstTipos.FindIndex(x => x.Tipo.Trim() == codigo.Tipo.Trim());
         if (index >= 0)
         {
            codigo.DescripTipo = lstTipos[index].Descrip;
         }
       

         codigo.Liga = newLink;
         codigo.Qr = cde;
         return codigo;
      }
   }
}
