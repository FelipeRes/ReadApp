using System;

namespace ReadApp{
	public enum Genero{
		Romance,
		Terror,
		Ficcao,
		Epico,
		Fantaisa,
		Didatico,
		Aventura,
		Guia,
		Biografia
	}

	public class GeneroExtend{
		static public Genero GeneroPorNome(string genero){
			if (genero == "Romance") {
				return Genero.Romance;
			}else if (genero == "Terror") {
				return Genero.Terror;
			}else if (genero == "Ficcao") {
				return Genero.Ficcao;
			}else if (genero == "Epico") {
				return Genero.Epico;
			}else if (genero == "Fantaisa") {
				return Genero.Fantaisa;
			}else if (genero == "Didatico") {
				return Genero.Didatico;
			}else if (genero == "Aventura") {
				return Genero.Aventura;
			}else if (genero == "Guia") {
				return Genero.Guia;
			}else{
				return Genero.Biografia;
			}
		}
	}
}

