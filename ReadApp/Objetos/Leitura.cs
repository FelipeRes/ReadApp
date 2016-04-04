using System;
using System.Collections.Generic;
namespace ReadApp{
	public class Leitura{

		public int id;
		public Livro livro;
		public int atualPaginas;
		public Data inicioLeitua;
		public Data terminoeitura;
		public EstadoLeitura estado;

		public Leitura (){
		}

		public Leitura (Livro livro){
			this.livro = livro;
			atualPaginas = 0;
			DateTime data = DateTime.Now;
			inicioLeitua = new Data (data.Day, data.Month,data.Year);
			estado = EstadoLeitura.Iniciado;
		}
	}
}

