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
	public class LidoAdapter : BaseAdapter<Leitura>{
		List<Leitura> items;
		Activity context;
		public LidoAdapter(Activity context, List<Leitura> items): base(){
			this.context = context;
			this.items = items;
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
				view = context.LayoutInflater.Inflate (Resource.Layout.lido_item, null);
				view.FindViewById<TextView> (Resource.Id.lidoNomeLivro).Text = item.livro.nome;
				view.FindViewById<TextView> (Resource.Id.lidoAutor).Text = item.livro.autor;
				view.FindViewById<RatingBar> (Resource.Id.lidoRank).Rating = item.livro.avaliacao;
			}
			return view;
		}
	}
}

