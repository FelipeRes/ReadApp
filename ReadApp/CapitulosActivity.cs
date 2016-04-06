
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReadApp{
	[Activity (Label = "Capitulos")]			
	public class CapitulosActivity : Activity{

		List<Capitulo> lista;
		DataBaseDAO database;

		protected override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.capitulos_list);
			database = new DataBaseDAO (this);

			int livroId = Intent.GetIntExtra ("LivroId", 0);
			Livro livro = database.BuscarLivroId (livroId);

			FindViewById<TextView> (Resource.Id.capituloNomeLivro).Text = livro.nome;
			lista = database.ListaCapitulosPorLivro (livro);
			CapituloAdapter adapter = new CapituloAdapter(this,lista);
			ListView listView = (ListView)FindViewById (Resource.Id.capitulos_listView);
			listView.Adapter = adapter;

			Button adicionarCapituloButton = FindViewById<Button> (Resource.Id.adicionarCapitulo);
			adicionarCapituloButton.Click += (capS, capE) => {
				var alert = new AlertDialog.Builder(this);
				View view = LayoutInflater.Inflate(Resource.Layout.adicionar_capitulo, null);
				alert.SetView(view);
				Capitulo capitulo = new Capitulo();
				alert.SetPositiveButton("Adicionar", (object sender, DialogClickEventArgs e2) => {
					capitulo.numero = int.Parse(view.FindViewById<EditText>(Resource.Id.inserirCapituloNumber).Text);
					capitulo.nomeCapitulo = view.FindViewById<EditText>(Resource.Id.inserirCapituloNome).Text;
					capitulo.cometario = view.FindViewById<EditText>(Resource.Id.inserirCapituloComentario).Text;
					capitulo.paginaInicial = int.Parse(view.FindViewById<EditText>(Resource.Id.inserirCapituloPaginaInicial).Text);
					capitulo.paginaFinal = int.Parse(view.FindViewById<EditText>(Resource.Id.inserirCapituloPaginaFinal).Text);
					capitulo.livro = livro;
					database.InserirCapitulo(capitulo);
				});

				alert.Create().Show();
			};

			listView.ItemClick += ((itemClickS, itemClickE) => {
				var alert = new AlertDialog.Builder(this);
				View view = LayoutInflater.Inflate(Resource.Layout.capitulo_informacao, null);
				alert.SetView(view);
				Capitulo capitulo = database.BuscarCapituloPorId(int.Parse(itemClickE.View.FindViewById<TextView> (Resource.Id.capituloId).Text));

				view.FindViewById<TextView> (Resource.Id.capituloInfoNumero).Text = capitulo.numero.ToString() + " - ";
				view.FindViewById<TextView> (Resource.Id.capituloInfoItemNomeCapitulo).Text = capitulo.nomeCapitulo;
				view.FindViewById<TextView> (Resource.Id.capituloInfoPaginaInicial).Text = capitulo.paginaInicial.ToString();
				view.FindViewById<TextView> (Resource.Id.capituloInfoPaginaFinal).Text = capitulo.paginaFinal.ToString();
				view.FindViewById<TextView> (Resource.Id.capituloInfoComentario).Text = capitulo.cometario;
				view.FindViewById<TextView> (Resource.Id.capituloInfoId).Text = capitulo.id.ToString();
				Button editButton = (Button)view.FindViewById<Button> (Resource.Id.capituloInfoEditar);

				editButton.Click += (editS, editE) => {
					var editAlert = new AlertDialog.Builder(this);
					View editView = LayoutInflater.Inflate(Resource.Layout.adicionar_capitulo, null);
					alert.SetView(editView);
					Capitulo editCapitulo = database.BuscarCapituloPorId(int.Parse(itemClickE.View.FindViewById<TextView> (Resource.Id.capituloId).Text));
					alert.SetPositiveButton("AtualizarLivro", (object sender, DialogClickEventArgs e2) => {
						editCapitulo.numero = int.Parse(editView.FindViewById<EditText>(Resource.Id.inserirCapituloNumber).Text);
						editCapitulo.nomeCapitulo = editView.FindViewById<EditText>(Resource.Id.inserirCapituloNome).Text;
						editCapitulo.cometario = editView.FindViewById<EditText>(Resource.Id.inserirCapituloComentario).Text;
						editCapitulo.paginaInicial = int.Parse(editView.FindViewById<EditText>(Resource.Id.inserirCapituloPaginaInicial).Text);
						editCapitulo.paginaFinal = int.Parse(editView.FindViewById<EditText>(Resource.Id.inserirCapituloPaginaFinal).Text);
						database.AtualizarCapitulo(editCapitulo);
						OnResume();
					});
					alert.Create().Show();
				};
				alert.Show();

			});

			listView.ItemLongClick += ((itemClickS, itemClickE) => {
				var alert = new AlertDialog.Builder(this);
				View removeView = LayoutInflater.Inflate(Resource.Layout.sim_nao, null); //infla o menu dela
				alert.SetPositiveButton("Remover", (object sender, DialogClickEventArgs e2) => {
					Capitulo capitulo = database.BuscarCapituloPorId(int.Parse(itemClickE.View.FindViewById<TextView> (Resource.Id.capituloId).Text));
					database.removerCapitulo(capitulo);
					OnResume();
				});
				alert.SetNegativeButton("Cancelar", (object sender, DialogClickEventArgs e2) => { });
				alert.SetView(removeView);
				alert.Create().Show(); //mostra alert
			});

		}
		protected override void OnResume (){
			base.OnResume ();
			int livroId = Intent.GetIntExtra ("LivroId", 0);
			Livro livro = database.BuscarLivroId (livroId);
			lista = database.ListaCapitulosPorLivro (livro);
			CapituloAdapter adapter = new CapituloAdapter(this,lista);
			ListView listView = (ListView)FindViewById (Resource.Id.capitulos_listView);
			listView.Adapter = adapter;
		}
	}
}

