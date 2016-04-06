using System;
using System.Collections.Generic;

namespace ReadApp{
	public class Livro{
		public int id	{ get; set; }
		public string nome	{ get; set; } 
		public string autor	{ get; set; } 
		public int ano	{ get; set; } 
		public int qntPaginas	{ get; set; } 
		public float avaliacao	{ get; set; } 
		public List<Genero> genero	{ get; set; } 
		public List<Capitulo> capitulo	{ get; set; } 

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
				tags += " - " + genero [i].ToString ();
			}
			return tags;
		}
	}
}

