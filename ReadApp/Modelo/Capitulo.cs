using System;

namespace ReadApp{
	public class Capitulo{
		public int id	{ get; set; } 
		public int livroId	{ get; set; } 
		public Livro livro	{ get; set; } 
		public int numero	{ get; set; } 
		public string nomeCapitulo	{ get; set; } 
		public string cometario	{ get; set; } 
		public int paginaInicial	{ get; set; } 
		public int paginaFinal	{ get; set; } 

		public Capitulo (){
		}
	}
}

