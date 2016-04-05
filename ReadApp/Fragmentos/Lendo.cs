
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
	[Activity (Label = "LendoActivity")]			
	class Lendo: Fragment{           

		DataBaseDAO database;
		ListView listaView;
		View view;
		View viewAlert;
		List<Leitura> lista;
		public override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){
			//infla o layout de list view
			base.OnCreateView (inflater, container, savedInstanceState);
			view = inflater.Inflate (Resource.Layout.lendo_tab, container, false);
			//pega banco de dados
			database = new DataBaseDAO (view.Context);
			lista = database.ListaLendo();
			//QueroLerAdapter
			LendoAdapter adapter = new LendoAdapter(this.Activity,lista,this);
			listaView = (ListView)view.FindViewById<ListView> (Resource.Id.lendoListViewTab);
			listaView.Clickable = true;
			listaView.Adapter = adapter;
			//listaView.item
			listaView.ItemClick += (s, e) => {
				View itemView = e.View; // pega a view do item
				Livro livro = database.BuscarLivroNome(e.View.FindViewById<TextView>(Resource.Id.nomeLivro).Text);
				Leitura leitura = database.BuscarLeituraPorLivro(livro);
				var alert = new AlertDialog.Builder(view.Context); //cria a alert
				viewAlert = inflater.Inflate(Resource.Layout.lendo_item, null); //infla o menu dela

				viewAlert.FindViewById<TextView>(Resource.Id.lendoNomeLivro).Text = livro.nome; //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.lendoAutor).Text = "Autor: " + livro.autor; //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.lendoAnoPublicacao).Text = "Ano: " + livro.ano.ToString(); //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.lendoPaginaAtual).Text = "Paginas: " + leitura.atualPaginas.ToString(); //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.lendoPagonaTotal).Text = livro.qntPaginas.ToString(); //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.lendoInicioDaLeitura).Text = "Inicio da leitura em: " + leitura.inicioLeitua.getDataString();
				viewAlert.FindViewById<TextView> (Resource.Id.lendoTag).Text = livro.getGeneroString();

				Button capitulosButton = viewAlert.FindViewById<Button>(Resource.Id.CapitulosButton);
				capitulosButton.Click += (Scap, Ecap) => {
					Intent capitulos = new Intent(this.Activity, typeof(Capitulos));
					StartActivity(capitulos);
				};

				alert.SetView(viewAlert);
				alert.Create().Show(); //mostra alert
			};

			listaView.ItemLongClick += (s, e) => {
				View itemView = e.View; // pega a view do item
				Livro livro = database.BuscarLivroNome(e.View.FindViewById<TextView>(Resource.Id.nomeLivro).Text);
				Leitura leitura = database.BuscarLeituraPorLivro(livro);
				AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
				AlertDialog alert = builder.Create(); //cria a alert
				viewAlert = inflater.Inflate(Resource.Layout.livro_opcoes, null); //infla o menu dela

				Button alterarInfoButton = viewAlert.FindViewById<Button>(Resource.Id.livroAlterarInformacoes);
				Button capitulosfoButton = viewAlert.FindViewById<Button>(Resource.Id.livroVerCapitulos);
				Button removerButton = viewAlert.FindViewById<Button>(Resource.Id.livroRemover);

				alterarInfoButton.Click += (AlterarSender,AlterarArgs) => {
					var editarAlert = new AlertDialog.Builder(e.View.Context); //cria a alert
					View editarView = inflater.Inflate(Resource.Layout.editar_livro, null); //infla o menu dela
					editarAlert.SetPositiveButton("Editar", (object sender, DialogClickEventArgs e2) => {
						Livro editLivro = new Livro();
						editLivro.nome = editarView.FindViewById<TextView>(Resource.Id.editarNomeLivro).Text;
						editLivro.autor = editarView.FindViewById<TextView>(Resource.Id.editarAutorLivro).Text;
						editLivro.ano = int.Parse(editarView.FindViewById<TextView>(Resource.Id.editarAnoLivro).Text);
						editLivro.qntPaginas = int.Parse(editarView.FindViewById<TextView>(Resource.Id.editarQuantidadePaginasLivro).Text);
						editLivro.id = livro.id;
						database.editarLivro(editLivro);
						OnResume();
					});
					editarAlert.SetView(editarView);
					editarAlert.Create().Show(); //mostra alert
				};
				capitulosfoButton.Click += (AlterarSender,AlterarArgs) => {
					Intent capitulos = new Intent(this.Activity, typeof(Capitulos));
					StartActivity(capitulos);
				};
				removerButton.Click += (AlterarSender,AlterarArgs) => {
					var removeAlert = new AlertDialog.Builder(e.View.Context); //cria a alert
					View removeView = inflater.Inflate(Resource.Layout.sim_nao, null); //infla o menu dela
					removeAlert.SetPositiveButton("Remover", (object sender, DialogClickEventArgs e2) => {
						database.removerLeitura(leitura);
						OnResume();
						alert.Dismiss();
					});
					removeAlert.SetNegativeButton("Cancelar", (object sender, DialogClickEventArgs e2) => { });
					removeAlert.SetView(removeView);
					removeAlert.Create().Show(); //mostra alert
				};
				alert.SetView(viewAlert);
				alert.Show(); //mostra alert
			};
			return view;
		}
		public override void OnResume (){
			base.OnResume ();
			lista = database.ListaLendo();
			LendoAdapter adapter = new LendoAdapter(this.Activity,lista,this);
			listaView = (ListView)view.FindViewById<ListView> (Resource.Id.lendoListViewTab);
			listaView.Adapter = adapter;
		}

	}


}

