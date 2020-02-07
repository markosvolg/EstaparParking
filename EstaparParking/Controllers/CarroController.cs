using EstaparParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EstaparParking.Controllers
{
    public class CarroController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        // GET: Carro

        public ActionResult EstaparCarro()
        {


            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QtdLinhaPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = CarroModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = CarroModel.RecuperarQuantidade();

            var difQtdPaginas = (quant % ViewBag.QtdLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QtdPaginas = (quant / ViewBag.QtdLinhaPorPagina) + difQtdPaginas;

            return View(lista);
        }






        [HttpPost]
        public JsonResult PessoaPagina(int pagina, int tamPag)
        {
            var lista = CarroModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }



        [HttpPost]
        public ActionResult RecuperarGrupoCarro(int id)
        {
            return Json(CarroModel.RecuperarPorId(id));
        }

        [HttpPost]
        public ActionResult ExcluirGrupoCarro(int id)
        {

            return Json(CarroModel.ExluirPorId(id));
        }



        [HttpPost]
        public ActionResult SalvaGrupoCarro(CarroModel gpm)
        {
            var resultado = "OK";
            var mensagens = new List<string>();
            var idSalvo = string.Empty;


            if (!ModelState.IsValid)
            {
                resultado = "Aviso";
                mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            }
            else
            {
                try
                {
                    var id = gpm.AdicionarCarro();

                    if (id > 0)
                    {
                        idSalvo = id.ToString();

                    }
                    else
                    {
                        resultado = "ERRO";
                    }
                }
                catch (Exception ex)
                {

                    resultado = "ERRO";
                }

            }
            return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
        }
    }
}