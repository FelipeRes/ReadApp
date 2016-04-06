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
			sql = "CREATE TABLE [Capitulos] (\n[id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,\n[livroId] INTEGER  NULL,\n[Numero] INTEGER  NULL,\n[NomeCapitulo] VARCHAR(50)  NULL,\n[Comentario] TEXT  NULL,\n[PaginaInicial] INTEGER  NULL,\n[PaginaFinal] INTEGER  NULL\n)";
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
			String sql = "SELECT id FROM Livros ORDER BY id DESC LIMIT 1";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			c.MoveToNext ();
			livro.id = c.GetInt (0);
			InserirGeneros (livro);
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
			removerGeneroLivro(livro);
			InserirGeneros (livro);
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
		public void InserirGeneros(Livro livro){
			if (livro.genero != null) {
				for (int i = 0; i < livro.genero.Count; i++) {
					ContentValues cv = new ContentValues ();
					cv.Put ("id", livro.id);
					cv.Put ("Genero", livro.genero [i].ToString ());
					WritableDatabase.Insert ("Generos", null, cv);
				}
			}
		}

		public List<Genero> GetGenerosPorLivro(Livro livro){
			List<Genero> lista = new List<Genero> ();
			string sql = "SELECT * FROM Generos;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Genero genero = GeneroExtend.GeneroPorNome( c.GetString(1));
				if (c.GetInt(c.GetColumnIndex ("id")) == livro.id) {
					lista.Add (genero);
				}
			}
			return lista;
		}
		public void removerGeneroLivro(Livro livro){
			string sql = "DELETE FROM Generos WHERE id = " + livro.id;
			WritableDatabase.ExecSQL (sql);
		}
		//==================================================================================================//
		public void InserirCapitulo(Capitulo capitulo){
			ContentValues cv = new ContentValues ();
			cv.Put ("livroId", capitulo.livro.id);
			cv.Put ("Numero", capitulo.numero);
			cv.Put ("NomeCapitulo", capitulo.nomeCapitulo);
			cv.Put ("Comentario", capitulo.cometario);
			cv.Put ("PaginaInicial", capitulo.paginaInicial);
			cv.Put ("PaginaFinal", capitulo.paginaFinal);
			WritableDatabase.Insert ("Capitulos", null, cv);
		}
		public List<Capitulo> ListaCapitulos(){
			List<Capitulo> lista = new List<Capitulo> ();
			string sql = "SELECT * FROM Capitulos;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Capitulo v = new Capitulo ();
				v.livroId = c.GetInt (c.GetColumnIndex("livroId"));
				v.numero = c.GetInt(c.GetColumnIndex("Numero"));
				v.nomeCapitulo = c.GetString(c.GetColumnIndex("NomeCapitulo"));
				v.cometario =  c.GetString(c.GetColumnIndex("Comentario"));
				v.paginaInicial = c.GetInt(c.GetColumnIndex("PaginaInicial"));
				v.paginaFinal = c.GetInt(c.GetColumnIndex("PaginaFinal"));
				v.livro = BuscarLivroId(v.livroId);
				v.id = c.GetInt (c.GetColumnIndex("id"));
				lista.Add (v);
			}
			return lista;
		}
		public List<Capitulo> ListaCapitulosPorLivro(Livro livro){
			List<Capitulo> lista = new List<Capitulo> ();
			string sql = "SELECT * FROM Capitulos;";
			Android.Database.ICursor c = ReadableDatabase.RawQuery (sql, null);
			while (c.MoveToNext ()) {
				Capitulo v = new Capitulo ();
				v.livroId = c.GetInt (c.GetColumnIndex("livroId"));
				v.numero = c.GetInt(c.GetColumnIndex("Numero"));
				v.nomeCapitulo = c.GetString(c.GetColumnIndex("NomeCapitulo"));
				v.cometario =  c.GetString(c.GetColumnIndex("Comentario"));
				v.paginaInicial = c.GetInt(c.GetColumnIndex("PaginaInicial"));
				v.paginaFinal = c.GetInt(c.GetColumnIndex("PaginaFinal"));
				v.livro = BuscarLivroId(v.livroId);
				v.id = c.GetInt (c.GetColumnIndex("id"));
				if (livro.id == v.livroId) {
					lista.Add (v);
				}
			}
			return lista;
		}

		public Capitulo BuscarCapituloPorId(int id){
			List<Capitulo> lista = ListaCapitulos ();
			for (int i = 0; i < lista.Count; i++) {
				if (lista [i].id == id) {
					return lista [i];
				}
			}
			return null;
		}
		public void removerCapitulo(Capitulo capitulo){
			string sql = "DELETE FROM Capitulos WHERE id = " + capitulo.id;
			WritableDatabase.ExecSQL (sql);
		}
		public void AtualizarCapitulo(Capitulo capitulo){
			ContentValues values = new ContentValues();
			//values.Put ("livroId", capitulo.livro.id);
			values.Put ("Numero", capitulo.numero);
			values.Put ("NomeCapitulo", capitulo.nomeCapitulo);
			values.Put ("Comentario", capitulo.cometario);
			values.Put ("PaginaInicial", capitulo.paginaInicial);
			values.Put ("PaginaFinal", capitulo.paginaFinal);
			WritableDatabase.Update ("Capitulos", values, "id =" + capitulo.id + "", null);
		}
	}
}

