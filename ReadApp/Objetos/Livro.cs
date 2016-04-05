using System;
using System.Collections.Generic;

namespace ReadApp{
	public class Livro{
		public int id;
		public string nome;
		public string autor;
		public int ano;
		public int qntPaginas;
		public float avaliacao;
		public List<Genero> genero;
		public List<Capitulo> capitulo;

		public Livro (){
		}
		public Livro (string nome, string autor, int ano, int qntPaginas){
			this.nome = nome;
			this.autor = autor;
			this.ano = ano;
			this.qntPaginas = qntPaginas;
			genero = new List<Genero> ();
			capitulo = new List<Capitulo> ();
		}

		public void adicionarCapitulo(Capitulo capitulo){
			this.capitulo.Add (capitulo);
		}
		public void avaliar(float avaliacao){
			this.avaliacao = avaliacao;
		}
		public void adicionarGenero(Genero genero){
			this.genero.Add (genero);
		}
		public void removerGenero(Genero genero){
			this.genero.Remove (genero);
		}
		public string getGeneroString(){
			string tags = "Tags: ";
			for (int i = 0; i < genero.Count; i++) {
				tags += " / " + genero [i].ToString ();
			}
			return tags;
		}
	}
}

