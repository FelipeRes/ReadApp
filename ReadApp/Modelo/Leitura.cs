using System;
using System.Collections.Generic;
namespace ReadApp{
	public class Leitura{

		public int id	{ get; set; } 
		public Livro livro	{ get; set; } 
		public int atualPaginas	{ get; set; } 
		public Data inicioLeitua	{ get; set; } 
		public Data terminoeitura	{ get; set; } 
		public EstadoLeitura estado	{ get; set; } 
		public string comentario	{ get; set; } 


		public Leitura (){
			this.comentario = "";
		}

		public Leitura (Livro livro){
			this.livro = livro;
			atualPaginas = 0;
			DateTime data = DateTime.Now;
			inicioLeitua = new Data (data.Day, data.Month,data.Year);
			estado = EstadoLeitura.Iniciado;
			this.comentario = "Bixo Piruleta";
		}
	}
}

