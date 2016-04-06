using System;
namespace ReadApp{
	public class Data{

		static public Data gerarDataPorString(string dataString){
			string[] campos = dataString.Split ('/');
			Data data = new Data ();
			data.dia = int.Parse (campos [0]);
			data.mes = int.Parse (campos [1]);
			data.ano = int.Parse (campos [2]);
			return data;
		}

		public int dia { get; set; } 
		public int mes { get; set; } 
		public int ano { get; set; } 

		public Data (){
		}

		public Data (int dia, int mes, int ano){
			this.dia = dia;
			this.mes = mes;
			this.ano = ano;
		}

		public string getDataString(){
			string data = dia.ToString () + "/" + mes.ToString () + "/" + ano.ToString ();
			return data;
		}
	}
}

