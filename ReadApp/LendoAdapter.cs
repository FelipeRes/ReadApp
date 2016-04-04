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
	public class LendoAdapter : BaseAdapter<Leitura>{
		List<Leitura> items;
		Activity context;
		Fragment lendoFrangment;
		public LendoAdapter(Activity context, List<Leitura> items,Fragment lendoFrangment): base(){
			this.context = context;
			this.items = items;
			this.lendoFrangment = lendoFrangment;
		}
		public override long GetItemId(int position){
			return position;
		}
		public override Leitura this[int position]{
			get { return items[position]; }
		}
		public override int Count{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			var item = items[position];
			View view = convertView;
			if (view == null) { // no view to re-use, create new
				view = context.LayoutInflater.Inflate (Resource.Layout.leitura_item2, null);
				view.FindViewById<TextView> (Resource.Id.nomeLivro).Text = item.livro.nome;
				view.FindViewById<TextView> (Resource.Id.numeroPaginaTotal).Text = item.livro.qntPaginas.ToString();
				view.FindViewById<TextView> (Resource.Id.numeroPaginaAtual).Text = item.atualPaginas.ToString ();
				view.FindViewById<ProgressBar> (Resource.Id.barraDeProgresso2).Max = item.livro.qntPaginas;
				view.FindViewById<ProgressBar> (Resource.Id.barraDeProgresso2).Progress = item.atualPaginas;
				view.FindViewById<ImageView> (Resource.Id.lendoReadUpdateImage).Click += (s,e) =>{
					AlertDialog.Builder builder = new AlertDialog.Builder(context);
					AlertDialog alert = builder.Create(); //cria a alert
					View updatePaginaView = context.LayoutInflater.Inflate(Resource.Layout.atualizar_leitura, null); //infla o menu dela
					alert.SetView(updatePaginaView);
					alert.SetButton("Autalizar",(object sender, DialogClickEventArgs e2) => {
						string paginaText = updatePaginaView.FindViewById<EditText>(Resource.Id.atualizarNumeroPagina).Text;
						int paginaValor = int.Parse(paginaText); //pega o numero digitado
						DataBaseDAO database = new DataBaseDAO(updatePaginaView.Context);
						Leitura leitura = database.BuscarLeituraPorLivro(item.livro);

						if(item.livro.qntPaginas <= paginaValor){
							var removeAlert = new AlertDialog.Builder(context); //cria a alert
							View teminoView = context.LayoutInflater.Inflate(Resource.Layout.sim_nao, null); //infla o menu dela
							teminoView.FindViewById<TextView>(Resource.Id.tem_certeza).Text = "Você Terminou o Livro";
							removeAlert.SetPositiveButton("Ok", (object senderUpdate, DialogClickEventArgs eUpdate) => {
								leitura.estado = EstadoLeitura.Terminado;
								database.AtualizarLeitura(leitura);
								alert.Dismiss();
								lendoFrangment.OnResume();
								//tem que atualizar a view
							});
							removeAlert.SetView(teminoView);
							removeAlert.Create().Show(); //mostra alert
						}

						leitura.atualPaginas = paginaValor;
						database.AtualizarLeitura(leitura);

						view.FindViewById<TextView> (Resource.Id.numeroPaginaAtual).Text = paginaValor.ToString();
						view.FindViewById<ProgressBar> (Resource.Id.barraDeProgresso2).Progress = paginaValor;
					});
					//Android.Widget.Toast.MakeText(view.Context,"oo", Android.Widget.ToastLength.Short).Show();
					alert.Show();
				};
			}
			return view;
		}
	}
}

