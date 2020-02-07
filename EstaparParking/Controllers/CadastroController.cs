using EstaparParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EstaparParking.Controllers
{
    public class CadastroController : Controller
    {
        private const int _quantMaxLinhasPorPagina = 5;
        // GET: Cadastro
        public ActionResult EstaparPessoa()
        {


            ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
            ViewBag.QtdLinhaPorPagina = _quantMaxLinhasPorPagina;
            ViewBag.PaginaAtual = 1;

            var lista = PessoaModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
            var quant = PessoaModel.RecuperarQuantidade();

            var difQtdPaginas = (quant % ViewBag.QtdLinhaPorPagina) > 0 ? 1 : 0;
            ViewBag.QtdPaginas = (quant / ViewBag.QtdLinhaPorPagina) + difQtdPaginas;

            return View(lista);
        }


        [HttpPost]
        public JsonResult PessoaPagina(int pagina, int tamPag)
        {
            var lista = PessoaModel.RecuperarLista(pagina, tamPag);

            return Json(lista);
        }



        [HttpPost]
        public ActionResult RecuperarGrupoPessoa(int id)
        {
            return Json(PessoaModel.RecuperarPorId(id));
        }

        [HttpPost]
        public ActionResult ExcluirGrupoPessoa(int id)
        {

            return Json(PessoaModel.ExluirPorId(id));
        }




        public ActionResult EstaparMovimentacao()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SalvaGrupoPessoa(PessoaModel gpm)
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
                    var id = gpm.AdicionarPessoa();

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