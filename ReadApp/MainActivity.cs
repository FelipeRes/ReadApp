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
					DataBaseDAO database = new DataBaseDAO(this);
					database.InserirLivro(livro);
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


