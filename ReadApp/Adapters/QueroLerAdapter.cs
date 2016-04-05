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
	public class QueroLerAdapter : BaseAdapter<Livro>{
		List<Livro> items;
		Activity context;
		public QueroLerAdapter(Activity context, List<Livro> items): base(){
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position){
			return position;
		}
		public override Livro this[int position]{
			get { return items[position]; }
		}
		public override int Count{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent){
			var item = items[position];
			View view = convertView;
			if (view == null) { // no view to re-use, create new
				view = context.LayoutInflater.Inflate (Resource.Layout.queroLer_item, null);
				view.FindViewById<TextView> (Resource.Id.nomeQueroLerItem).Text = item.nome;
				view.FindViewById<TextView> (Resource.Id.quantidadePaginasQueroLerItem).Text = item.qntPaginas.ToString() + " Paginas";
			}
			return view;
		}
	}
}

