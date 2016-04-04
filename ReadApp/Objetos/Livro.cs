using System;
using System.Collections.Generic;

namespace ReadApp{
	public class Livro{
		public int id;
		public string nome;
		public string autor;
		public int ano;
		public int qntPaginas;
		public int avaliacao;
		public List<Capitulo> capitulo;

		public Livro (){
		}
		public Livro (string nome, string autor, int ano, int qntPaginas){
			this.nome = nome;
			this.autor = autor;
			this.ano = ano;
			this.qntPaginas = qntPaginas;
		}

		public void adicionarCapitulo(Capitulo capitulo){
			this.capitulo.Add (capitulo);
		}
		public void avaliar(int avaliacao){
			this.avaliacao = avaliacao;
		}
	}
}

