﻿
using Microsoft.AspNetCore.Mvc;
using AutomacaoFolhaPagamento.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutomacaoFolhaPagamento.Controllers
{
    public class RelatorioController : Controller
    {
 
        private readonly IHttpClientFactory _clientFactory;
        public RelatorioController(IHttpClientFactory clientFactory)
        {
       
            _clientFactory = clientFactory;
        }

        public IActionResult Detalhado()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Relatorio(int MesSelecionado)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7067/api/Relatorio/listaRelatorio/{MesSelecionado}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<List<HistPagamentoModel>>(body);
                return View("Detalhado", objetoRetorno);
            }
            else
            {
                return RedirectToAction("Erro");
            }


        }
        public async Task<IActionResult> Funcionario()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7067/api/Relatorio/listaFuncionario");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<List<Funcionario>>(body);
               
                var viewModel = new FuncionarioViewModel
                {
                    funcionarioBasicoList = objetoRetorno
                };
                 return View("Funcionario", viewModel);
            }
            else
            {
                return RedirectToAction("Erro");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Funcionario(string Funcionario, string MesSelecionado)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7067/api/Relatorio/funcionarioDeducoes/{Funcionario}/{MesSelecionado}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<FuncionarioViewModel>(body);
                return View("Funcionario", objetoRetorno);
            }
            else
            {
                return View("Funcionario", new FuncionarioViewModel());
            }

        }


        public async Task<IActionResult> Dashboard()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7067/api/Relatorio/dashboard");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var objetoRetorno = JsonSerializer.Deserialize<GraficosViewModel>(body);
                return View(objetoRetorno);
            }
            else
            {
                return RedirectToAction("Erro");
            }
       
        }



    }

}
