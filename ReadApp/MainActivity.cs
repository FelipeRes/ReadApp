using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using Android.Views;
using System.Collections.Generic;

namespace ReadApp{
	[Activity (Label = "ReadApp", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity{

		Fragment lendo = new Lendo();
		Fragment queroLer = new QueroLer();
		Fragment lido = new Lido();


		protected override void OnCreate (Bundle savedInstanceState){
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			AddTab ("Lendo",lendo);
			AddTab ("Quero Livros", queroLer);
			AddTab ("Lidos", lido);

			Button inserirLivroButton = (Button)FindViewById<Button> (Resource.Id.inserirLivro);
			inserirLivroButton.Click += (s,e) => {
				var alert = new AlertDialog.Builder(this);
				View view = LayoutInflater.Inflate(Resource.Layout.inserir_livro, null);
				alert.SetView(view);
				alert.SetPositiveButton("Adicionar Livro", (object sender, DialogClickEventArgs e2) => {
					string nome = view.FindViewById<TextView>(Resource.Id.inserirNomeLivro).Text;
					string autor = view.FindViewById<TextView>(Resource.Id.inserirAutorLivro).Text;
					int ano = int.Parse(view.FindViewById<TextView>(Resource.Id.inserirAnoLivro).Text);
					int qntPag = int.Parse(view.FindViewById<TextView>(Resource.Id.inserirQuantidadePaginasLivro).Text);
					Livro livro = new Livro(nome,autor,ano,qntPag);

					List<CheckBox> checkBoxList = new List<CheckBox>();
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirRomance));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirTerror));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirFiccao));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirEpico));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirFantaisa));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirDidatico));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirAventura));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirGuia));
					checkBoxList.Add(view.FindViewById<CheckBox>(Resource.Id.inserirBiografia));

					for(int i = 0; i<checkBoxList.Count; i++){
						if(checkBoxList[i].Checked){
							livro.adicionarGenero(GeneroExtend.GeneroPorNome(checkBoxList[i].Text));
						}
					}

					DataBaseDAO database = new DataBaseDAO(this);
					database.InserirLivro(livro);
					database.InserirGenero(livro);
					queroLer.OnResume();
				});
				alert.Create().Show();
			};
		}
		void AddTab (string tabText, Fragment fragment){
			var tab = this.ActionBar.NewTab ();            
			tab.SetText (tabText);
			tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
				e.FragmentTransaction.Replace(Resource.Id.fragmentContainer, fragment);
			};
			this.ActionBar.AddTab (tab);
		}
	}
}


