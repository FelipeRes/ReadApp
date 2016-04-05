using System;
using Android.Content;
using Android.Database.Sqlite;
using System.Collections.Generic;
using System.Globalization;

namespace ReadApp{
	public class DataBaseDAO : SQLiteOpenHelper{
		public DataBaseDAO (Context context) : base(context, "Veiculos.db", null, 1){
		}
		public override void OnCreate(SQLiteDatabase db){
			string sql = "CREATE TABLE [Livros] (\n[id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,\n[Nome] VARCHAR(50)  NULL,\n[Autor] VARCHAR(50)  NULL,\n[Ano] INTEGER  NULL,\n[TotalDePaginas] INTEGER  NULL,\n[Avaliacao] FLOAT  NULL\n)";
			db.ExecSQL (sql);
			sql = "CREATE TABLE [Leituras] (\n[id]  INTEGER  NULL,\n[NomeLivro] VARCHAR(50)  NULL,\n[EstadoLeitura] VARCHAR(50)  NULL,\n[InicioLeitura] VARCHAR(50)  NULL,\n[TerminoLeitura] VARCHAR(50)  NULL,\n[PaginaAtual] INTEGER  NULL,\n[Comentario] TEXT  NULL\n)";
			db.ExecSQL (sql);
			sql = "CREATE TABLE [Generos] (\n[id]  INTEGER  NULL,\n[Genero] VARCHAR(50))";
			db.ExecSQL (sql);
		}
		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int  newVersion){
			string sql = "DROP TABLE IF EXISTS Veiculo;";
			db.ExecSQL (sql);
			OnCreate (db);
		}

		public void InserirLivro(Livro livro){
			ContentValues cv = new ContentValues ();
			cv.Put ("Nome", livro.nome);
			cv.Put ("Autor", livro.autor);
			cv.Put ("Ano", livro.ano);
			cv.Put ("TotalDePaginas", livro.qntPaginas);
			cv.Put ("Avaliacao", livro.avaliacao);
			WritableDatabase.Insert ("Livros", null, cv);
		}
		public List<Livro> ListaLivros(){
			List<Livro> lista = new List<Livro> ();
			string sql = "SELECT * FROM Livros;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Livro v = new Livro ();
				v.id = c.GetInt (c.GetColumnIndex("id"));
				v.nome = c.GetString (1);
				v.autor = c.GetString (2);
				v.ano =  c.GetInt(c.GetColumnIndex("Ano"));
				v.qntPaginas = c.GetInt(c.GetColumnIndex("TotalDePaginas"));
				v.avaliacao = c.GetFloat(c.GetColumnIndex("Avaliacao"));
				v.genero = GetGenerosPorLivro (v);
				lista.Add (v);
			}
			return lista;
		}

		public Livro BuscarLivroNome(string livroNome){
			Livro livro = new Livro ();
			List<Livro> listaLivros = ListaLivros();
			for (int i = 0; i < listaLivros.Count; i++) {
				if (listaLivros [i].nome == livroNome) {
					livro = listaLivros [i];
					return livro;
				}
			}
			return livro;
		}

		public Livro BuscarLivroId(int id){
			Livro livro = new Livro ();
			List<Livro> listaLivros = ListaLivros();
			for (int i = 0; i < listaLivros.Count; i++) {
				if (listaLivros [i].id == id) {
					livro = listaLivros [i];
					return livro;
				}
			}
			return livro;
		}

		public void removerLivro (Livro livro){
			string sql = "DELETE FROM Livros WHERE id = " + livro.id;
			WritableDatabase.ExecSQL (sql);
		}
		public void editarLivro (Livro livro){
			ContentValues values = new ContentValues();
			values.Put ("Nome", livro.nome); 
			values.Put ("Autor", livro.autor); 
			values.Put ("Ano", livro.ano); 
			values.Put ("TotalDePaginas", livro.qntPaginas);
			WritableDatabase.Update ("Livros", values, "id =" + livro.id, null);
		}
		public void avaliarLivro(Livro livro){
			ContentValues values = new ContentValues();
			values.Put ("Avaliacao", livro.avaliacao);
			WritableDatabase.Update ("Livros", values, "id =" + livro.id, null);
		}
		//==================================================================================================//

		public void InserirLeitura(Leitura leitura){
			ContentValues cv = new ContentValues ();
			cv.Put ("id", leitura.livro.id);//recebe id do livro
			cv.Put ("NomeLivro", leitura.livro.nome);
			cv.Put ("EstadoLeitura", leitura.estado.ToString());
			cv.Put ("InicioLeitura", leitura.inicioLeitua.getDataString());
			cv.Put ("EstadoLeitura", EstadoLeitura.Iniciado.ToString());
			cv.Put ("TerminoLeitura", leitura.inicioLeitua.getDataString());
			cv.Put ("PaginaAtual", 0);
			WritableDatabase.Insert ("Leituras", null, cv);
		}
		public List<Leitura> ListaLeituras(){
			List<Leitura> lista = new List<Leitura> ();
			string sql = "SELECT * FROM Leituras;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Leitura v = new Leitura ();
				v.id = c.GetInt (0);
				v.livro = BuscarLivroId(c.GetInt (0));
				v.estado = EstadoLeituraExtend.EstadoPorNome( c.GetString (2));
				v.inicioLeitua = Data.gerarDataPorString(c.GetString(3));
				v.terminoeitura = Data.gerarDataPorString(c.GetString(4));
				v.atualPaginas = c.GetInt (5);
				v.comentario = c.GetString (6);
				lista.Add (v);
			}
			return lista;
		}
		public void removerLeitura (Leitura leitura){
			string sql = "DELETE FROM Leituras WHERE id = " + leitura.id;
			WritableDatabase.ExecSQL (sql);
		}
		public void AtualizarLeitura(Leitura leitura){
			ContentValues values = new ContentValues();
			values.Put ("PaginaAtual", leitura.atualPaginas); 
			values.Put ("EstadoLeitura", leitura.estado.ToString()); 
			values.Put ("TerminoLeitura", leitura.terminoeitura.getDataString()); 
			values.Put ("Comentario", leitura.comentario); 
			WritableDatabase.Update ("Leituras", values, "id =" + leitura.id + "", null);
		}
		public Leitura BuscarLeituraPorLivro(Livro livro){
			Leitura leitura = new Leitura ();
			List<Leitura> listaLeituras = ListaLeituras();
			for (int i = 0; i < listaLeituras.Count; i++) {
				if (listaLeituras [i].id == livro.id) {
					leitura = listaLeituras [i];
					return leitura;
				}
			}
			return null;
		}
		public List<Leitura> ListaLidos(){
			List<Leitura> lidos = new List<Leitura> ();
			List<Leitura> listaLeituras = ListaLeituras();
			for (int i = 0; i < listaLeituras.Count; i++) {
				if (listaLeituras[i].estado == EstadoLeitura.Terminado) {
					lidos.Add(listaLeituras[i]);
				}
			}
			return lidos;
		}
		public List<Leitura> ListaLendo(){
			List<Leitura> lidos = new List<Leitura> ();
			List<Leitura> listaLeituras = ListaLeituras();
			for (int i = 0; i < listaLeituras.Count; i++) {
				if (listaLeituras[i].estado == EstadoLeitura.Iniciado) {
					lidos.Add(listaLeituras[i]);
				}
			}
			return lidos;
		}

		//==================================================================================================//
		public void InserirGenero(Livro livro){
			for (int i = 0; i < livro.genero.Count; i++) {
				ContentValues cv = new ContentValues ();
				cv.Put ("id", livro.id);
				cv.Put ("Genero", livro.genero[i].ToString());
				WritableDatabase.Insert ("Generos", null, cv);
			}
		}

		public List<Genero> GetGenerosPorLivro(Livro livro){
			List<Genero> lista = new List<Genero> ();
			string sql = "SELECT * FROM Generos;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Genero genero = new Genero ();
				genero = GeneroExtend.GeneroPorNome( c.GetString(0));
				/*if (c.GetColumnIndex ("id") == livro.id) {
					lista.Add (genero);
				}*/
				lista.Add (genero);
			}
			return lista;
		}

	}
}

