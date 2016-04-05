
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ReadApp{
	public class QueroLer : Fragment{

		DataBaseDAO database;
		ListView listaView;
		View view;
		View viewAlert;
		List<Livro> lista = new List<Livro> ();

		public override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){
			//infla o layout de list view
			base.OnCreateView (inflater, container, savedInstanceState);
			view = inflater.Inflate (Resource.Layout.list_tab, container, false);

			//pega banco de dados
			database = new DataBaseDAO (view.Context);
			lista = database.ListaLivros();

			//adiciona uma array na lista
			//QueroLerAdapter
			QueroLerAdapter adapter = new QueroLerAdapter(this.Activity,lista);
			listaView = (ListView)view.FindViewById<ListView> (Resource.Id.listView1);
			listaView.Adapter = adapter;
			listaView.ItemClick += (s, e) => {
				View itemView = e.View; // pega a view do item
				TextView livroView = (TextView)itemView.FindViewById<TextView>(Resource.Id.nomeQueroLerItem);//carrega nome do livro
				DataBaseDAO database = new DataBaseDAO(view.Context);//carrega o banco de dados;
				Livro livro = database.BuscarLivroNome(livroView.Text);

				var alert = new AlertDialog.Builder(view.Context); //cria a alert
				View viewAlert = inflater.Inflate(Resource.Layout.iniciar_leitura, null); //infla o menu dela

				viewAlert.FindViewById<TextView>(Resource.Id.iniciarLeituraNomeLivro).Text = livro.nome; //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.iniciarLeituraAutor).Text = livro.autor; //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.iniciarLeituraAno).Text = "Ano:" + livro.ano.ToString(); //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.iniciarLeituraQuantidadePaginas).Text = livro.qntPaginas.ToString() + " Páginas"; //adiciona o nome do livro no alert
				viewAlert.FindViewById<TextView>(Resource.Id.iniciarLeituraTags).Text = livro.getGeneroString();


				alert.SetView(viewAlert);
				Leitura estaLendo = database.BuscarLeituraPorLivro(livro);
				if(estaLendo == null){
					alert.SetPositiveButton("Iniciar Leitura", (object sender, DialogClickEventArgs e2) => {
						Leitura leitura = new Leitura(livro);
						database.InserirLeitura(leitura);
					});
				}else{
					Android.Widget.Toast.MakeText(view.Context,"Você ja está lendo esse livro", Android.Widget.ToastLength.Short).Show();
				}
				alert.Create().Show();
			};

			listaView.ItemLongClick += (s, e) => {
				View itemView = e.View; // pega a view do item
				Livro livro = database.BuscarLivroNome(e.View.FindViewById<TextView>(Resource.Id.nomeQueroLerItem).Text);
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
						editLivro.genero = new List<Genero>();

						List<CheckBox> checkBoxList = new List<CheckBox>();
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarRomance));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarTerror));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarFiccao));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarEpico));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarFantaisa));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarDidatico));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarAventura));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarGuia));
						checkBoxList.Add(editarView.FindViewById<CheckBox>(Resource.Id.editarBiografia));
						for(int i = 0; i<checkBoxList.Count; i++){
							if(checkBoxList[i].Checked){
								editLivro.adicionarGenero(GeneroExtend.GeneroPorNome(checkBoxList[i].Text));
							}
						}

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
						leitura = database.BuscarLeituraPorLivro(livro);
						if(leitura != null){
							database.removerLeitura(leitura);
							database.removerGeneroLivro(livro);
						}
						database.removerLivro(livro);
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
			lista = database.ListaLivros();
			QueroLerAdapter adapter = new QueroLerAdapter(this.Activity,lista);
			listaView = (ListView)view.FindViewById<ListView> (Resource.Id.listView1);
			listaView.Adapter = adapter;
		}
	}
}

